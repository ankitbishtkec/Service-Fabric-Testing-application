using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceFabricTestClientLib
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestMethodServiceFabricClientAttribute : TestMethodAttribute
    {
        //this represents the maximum number of retries Service Fabric test client should make to fetch 'TestResult' of a test case. We will only make next retry if earlier retry has failed.
        private uint maxRetriesToFetchTestResult = 2;//default is 2 retries
        //-> 'testClassRunId' represents the running instance id of test class containing test methods. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run on server side.
        private Guid testClassRunId = new Guid();

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
            testResult = testMethod.Invoke(null);
            if (testResult.Outcome == UnitTestOutcome.Passed && this.testClassRunId != Guid.Empty)
            {
                //write code here to start test case at server side and fetch the result from server.
                ;
            }
            TestResult[] x = { testResult };
            return x;
        }
    }
}
