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
using Microsoft.ServiceFabric.Data;

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
            ConditionalValue<TestResult> testResult;
            try
            {
                var testResults = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, TestResult>>("TestResults");
                using (var tx = this.StateManager.CreateTransaction())
                {
                    testResult = await testResults.TryGetValueAsync(tx, guid);
                }
            }
            catch( Exception ex)
            {
                return null;
            }
            return (testResult.HasValue) ? testResult.Value : null;
        }

        public async Task<TestRunnerResponse> RunTestCase(TestId testId)
        {
            Type classType = null;
            MethodInfo testMethod = null;
            MethodInfo preTestMethod = null;
            MethodInfo postTestMethod = null;
            TestRunnerResponse response = null;
            //not using cancellation token. As, this testing service will be short lived and will be killed and respawned multiple times during countinous delivery
            //var tokenSource = new CancellationTokenSource();
            //var cancellationtoken = tokenSource.Token;
            Dictionary<string, MethodInfo> methodsInClass = new Dictionary<string, MethodInfo>();
            try
            {
                Assembly mscorlib = Assembly.GetExecutingAssembly();
                foreach (Type type in mscorlib.GetTypes())//find all classes in assembly
                {
                    //find class first
                    if (type.FullName == testId.NamespaceNameDotClassName)//if class matches the class name of required test case
                    {
                        //commented below code as "TestClass" tag is redundant here
                        /*TestClassAttribute testClassAttribute;
                        foreach (Object attributes in type.GetCustomAttributes(false))//find all attributes on class
                        {
                            testClassAttribute = attributes as TestClassAttribute;

                            if (testClassAttribute != null)//if class has 'TestClass' attribute and matches the class name of required test case
                            {
                                classType = type;
                                break;
                            }
                        }*/
                        classType = type;
                    }
                    //find method now
                    if (classType != null)
                    {
                        foreach (MethodInfo m in classType.GetMethods())
                        {
                            methodsInClass.Add(m.Name, m);
                        }
                        if (methodsInClass.ContainsKey(testId.MethodName))
                        {
                            foreach (Attribute attr in methodsInClass[testId.MethodName].GetCustomAttributes(true))
                            {
                                if (attr is TestMethodServiceFabricAttribute testMethodAttr)
                                {
                                    testMethod = methodsInClass[testId.MethodName];
                                    response = new TestRunnerResponse(testMethodAttr.ExpectedExecutionTimeOfTestInMillisecondsIncludingPreTestAndPostTest);
                                    if (methodsInClass.ContainsKey(testMethodAttr.PreTestMethod))
                                    {
                                        preTestMethod = methodsInClass[testMethodAttr.PreTestMethod];
                                    }
                                    if (methodsInClass.ContainsKey(testMethodAttr.PostTestMethod))
                                    {
                                        postTestMethod = methodsInClass[testMethodAttr.PostTestMethod];
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
                if (classType != null && testMethod != null)
                {
                    //run test case here in a thread
                    var testCaseTask = Task.Run(() => RunTestCaseInternal( testId, response.TestRunInstanceGuid, classType, testMethod, preTestMethod, postTestMethod));
                }
            }
            catch( Exception ex)
            {
                response = new TestRunnerResponse(0);
                response.TestRunnerErrorDescription = ex.Message;
                response.TestRunnerStackTrace = ex.StackTrace;
            }
            return response;
        }

        void RunTestCaseInternal(TestId testId, Guid TestRunInstanceGuid, Type classType, MethodInfo testMethod, MethodInfo preTestMethod = null, MethodInfo postTestMethod = null)
        {
            object classInstance;
            TestResult testResult = null;
            try
            {
                if (classType != null && testMethod != null)
                {
                    testResult = new TestResult();
                    testResult.Outcome = UnitTestOutcome.InProgress;
                    testResult.LogOutput = TestRunInstanceGuid.ToString();
                    testResult.LogOutput = testResult.LogOutput + " # " + classType.Namespace;
                    testResult.LogOutput = testResult.LogOutput + " # " + classType.Name;
                    testResult.LogOutput = testResult.LogOutput + " # " + testMethod.Name;
                    testResult.LogOutput = testResult.LogOutput + " \n TestRunInstanceGuid # Namespace Name # Test Class Name # Test Method Name";
                    object[] parametersArray = new object[] { testId.ClientTestClassRunId };
                    classInstance = Activator.CreateInstance(classType, null);

                    if (preTestMethod != null)
                    {
                        testResult.LogOutput = testResult.LogOutput + " \n PreTestMethod has started execution.";
                        SaveOrUpdateTestResult(TestRunInstanceGuid, testResult);
                        preTestMethod.Invoke(classInstance, parametersArray);
                    }
                    else
                    {
                        testResult.LogOutput = testResult.LogOutput + " \n PreTestMethod does not exists.";
                    }

                    testResult.LogOutput = testResult.LogOutput + " \n TestMethod has started execution.";
                    SaveOrUpdateTestResult(TestRunInstanceGuid, testResult);
                    DateTime startTime = DateTime.UtcNow;
                    testMethod.Invoke(classInstance, parametersArray);
                    testResult.Duration = DateTime.UtcNow - startTime;

                    if (postTestMethod != null)
                    {
                        testResult.LogOutput = testResult.LogOutput + " \n PostTestMethod has started execution.";
                        SaveOrUpdateTestResult(TestRunInstanceGuid, testResult);
                        postTestMethod.Invoke(classInstance, parametersArray);
                    }
                    else
                    {
                        testResult.LogOutput = testResult.LogOutput + " \n PostTestMethod does not exists.";
                    }
                    testResult.Outcome = UnitTestOutcome.Passed;
                    testResult.LogOutput = testResult.LogOutput + " \n Test case has completed and passed.";
                }
            }
            catch( Exception ex)
            {
                Exception exception;
                if (ex.InnerException != null)
                {
                    exception = new Exception("MESSAGE:< " + ex.InnerException.Message + " > STACK TRACE:" + ex.InnerException.StackTrace + " > INNER MESSAGE:< " + ex.Message + " > INNER STACK TRACE:" + ex.StackTrace + " >");
                }
                else
                {
                    exception = new Exception("MESSAGE:< " + ex.Message + " > STACK TRACE:" + ex.StackTrace + " >");
                }
                testResult.TestFailureException = exception;
                testResult.Outcome = UnitTestOutcome.Failed;
                testResult.LogOutput = testResult.LogOutput + " \n Test case might have completed and failed. Please check 'LogOutput' to know which stages of test cases has executed.";
            }
            SaveOrUpdateTestResult(TestRunInstanceGuid, testResult);
        }

        //save or updates the test result to reliable dictionary
        async void  SaveOrUpdateTestResult(Guid testId, TestResult tr)
        {
            var testResults = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, TestResult>>("TestResults");
            using (var tx = this.StateManager.CreateTransaction())
            {
                await testResults.AddOrUpdateAsync(tx, testId, tr, (id, value) => tr);

                await tx.CommitAsync();
            }
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
