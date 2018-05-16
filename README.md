# Service-Fabric-Testing-application

A framework to run Integration tests, Component tests, End-to-end tests and Contract tests on Micro-services residing inside Service Fabric cluster.
[Testing Strategies in a Microservice Architecture](https://martinfowler.com/articles/microservice-testing)

This comprises of two part:
1- [Service-Fabric-Testing-Application](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/tree/master/Service-Fabric-Testing-Application "Service-Fabric-Testing-Application") This folder has a stateful service which runs inside the service fabric cluster. This service has responsibility to run every test cases inside a new thread and store the result of it. Test cases code files are contained in this service.

This folder also contains another stateless service which exposes REST API over HTTP which allows client side test project to run test inside fabric cluster and also fetch the result for the run.

2- [Service-Fabric-Client-Test-Proj-Demo](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/tree/master/Service-Fabric-Client-Test-Proj-Demo "Service-Fabric-Client-Test-Proj-Demo") This is the demo of a client side test project which calls the services mentioned in point 1 to run tests and fetch results.

The services mentioned in point 1 should run inside the same fabric cluster which has the micro-services being tested.

This project uses Reflections in C# and overloading of Microsoft's Unit Testing attributes( annotations) to achieve the results.

## Motivation
 [ServiceFabric.Mocks](https://github.com/loekd/ServiceFabric.Mocks) was already available for running unit tests on Fabric micro-services. It was also of great help to write "narrow" Integration tests.[[How we can perform integration test on service fabric (stateless service)?](https://stackoverflow.com/questions/41782300/how-we-can-perform-integration-test-on-service-fabric-stateless-service)
 
 However, for testing the service endpoints which are only exposed inside cluster( e.g. service remoting endpoints), queues/ services( only available inside cluster) this can be used. As, this allows one to run tests and return results inside fabric cluster.
 
That said, I am firmly located on the side which believes all the Integration tests etc should be written using mocks of other interacting components. This allows cheaper infra( test bed) cost, faster test run time, broader cases( with faster setup) for testing using simulation( service down or high response time etc), testing with a version of interacting component which is not released yet. Only if used mocks are of good quality.
[Integration Tests](https://martinfowler.com/bliki/IntegrationTest.html)


## How to use?
1-  Create a Unit Test Project in .net framework in Visual studio 2017.

2-  Add reference to [Service-Fabric-Test-Model](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/tree/master/Service-Fabric-Client-Test-Proj-Demo/Service-Fabric-Test-Model)

3- Add test files containing test cases. Using attributes imported in point 2. Please refer [UnitTest1.cs](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/blob/master/Service-Fabric-Client-Test-Proj-Demo/Service-Fabric-Client-Test-Proj-Demo/TestExamples/UnitTest1.cs "UnitTest1.cs"). It has lot of comments and full explanation. This file will not contain your actual test case. Please refer above mentioned file to know what it can contain ?

4- Add test files containing test cases to server side i.e. in test runner stateful service. Using overloaded attributes. Please refer [DemoTestCases.cs](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/blob/master/Service-Fabric-Testing-Application/Service_Fabric_Test_Runner/TestExamples/DemoTestCases.cs "DemoTestCases.cs"). It has lot of comments and full explanation. This file will contain your actual test case. Please refer above mentioned file to know what it can contain ? This test class name, namespace name and method name should match a corresponding test class name, namespace name and method name on client side.

5- Run all services including this test runner service group inside fabric cluster.

6- Run tests on client side test project of point 1 using Test-> Run-> All Tests on menu of visual studio or using .exe run. This will run corresponding tests on server side and return results back to client.

A test on server side is recognized uniquely by its Namespace.Class.Method name.

### How it works?
When a test is run on client side, attributes used in point 3 of above section makes 2 REST API call to test runner service:

1- POST call to run test at server side in a new thread. A test on server side is recognized uniquely by its Namespace.Class.Method name. So, you must have a corresponding test case on server side too.
This call return a time period which tells client to wait for given amount to time before trying to fetch test results. This is configurable by test writer.

2- After, waiting for given amount of time. Client makes another call to fetch the test results, if test results are available it exits else it retries for given amount of time before retrying. The number of maximum retries are configurable by test writer. If test results are still unavailable after configured retries the test is considered to be 'failure'.

### Troubleshooting
If test cases are not being discovered in 'Service-Fabric-Client-Test-Proj-Demo'. Go to Visual Studio 2017 menu Test-> Test Settings -> Default Processor Architecture and change it to x64.


#### For complete documentation and features, must read [DemoTestCases.cs](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/blob/master/Service-Fabric-Testing-Application/Service_Fabric_Test_Runner/TestExamples/DemoTestCases.cs "DemoTestCases.cs") for server test cases and [UnitTest1.cs](https://github.com/ankitbishtkec/Service-Fabric-Testing-application/blob/master/Service-Fabric-Client-Test-Proj-Demo/Service-Fabric-Client-Test-Proj-Demo/TestExamples/UnitTest1.cs "UnitTest1.cs") for corresponding client test cases.
