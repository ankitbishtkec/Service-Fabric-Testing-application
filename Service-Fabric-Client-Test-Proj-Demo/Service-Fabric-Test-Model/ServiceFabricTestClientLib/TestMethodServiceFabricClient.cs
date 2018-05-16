using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service_Fabric_Test_Model;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading;
using Service_Fabric_Test_Model.ServiceFabricTestClientLib.Interfaces;

namespace Service_Fabric_Test_Model.ServiceFabricTestClientLib
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestMethodServiceFabricClientAttribute : TestMethodAttribute
    {
        //this represents the maximum number of retries Service Fabric test client should make to fetch 'TestResult' of a test case. We will only make next retry if earlier retry has failed.
        private uint maxRetriesToFetchTestResult = 2;//default is 2 retries

        //-> 'testClassRunId' represents the running instance id of test class containing test methods. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run on server side.
        private Guid testClassRunId = new Guid();

        //-> represents test Runner Service Locator used in this test method. Default is 'DefaultTestRunnerServiceLocator'.
        private ITestRunnerServiceLocator testRunnerServiceLocator = new DefaultTestRunnerServiceLocator();

        public TestMethodServiceFabricClientAttribute(uint maxRetriesToFetchTestResult = 2)
        {
            this.maxRetriesToFetchTestResult = maxRetriesToFetchTestResult;
        }
        public uint MaxRetriesToFetchTestResult
        {
            get
            {
                return this.maxRetriesToFetchTestResult;
            }
        }

        public Guid TestClassRunId
        {
            get
            {
                return this.testClassRunId;
            }
            set
            {
                this.testClassRunId = value;
            }
        }

        public ITestRunnerServiceLocator TestRunnerServiceLocator
        {
            get
            {
                return this.testRunnerServiceLocator;
            }
            set
            {
                this.testRunnerServiceLocator = value;
            }
        }

        //
        // Summary:
        //     Executes a test method.
        //
        // Parameters:
        //   testMethod:
        //     The test method to execute.
        //
        // Returns:
        //     An array of TestResult objects that represent the outcome(s) of the test.
        //
        // Remarks:
        //     Extensions can override this method to customize running a TestMethod.
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            TestResult testResult = null;
            testResult = new TestResult
            {
                Outcome = UnitTestOutcome.Failed
            };
            if (this.testClassRunId != Guid.Empty)
            {
                //request server to run test case
                TestId testRequestId = new TestId(testMethod.TestClassName, testMethod.TestMethodName, this.testClassRunId);
                string requestBody = JsonConvert.SerializeObject(testRequestId);
                HttpWebResponse testRunResponse;
                HttpWebResponse testResultResponse;
                string testRunResponseString = string.Empty;
                string testResultResponseString = string.Empty;
                string debuggingLog = string.Empty;
                string testServerPOSTapiURL = this.testRunnerServiceLocator.GetTestRunnerServiceHttpURL(testMethod.TestClassName, testMethod.TestMethodName) + "/api/testresult";
                debuggingLog = debuggingLog + "< " + testServerPOSTapiURL + " >< " + requestBody + " >";
                try
                {
                    testRunResponse = HttpRequestHelper.makePOSTRequest(testServerPOSTapiURL, requestBody);
                    using (var streamReader = new StreamReader(testRunResponse.GetResponseStream()))
                    {
                        testRunResponseString = streamReader.ReadToEnd();
                    }
                    debuggingLog = debuggingLog + "< " + testRunResponseString + " >";
                    TestRunnerResponse testRunnerResponse = JsonConvert.DeserializeObject<TestRunnerResponse>( testRunResponseString );
                    if(testRunnerResponse.HasError == true)
                    {
                        throw new Exception("< " + testRunnerResponse.TestRunnerErrorDescription + " >< " + testRunnerResponse.TestRunnerStackTrace + " >");
                    }
                    //waiting for the time, as suggested by server, before fetching the results
                    Thread.Sleep((int)(testRunnerResponse.TimeToWaitBeforeFetchingResults));

                    //fetch results
                    string testServerGETapiURL = testServerPOSTapiURL + "/" + testRunnerResponse.TestRunInstanceGuid.ToString();
                    debuggingLog = debuggingLog + "< " + testServerGETapiURL + " >";
                    debuggingLog = debuggingLog + "< MAX TRY: " + (this.maxRetriesToFetchTestResult).ToString() + " >";
                    for ( int i = 0; i < this.maxRetriesToFetchTestResult; i++)
                    {
                        debuggingLog = debuggingLog + "< TRY: " + (i + 1).ToString() + " >";
                        testResultResponse = HttpRequestHelper.makeGETRequest( testServerGETapiURL);
                        using (var streamReader = new StreamReader(testResultResponse.GetResponseStream()))
                        {
                            testResultResponseString = streamReader.ReadToEnd();
                        }
                        debuggingLog = debuggingLog + "< " + testResultResponseString + " >";
                        TestResult currTestResult = JsonConvert.DeserializeObject<TestResult>(testResultResponseString);
                        testResult = currTestResult;
                        if(currTestResult.Outcome == UnitTestOutcome.InProgress )
                        {
                            //waiting for the time, as suggested by server, before fetching the results
                            Thread.Sleep((int)(testRunnerResponse.TimeToWaitBeforeFetchingResults));
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    testResult.LogOutput = testResult.LogOutput + debuggingLog;
                }
                catch( Exception ex)
                {
                    testResult.Outcome = UnitTestOutcome.Failed;
                    testResult.LogError = testResult.LogError + debuggingLog;
                    testResult.TestFailureException = ex;
                }
            }
            TestResult[] x = { testResult };
            return x;
        }
    }
}
