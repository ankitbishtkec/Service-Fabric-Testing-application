using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceFabricTestClientLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassServiceFabricClient : TestClassAttribute
    {
        //-> 'testClassRunId' represents the running instance id of test class containing test methods. It will be same for all Test methods run inside a Test class. It can be used to associate different test methods run on server side.
        private Guid testClassRunId = Guid.NewGuid();

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
                testMethodAttr.TestClassRunId = this.testClassRunId;
            }
            return testMethodAttribute;
        }
    }
}
