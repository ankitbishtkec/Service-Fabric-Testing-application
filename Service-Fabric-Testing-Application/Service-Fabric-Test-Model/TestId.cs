using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service_Fabric_Test_Model
{
    public class TestId
    {
        private string namespaceNameDotClassName = string.Empty;
        private string methodName = string.Empty;

        public TestId( string namespaceNameDotClassName, string methodName)
        {
            this.namespaceNameDotClassName = namespaceNameDotClassName;
            this.methodName = methodName;
        }

        public string NamespaceNameDotClassName
        {
            get
            {
                return this.namespaceNameDotClassName;
            }
            set
            {
                this.namespaceNameDotClassName = value;
            }
        }

        public string MethodName
        {
            get
            {
                return this.methodName;
            }
            set
            {
                this.methodName = value;
            }
        }

        public bool IsValid(ref string error)
        {
            if( this.methodName == null || this.namespaceNameDotClassName == null || this.methodName == string.Empty || this.namespaceNameDotClassName == string.Empty)
            {
                error = "'methodName' and 'namespaceNameDotClassName' should neither be empty string nor null. Please refer example JSON body: {\"namespaceNameDotClassName\": \"DemoTestCasesNamespace.DemoTestCases\", \"methodName\": \"DemoTestMethodFailAgain\"}";
                return false;
            }
            return true;
        }
    }
}
