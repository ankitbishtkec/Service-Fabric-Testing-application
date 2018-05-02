using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Service_Fabric_Test_Model
{
    [AttributeUsage( AttributeTargets.Method , AllowMultiple = false)]
    public class TestMethodServiceFabricAttribute : TestMethodAttribute
    {
        private uint expectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest = 60000;//default time is 60 seconds
        private string preTestMethod = "";//default to empty string
        private string postTestMethod = "";//default to empty string

        public TestMethodServiceFabricAttribute(uint expectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest = 60000, string preTestMethod = "", string postTestMethod = "")
        {
            this.expectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest = expectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest;
            this.preTestMethod = preTestMethod;
            this.postTestMethod = postTestMethod;
        }
        public uint ExpectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest
        {
            get
            {
                return this.expectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest;
            }
        }
        public string PreTestMethod
        {
            get
            {
                return this.preTestMethod;
            }
        }
        public string PostTestMethod
        {
            get
            {
                return this.postTestMethod;
            }
        }
    }
}
