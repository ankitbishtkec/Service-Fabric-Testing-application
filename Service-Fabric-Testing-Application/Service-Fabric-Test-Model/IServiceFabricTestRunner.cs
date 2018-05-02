using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Service_Fabric_Test_Model
{
    public interface IServiceFabricTestRunner : IService
    {
        Task<TestRunnerResponse> RunTestCase(TestId testId);

        Task<TestResult> GetTestResult(Guid guid);
    }
}
