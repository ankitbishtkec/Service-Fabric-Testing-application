using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Fabric_Test_Model
{
    public class TestRunnerResponse
    {
        private Guid testRunInstanceGuid;
        private int timeToWaitBeforeFetchingResults;

        public TestRunnerResponse(int timeToWaitBeforeFetchingResults)
        {
            this.testRunInstanceGuid = Guid.NewGuid();
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
    }
}
