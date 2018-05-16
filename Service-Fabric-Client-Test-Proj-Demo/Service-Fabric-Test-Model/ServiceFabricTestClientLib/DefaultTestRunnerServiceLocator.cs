using Service_Fabric_Test_Model.ServiceFabricTestClientLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Fabric_Test_Model.ServiceFabricTestClientLib
{
    //default implementation of ITestRunnerServiceLocator. It uses a hard code url for demo.
    public class DefaultTestRunnerServiceLocator : ITestRunnerServiceLocator
    {
        public string GetTestRunnerServiceHttpURL(string namespaceNameDotClassName, string methodName)
        {
            //arguments has not been considered before returning url.
            return "http://localhost:9031";
        }
    }
}
