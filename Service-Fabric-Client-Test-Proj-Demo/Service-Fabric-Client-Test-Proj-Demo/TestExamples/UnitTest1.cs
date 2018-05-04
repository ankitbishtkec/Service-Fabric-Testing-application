using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceFabricTestClientLib;


//-> Class name, namespace name and function name here should exactly match( case-sensitive matching) with Class name, namespace name and function name of test files on server
//These 3 are used to identify a test cases here and have a 1 to 1 mapping with a test case on client side.
namespace DemoTestCasesNamespace
{
    //->Functions should have no arguments.
    //->Only functions having 'TestMethodServiceFabricClient' attribute inside class having 'TestClassServiceFabricClient' attribute will run respective test case on server. Any other scheme of assigning attributes will not run test cases on server side. However, 'TestMethodServiceFabricClient', 'TestClass' pair or 'TestMethod', 'TestClassServiceFabricClient' pair will run tests on client side and have behaviour equivalent of 'TestMethod', 'TestClass'.
    
    //[TestClass]
    [TestClassServiceFabricClient]
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
            throw new Exception("User defined exception");
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabricClient(5)]
        public void TestMethodFailGetResultAfterThreeRetries()
        {
            int i = 0;
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabricClient(1)]
        public void DemoTestMethodFailAgain()
        {
            int i = 0;
            Assert.AreEqual(i, 1);
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
