using Service_Fabric_Test_Model.ServiceFabricTestClientLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Fabric_Client_Test_Proj_Demo.TestExamples.TestRunnerServiceLocatorExamples
{
    public class DemoTestRunnerServiceLocator : ITestRunnerServiceLocator
    {
        public string GetTestRunnerServiceHttpURL(string namespaceNameDotClassName, string methodName)
        {
            //returning a invalid URL. Not considering arguments for returning URL.
            return " ";
        }
    }
}
