using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service_Fabric_Test_Model;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using System.Reflection;

namespace Service_Fabric_Test_Runner
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service_Fabric_Test_Runner : StatefulService, IServiceFabricTestRunner
    {
        public Service_Fabric_Test_Runner(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<TestResult> GetTestResult(Guid guid)
        {
            TestResult tr = new TestResult();
            tr.Outcome = UnitTestOutcome.Inconclusive;
            return tr;
        }

        public async Task<TestRunnerResponse> RunTestCase(TestId testId)
        {
            //Type classType = null;
            //MethodInfo testMethod = null;
            //MethodInfo preTestMethod = null;
            //MethodInfo postTestMethod = null;
            //Dictionary<string, MethodInfo> methodsInClass = new Dictionary<string, MethodInfo>();
            //Assembly mscorlib = Assembly.GetExecutingAssembly();
            //foreach (Type type in mscorlib.GetTypes())//find all classes in assembly
            //{
            //    foreach (Object attributes in type.GetCustomAttributes(false))//find all attributes on class
            //    {
            //        TestClassAttribute testClassAttribute = attributes as TestClassAttribute;

            //        if (testClassAttribute != null && type.FullName == testId.NamespaceNameDotClassName)//if class has 'TestClass' attribute and matches the class name of required test case
            //        {
            //            classType = type;
            //            foreach (MethodInfo m in type.GetMethods())
            //                methodsInClass.Add(m.Name, m);
            //            if( methodsInClass.ContainsKey( testId.MethodName) )
            //            {
            //                testMethod = methodsInClass[testId.MethodName];

            //            }
            //            //write your code here
            //            break;
            //        }
            //    }
            //}
            TestRunnerResponse tr = new TestRunnerResponse(3000);
            return tr;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]{
                new ServiceReplicaListener(
                    (context) => new FabricTransportServiceRemotingListener(context,this))};
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
