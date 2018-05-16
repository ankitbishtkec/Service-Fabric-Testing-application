using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Fabric_Test_Model.ServiceFabricTestClientLib.Interfaces
{
    //Finds the http url of Test runner service. Client Test project can implement their own test runner service locator and use it.
    //Its implementor must provide a public argument-less constructor.
    public interface ITestRunnerServiceLocator
    {
        //returns the http url of Test runner service. One can use the argument value to return different test runner service's url
        //'namespaceNameDotClassName' -> namespace.class name of test method
        //'methodName' -> method name of test method
        string GetTestRunnerServiceHttpURL( string namespaceNameDotClassName, string methodName);
    }
}
