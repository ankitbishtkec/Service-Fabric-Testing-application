using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Fabric_Test_Model
{
    public class TestRunnerResponse
    {
        private Guid testRunInstanceGuid = Guid.NewGuid();
        private int timeToWaitBeforeFetchingResults = 60000;//default time is 60 seconds
        private bool hasError = false;
        private string testRunnerErrorDescription = string.Empty;
        private string testRunnerStackTrace = string.Empty;


        //empty argument constructor added to resolve exception during service remoting
        private TestRunnerResponse()
        {
        }

        public TestRunnerResponse(int timeToWaitBeforeFetchingResults)
        {
            this.timeToWaitBeforeFetchingResults = timeToWaitBeforeFetchingResults;
        }

        public int TimeToWaitBeforeFetchingResults
        {
            get
            {
                return this.timeToWaitBeforeFetchingResults;
            }
            set
            {
                this.timeToWaitBeforeFetchingResults = value;
            }
        }

        public Guid TestRunInstanceGuid
        {
            get
            {
                return this.testRunInstanceGuid;
            }
        }

        public bool HasError
        {
            get
            {
                return this.hasError;
            }
        }

        public string TestRunnerErrorDescription
        {
            get
            {
                return this.testRunnerErrorDescription;
            }
            set
            {
                if (value != string.Empty)
                    this.hasError = true;
                else
                    this.hasError = false;
                this.testRunnerErrorDescription = value;
            }
        }

        public string TestRunnerStackTrace
        {
            get
            {
                return this.testRunnerStackTrace;
            }
            set
            {
                this.testRunnerStackTrace = value;
            }
        }
    }
}
