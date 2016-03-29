using SAHL.Config.Services.X2.Client;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Tests.X2.Managers;
using System;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.LoadTests
{
    public class ApplicationCaptureTestCase : ITestCase
    {
        private IX2TestDataManager dataManager;

        public ApplicationCaptureTestCase(IX2TestDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Test(string hostName, int workerId)
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
            ServiceRequestMetadata metadata = null;
            foreach (var appCapCase in this.dataManager.GetApplicationCaptureTestCases(hostName, workerId))
            {
                try
                {
                    Console.WriteLine("Start Create => Activity: {0} Process: {1} Workflow: {2} HostName: {3} WorkerId: {4}", appCapCase.ActivityName, appCapCase.ProcessName, appCapCase.WorkFlowName, hostName, workerId);

                    SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest request = new SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), appCapCase.ActivityName, appCapCase.ProcessName, appCapCase.WorkFlowName, metadata, appCapCase.IgnoreWarnings, null, null, appCapCase.MapVariables, null);
                    var x2Response = client.PerformCommand(request);

                    X2Response response = request.Result;

                    if (response.InstanceId > 0)
                    {
                        if (response.IsErrorResponse)
                        {
                            X2ErrorResponse errorResponse = request.Result as X2ErrorResponse;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Test failed => Error Response: {0}", errorResponse.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Create Complete => Activity: {0} Process: {1} Workflow: {2}", appCapCase.ActivityName, appCapCase.ProcessName, appCapCase.WorkFlowName);
                            Console.ForegroundColor = ConsoleColor.White;
                            if (appCapCase.SleepSeconds > 0)
                            {
                                System.Threading.Thread.Sleep(appCapCase.SleepSeconds);
                            }

                            // main instance at correct state
                            // AssertExpectedEndState(response.InstanceId);

                            // check clone is created
                            // long relatedInstanceID = GetRelatedInstanceIDFromParentInstance(response.InstanceId);
                            // AssertExpectedEndState(relatedInstanceID, "Awaiting Timeout");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Test failed : {0}", ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private void AssertExpectedEndState(long instanceId, string expectedState = "Application Capture")
        {
            var x2Case = this.dataManager.GetX2Case(instanceId);
            if (x2Case == null)
            {
                throw new Exception(string.Format("No X2 instance found for instance '{0}'", instanceId));
            }

            if (x2Case.Process == null)
            {
                throw new Exception(string.Format("No process for instance '{0}'", instanceId));
            }

            if (x2Case.State == null)
            {
                throw new Exception(string.Format("No state for instance '{0}'", instanceId));
            }

            if (!x2Case.State.Contains(expectedState))
            {
                throw new Exception(string.Format("Scenariomap : 'Process:{0} Workflow:{1}' failed, workflow case did not end up at the expected state. Instance:{2} is at workflow state:{3}", x2Case.Process, x2Case.Workflow, x2Case.InstanceId, x2Case.State));
            }
        }
    }
}