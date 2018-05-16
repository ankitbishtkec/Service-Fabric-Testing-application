using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service_Fabric_Test_Model.ServiceFabricTestClientLib;


//-> Class name, namespace name and function name here should exactly match( case-sensitive matching) with Class name, namespace name and function name of test files on server
//These 3 are used to identify a test cases here and have a 1 to 1 mapping with a test case on client side.
namespace DemoTestCasesNamespace
{
    //->Functions should have no arguments.
    //->Only functions having 'TestMethodServiceFabricClient' attribute inside class having 'TestClassServiceFabricClient' attribute will run respective test case on server. Any other scheme of assigning attributes will not run test cases on server side.

    //'testRunnerServiceLocatorNamespaceNameDotClassName'-> non mandatory: Namespace name dot Class name which will be used to locate testing service. This class must implement interface 'ITestRunnerServiceLocator'. It is not case-sensitive. If it is not passed or passed as null or passed as empty string, instance of 'DefaultTestRunnerServiceLocator' will be used.
    [TestClassServiceFabricClient("Service_Fabric_Test_Model.ServiceFabricTestClientLib.DefaultTestRunnerServiceLocator")]
    //uncomment above attribute, and comment other attributes below to use default test runner service locator 'DefaultTestRunnerServiceLocator'

    //[TestClassServiceFabricClient("Service_Fabric_Client_Test_Proj_Demo.TestExamples.TestRunnerServiceLocatorExamples.DemoTestRunnerServiceLocatoR")]
    //uncomment above attribute, and comment other attributes below to use custom test runner service locator 'DemoTestRunnerServiceLocator'
    //Argument provided above is not case sensitive. Check the 'R' at the end of argument string.


    //[TestClassServiceFabricClient]
    //uncomment above attribute, and comment other attributes below to use default test runner service locator 'DefaultTestRunnerServiceLocator'
    public class DemoTestCases
    {

        public DemoTestCases()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {; }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup() {; }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currRunningTestMethod = this.TestContext.TestName;
            //use 'currRunningTestMethod' to run test method specific initialization code before a test method run.
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string currRunningTestMethod = this.TestContext.TestName;
            //use 'currRunningTestMethod' to run test method specific cleanup code after a test method run.

        }

        #region test_cases

        [TestMethodServiceFabricClient(3)]
        //Non-Mandatory field: "maxRetriesToFetchTestResult" this represents the maximum number of retries Service Fabric test client should make to fetch 'TestResult' of a test case. We will only make next retry if earlier retry has failed. Default value is 2.
        public void TestMethodPass()
        {
            //->Only functions having 'TestMethodServiceFabricClient' attribute inside class having 'TestClassServiceFabricClient' attribute will run respective test case on server. Any other scheme of assigning attributes will not run test cases on server side.
            //Functions having 'TestMethodServiceFabricClient' attribute will not run. Rather, their corresponding method(  Class name, namespace name and function name combination) on server side will run. So, they can be empty. However, 'TestInitialize' and 'TestCleanup' will run before and after it. So, flow is: execute 'TestInitialize' -> execute corresponding server side test code -> execute 'TestCleanup'.
            ;
        }

        [TestMethodServiceFabricClient(2)]
        public void TestMethodFail()
        {
            ;
        }

        [TestMethodServiceFabricClient(5)]
        public void TestMethodFailGetResultAfterThreeRetries()
        {
            ;
        }

        [TestMethodServiceFabricClient(1)]
        public void DemoTestMethodFailAgain()
        {
            ;
        }

        [TestMethodServiceFabricClient()]
        public void TestMethodPass1()
        {
            ;
        }

        [TestMethodServiceFabricClient(3)]
        public void TestMethodPassLongRunning()
        {
            ;
        }

        #endregion
    }
}
