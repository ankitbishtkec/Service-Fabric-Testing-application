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
        private Guid clientTestClassRunId = new Guid();

        //empty argument constructor added to resolve exception during service remoting
        private TestId()
        {

        }
        public TestId( string namespaceNameDotClassName, string methodName, Guid clientTestClassRunId)
        {
            this.namespaceNameDotClassName = namespaceNameDotClassName;
            this.methodName = methodName;
            if( clientTestClassRunId != null)
                this.clientTestClassRunId = clientTestClassRunId;
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

        public Guid ClientTestClassRunId
        {
            get
            {
                return this.clientTestClassRunId;
            }
            set
            {
                this.clientTestClassRunId = value;
            }
        }

        public bool IsValid(ref string error)
        {
            if( this.methodName == null || this.namespaceNameDotClassName == null || this.methodName == string.Empty || this.namespaceNameDotClassName == string.Empty)
            {
                error = "'methodName' and 'namespaceNameDotClassName' should neither be empty string nor null. Please refer example JSON body: {\"namespaceNameDotClassName\": \"DemoTestCasesNamespace.DemoTestCases\", \"methodName\": \"DemoTestMethodFailAgain\", \"clientTestClassRunId\": \"3c3ed451-1b3c-4691-ba1b-5ca86f31e1d9\"}";
                return false;
            }
            return true;
        }
    }
}
