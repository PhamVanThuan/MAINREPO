using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.Services.Interfaces.X2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.X2Workflow
{
    public class when_successfully_creating_workflow_task : WithFakes
    {
        private static X2WorkflowManager x2workflowManger;
        private static ICombGuid combGuidGenerator;
        private static IX2Service x2Service;
        private static Guid domainProcessId;
        private static Guid requestInstanceCorrelationId;
        private static int applicationNumber;
        private static IApplicationStateMachine applicationStateMachine;
        private static ISystemMessageCollection systemMessages;
        private static long instanceId;
        private static string createInstanceRequestActivityName;
        private static string createInstanceRequestUserName;
        private static bool createInstanceRequestIgnoreWarnings;
        private static IDictionary<string, string> createInstanceRequestMapVariables;
        private static object createInstanceRequestData;
        private static DomainProcessServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            combGuidGenerator = An<ICombGuid>();
            x2Service = An<IX2Service>();
            applicationStateMachine = An<IApplicationStateMachine>();
            systemMessages = SystemMessageCollection.Empty();

            requestInstanceCorrelationId = combGuidGenerator.Generate();
            domainProcessId = combGuidGenerator.Generate();
            applicationNumber = 13244;
            instanceId = 547569854;

            serviceRequestMetadata = new DomainProcessServiceRequestMetadata(domainProcessId, requestInstanceCorrelationId);

            x2workflowManger = new X2WorkflowManager(x2Service);

            X2Response cannedErrorResponse = new X2Response(requestInstanceCorrelationId, string.Empty, instanceId);
            x2Service.WhenToldTo(x => x.PerformCommand(Param.IsAny<X2CreateInstanceRequest>()))
                .Return<X2CreateInstanceRequest>(rqst =>
                {
                    rqst.Result = cannedErrorResponse;
                    createInstanceRequestActivityName = rqst.ActivityName;
                    createInstanceRequestUserName = rqst.ServiceRequestMetadata.UserName;
                    createInstanceRequestIgnoreWarnings = rqst.IgnoreWarnings;
                    createInstanceRequestMapVariables = rqst.MapVariables;
                    createInstanceRequestData = rqst.Data;

                    return systemMessages;
                });
        };

        private Because of = () =>
        {
            systemMessages = x2workflowManger.CreateWorkflowCase(applicationNumber, serviceRequestMetadata);
        };

        private It should_start_a_create_workflow_activity = () =>
        {
            x2Service.WasToldTo(x => x.PerformCommand(Param<X2CreateInstanceRequest>.Matches(m =>
                m.ActivityName == X2Constants.WorkFlowActivities.ApplicationCreate &&
                    m.CorrelationId == requestInstanceCorrelationId &&
                    m.ProcessName == X2Constants.WorkflowProcesses.Origination &&
                    m.WorkflowName == X2Constants.WorkFlowNames.ApplicationCapture &&
                    MatchMetadata(m.ServiceRequestMetadata as ServiceRequestMetadata) &&
                    !m.IgnoreWarnings &&
                    MatchVariables(m.MapVariables)
                )));
        };

        private static bool MatchMetadata(IServiceRequestMetadata serviceRequestMeatadata)
        {
            return serviceRequestMeatadata.ContainsKey(CoreGlobals.DomainProcessIdName) &&
              serviceRequestMeatadata[CoreGlobals.DomainProcessIdName] == domainProcessId.ToString();
        }

        private static bool MatchVariables(Dictionary<string, string> mapVariables)
        {
            return mapVariables.ContainsKey("ApplicationKey") &&
                    mapVariables["ApplicationKey"] == applicationNumber.ToString() &&
                    mapVariables.ContainsKey("isEstateAgentApplication") &&
                    mapVariables["isEstateAgentApplication"] == "False";

        }

        private It should_complete_the_activity = () =>
        {
            x2Service.WasToldTo(x => x.PerformCommand(
                Param<X2RequestForExistingInstance>.Matches(m =>
                    m.CorrelationId == requestInstanceCorrelationId &&
                        m.InstanceId == instanceId &&
                        m.ActivityName == createInstanceRequestActivityName &&
                        m.ServiceRequestMetadata.UserName == createInstanceRequestUserName &&
                        m.IgnoreWarnings == createInstanceRequestIgnoreWarnings &&
                        m.MapVariables == createInstanceRequestMapVariables &&
                        m.Data == createInstanceRequestData
                    )
                ));
        };

        private It should_not_add_any_system_messages = () =>
        {
            systemMessages.AllMessages.Count().ShouldEqual(0);
        };
    }
}
