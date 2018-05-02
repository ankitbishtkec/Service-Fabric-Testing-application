using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Service_Fabric_Test_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Service_Fabric_Testing_Web_API.Controllers
{
    [Route("api/[controller]")]
    public class TestResultController : Controller
    {
        private readonly IServiceFabricTestRunner _serviceFabricTestRunner;

        public TestResultController()
        {
            _serviceFabricTestRunner = ServiceProxy.Create<IServiceFabricTestRunner>(new Uri("fabric:/Service_Fabric_Testing_Application/Service_Fabric_Test_Runner"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));
        }

        // GET api/testresult/00000000-0000-0000-0000-000000000000
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(Guid id)
        {
            JsonResult response;
            //response.StatusCode = 500;
            TestResult tr = await _serviceFabricTestRunner.GetTestResult(id);
            response = new JsonResult(tr);
            return response;
        }

        // POST api/testresult
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]TestId testId)
        {
            string error = "";
            JsonResult response;
            if (testId == null || !testId.IsValid(ref error))
            {
                response = new JsonResult(error);
                response.StatusCode = 400;
                return response;
            }
            response = new JsonResult(new TestRunnerResponse(50000));
            return response;
        }
    }
}
