using Service_Fabric_Test_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;

//-> Class name, namespace name and function name here should exactly match with Class name, namespace name and function name of test solution
//These 3 are used to identify a test cases here and have a 1 to 1 mapping with a test case on client side.
//-> 'TestResult' object will not have your output or error logs emitted. Use your own logs library. 'clientTestClassRunId' + namespace name + class name + method name can be unique id for a run during logging. Or override Console.WriteLine https://stackoverflow.com/questions/32661000/how-can-i-use-an-own-class-for-console-writeline-override https://blogs.msdn.microsoft.com/gautamg/2009/12/08/logging-a-message-in-test-result-as-part-of-an-automated-test/
namespace DemoTestCasesNamespace
{
    //a public argument less constructor must be added, if another constructor is present.
    [TestClass]
    public class DemoTestCases
    {
        public void preTestMethodPass(Guid clientTestClassRunId = new Guid())
        {
            //write your pre-test code( environment reset etc) here
            //you are responsible for :
            //-> Writing a test case that does not runs indefinitely.
            //-> Throwing exception, if needed, with relevent information to debug your test case. This information will be available at client test case's TestResult object.
            //-> Reliable store can be used to mark enviroment as fresh or dirty at preTestMethod or postTestMethod. So that multiple environment reset etc code in preTestMethod or environment cleanup etc in postTestMenthod can be skipped based on its value. This logic can be written by Test developer based on need.
            //-> This method should have one argument a Guid and should be in same class as Test method.
            //-> 'clientTestClassRunId' represents the running instance id of test class containing test methods on client side. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run and help to execute( using reliable store or otherwise) certain operations only once per class per run.
            //-> Use your own logs library. 'clientTestClassRunId' + namespace name + class name + method name can be unique id for a run during logging.. Or override Console.WriteLine https://stackoverflow.com/questions/32661000/how-can-i-use-an-own-class-for-console-writeline-override https://blogs.msdn.microsoft.com/gautamg/2009/12/08/logging-a-message-in-test-result-as-part-of-an-automated-test/

            Thread.Sleep(2000);
        }

        [TestMethodServiceFabric(10000, "preTestMethodPass", "postTestMethodPass")]
        //Non-Mandatory field: "ExpectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest" must contain expected time this pretest case, test case and post test case all combined will take to complete execution. By default it is 60 seconds.
        //Non-Mandatory field: "preTestMethod" must contain name of method to run before running actual test case. By default it is empty string. Case-sensitive.
        //Non-Mandatory field: "postTestMethod" must contain name of method to run after running actual test case. By default it is empty string. Case-sensitive.
        public void TestMethodPass( Guid clientTestClassRunId = new Guid())
        {
            //write your test code here
            //you are responsible for :
            //-> Writing a test case that does not runs indefinitely i.e. test case should end. Either as success or failure.
            //-> A test case which does not affects other test cases result during its run and after its run is over. If it does, please consider using an exclusive environment, preTestMethod and postTestMethod.
            //-> Before and after test case run. A clean up should be done so that subsequent test cases are not affected.  If it does, please consider using an exclusive environment, preTestMethod and postTestMethod.
            //-> If certain output or event is not available after 'n' retries. This information will be available at client test case's TestResult object.
            //-> This method should have one argument a Guid.
            //-> 'clientTestClassRunId' represents the running instance id of test class containing test methods on client side. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run and help to execute( using reliable store or otherwise) certain operations only once per class per run.
            //-> Use your own logs library. 'clientTestClassRunId' + namespace name + class name + method name can be unique id for a run during logging.. Or override Console.WriteLine https://stackoverflow.com/questions/32661000/how-can-i-use-an-own-class-for-console-writeline-override https://blogs.msdn.microsoft.com/gautamg/2009/12/08/logging-a-message-in-test-result-as-part-of-an-automated-test/

            int i = 0;
            i++;
            Assert.AreEqual(i, 1);
        }

        public void postTestMethodPass( Guid clientTestClassRunId = new Guid())
        {
            //write your post-test code( environment cleanup etc) here
            //you are responsible for :
            //-> Writing a test case that does not runs indefinitely.
            //-> Throwing exception, if needed, with relevent data to debug your test case. This data will be available at client test case's TestResult object.
            //-> Reliable store can be used to mark enviroment as fresh or dirty at preTestMethod or postTestMethod. So that multiple environment reset etc code in preTestMethod or environment cleanup etc in postTestMenthod can be skipped based on its value. This logic can be written by Test developer based on need.
            //-> This method should have one argument a Guid and should be in same class as Test method.
            //-> 'clientTestClassRunId' represents the running instance id of test class containing test methods on client side. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run and help to execute( using reliable store or otherwise) certain operations only once per class per run.
            //-> Use your own logs library. 'clientTestClassRunId' + namespace name + class name + method name can be unique id for a run during logging.. Or override Console.WriteLine https://stackoverflow.com/questions/32661000/how-can-i-use-an-own-class-for-console-writeline-override https://blogs.msdn.microsoft.com/gautamg/2009/12/08/logging-a-message-in-test-result-as-part-of-an-automated-test/

            Thread.Sleep(1000);
        }

        [TestMethodServiceFabric(5000, "", "")]
        public void TestMethodFail( Guid clientTestClassRunId = new Guid())
        {
            int i = 0;
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabric(5000)]
        public void TestMethodFailGetResultAfterThreeRetries(Guid clientTestClassRunId = new Guid())
        {
            int i = 0;
            Thread.Sleep(15000);
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabric(2000)]
        public void DemoTestMethodFailAgain( Guid clientTestClassRunId = new Guid())
        {
            int i = 0;
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabric()]
        public void TestMethodPass1(Guid clientTestClassRunId = new Guid())
        {
            int i = 0;
            i++;
            Thread.Sleep(3333);
            Assert.AreEqual(i, 1);
        }

        [TestMethodServiceFabric(5000)]
        public void TestMethodPassLongRunning(Guid clientTestClassRunId = new Guid())
        {
            int i = 0;
            i++;
            Thread.Sleep(20000);
            Assert.AreEqual(i, 1);
        }
    }
}
