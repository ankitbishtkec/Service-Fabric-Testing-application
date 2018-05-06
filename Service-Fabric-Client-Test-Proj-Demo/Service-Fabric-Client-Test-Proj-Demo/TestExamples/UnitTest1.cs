using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service_Fabric_Test_Model.ServiceFabricTestClientLib;


//-> Class name, namespace name and function name here should exactly match( case-sensitive matching) with Class name, namespace name and function name of test files on server
//These 3 are used to identify a test cases here and have a 1 to 1 mapping with a test case on client side.
namespace DemoTestCasesNamespace
{
    //->Functions should have no arguments.
    //->Only functions having 'TestMethodServiceFabricClient' attribute inside class having 'TestClassServiceFabricClient' attribute will run respective test case on server. Any other scheme of assigning attributes will not run test cases on server side. However, 'TestMethodServiceFabricClient', 'TestClass' pair or 'TestMethod', 'TestClassServiceFabricClient' pair will run tests on client side and have behaviour equivalent of 'TestMethod', 'TestClass'.
    
    //[TestClass]
    [TestClassServiceFabricClient("http://localhost:9031")]
    //Non-Mandatory field: "testServerURL" represents server where test code will run
    public class DemoTestCases
    {
        [TestMethodServiceFabricClient(3)]
        //Non-Mandatory field: "maxRetriesToFetchTestResult" this represents the maximum number of retries Service Fabric test client should make to fetch 'TestResult' of a test case. We will only make next retry if earlier retry has failed. Default value is 2.
        public void TestMethodPass()
        {
            //Every test method will be invoked before submitting test run request for respective server test case. If this code raises any exception including assertion failure exception, test run on server will not be done. And, the reported 'TestResult' will contain the exception raised during execution of this code and 'TestResult' would be reported as 'Failed' or any other status except "passed'.
            //write your test code here
            //you are responsible for :
            //-> Writing a test case that does not runs indefinitely i.e. test case should end.
            //-> One can write client side preTestCase code here. Like deploying a service etc.
            int i = 0;
            i++;
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabricClient(2)]
        public void TestMethodFail()
        {
            int i = 0;
            Thread.Sleep(1000);
            //due to assertion failure or throwing exception these tests are failing on client side itself. So, server side tests are not being run and client side run results are being considered. As, client side example test cases and server side example test cases have similar code. Commenting lines below in this function will not fail tests here, and allow tests to run on server side. In that case server side test case result will be considered. Please comment lines below to demo that scenario.
            throw new Exception("User defined exception");//comment me as mentioned above to demo different scenario.
            Assert.AreEqual(i, 1);//comment me as mentioned above to demo different scenario.
        }

        [TestMethodServiceFabricClient(5)]
        public void TestMethodFailGetResultAfterThreeRetries()
        {
            int i = 0;
            //due to assertion failure or throwing exception these tests are failing on client side itself. So, server side tests are not being run and client side run results are being considered. As, client side example test cases and server side example test cases have similar code. Commenting lines below in this function will not fail tests here, and allow tests to run on server side. In that case server side test case result will be considered. Please comment lines below to demo that scenario.
            Assert.AreEqual(i, 1);//comment me as mentioned above to demo different scenario.
        }

        [TestMethodServiceFabricClient(1)]
        public void DemoTestMethodFailAgain()
        {
            int i = 0;
            //due to assertion failure or throwing exception these tests are failing on client side itself. So, server side tests are not being run and client side run results are being considered. As, client side example test cases and server side example test cases have similar code. Commenting lines below in this function will not fail tests here, and allow tests to run on server side. In that case server side test case result will be considered. Please comment lines below to demo that scenario.
            Assert.AreEqual(i, 1);//comment me as mentioned above to demo different scenario.
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
    }
}
