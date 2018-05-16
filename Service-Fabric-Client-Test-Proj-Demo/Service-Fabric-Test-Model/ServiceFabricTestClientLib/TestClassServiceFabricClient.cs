using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service_Fabric_Test_Model.ServiceFabricTestClientLib.Interfaces;

namespace Service_Fabric_Test_Model.ServiceFabricTestClientLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassServiceFabricClient : TestClassAttribute
    {
        //-> 'testClassRunId' represents the running instance id of test class containing test methods. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run on server side.
        private Guid testClassRunId = Guid.NewGuid();

        //-> represents test Runner Service Locator used in this test method.
        private ITestRunnerServiceLocator testRunnerServiceLocator = null;

        //-> represents Namespace name dot Class name which will be used to locate testing service.
        private string testRunnerServiceLocatorNamespaceNameDotClassName = "";

        //'testRunnerServiceLocatorNamespaceNameDotClassName'-> non mandatory: Namespace name dot Class name which will be used to locate testing service. This class must implement interface 'ITestRunnerServiceLocator'. It is not case-sensitive. If it is not passed or passed as null or passed as empty string, instance of 'DefaultTestRunnerServiceLocator' will be used as 'testRunnerServiceLocator'.
        public TestClassServiceFabricClient(string testRunnerServiceLocatorNamespaceNameDotClassName = "")
        {
            this.testRunnerServiceLocatorNamespaceNameDotClassName = testRunnerServiceLocatorNamespaceNameDotClassName;
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
        }

        //
        // Summary:
        //     Gets a test method attribute that enables running this test.
        //
        // Parameters:
        //   testMethodAttribute:
        //     The test method attribute instance defined on this method.
        //
        // Returns:
        //     The Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute to be used
        //     to run this test.
        //
        // Remarks:
        //     Extensions can override this method to customize how all methods in a class are
        //     run.
        public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
        {
            if(testMethodAttribute is TestMethodServiceFabricClientAttribute testMethodAttr)
            {
                if (this.testRunnerServiceLocator == null)
                    this.testRunnerServiceLocator = this.getTestRunnerServiceLocator(this.testRunnerServiceLocatorNamespaceNameDotClassName);
                testMethodAttr.TestClassRunId = this.testClassRunId;
                testMethodAttr.TestRunnerServiceLocator = this.testRunnerServiceLocator;
            }
            return testMethodAttribute;
        }

        //Searches the assembly and return the test service locator instance of class name mentioned in argument.
        //'testRunnerServiceLocatorNamespaceNameDotClassName'-> non mandatory: Namespace name dot Class name which will be used to locate testing service. This class must implement interface 'ITestRunnerServiceLocator'. It is not case-sensitive. If it is not passed or passed as null or passed as empty string, instance of 'DefaultTestRunnerServiceLocator' will be returned.
        public ITestRunnerServiceLocator getTestRunnerServiceLocator(string testRunnerServiceLocatorNamespaceNameDotClassName = "")
        {
            ITestRunnerServiceLocator response = new DefaultTestRunnerServiceLocator();
            if (testRunnerServiceLocatorNamespaceNameDotClassName != null && testRunnerServiceLocatorNamespaceNameDotClassName != string.Empty)
            {

                AppDomain currentDomain = AppDomain.CurrentDomain;
                Assembly[] allAssemblies = currentDomain.GetAssemblies();
                foreach (Assembly assembly in allAssemblies)
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsAbstract && !type.IsInterface && type.GetInterfaces().Contains(typeof(ITestRunnerServiceLocator)) && type.IsClass && string.Equals(type.FullName, testRunnerServiceLocatorNamespaceNameDotClassName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return Activator.CreateInstance(type, null) as ITestRunnerServiceLocator;
                        }
                    }
            }
            return response;
        }
    }
}
