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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.X2Workflow
{
    public class when_creating_workflow_task_response_in_error : WithFakes
    {
        private static X2WorkflowManager x2workflowManger;
        private static ICombGuid combGuidGenerator;
        private static IX2Service x2Service;
        private static Guid domainProcessId;
        private static Guid requestInstanceCorrelationId;
        private static int applicationNumber;
        private static IApplicationStateMachine applicationStateMachine;
        private static ISystemMessageCollection systemMessages;
        private static DomainProcessServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
         {
             combGuidGenerator = An<ICombGuid>();
             x2Service = An<IX2Service>();
             applicationStateMachine = An<IApplicationStateMachine>();
             systemMessages = An<ISystemMessageCollection>();

             requestInstanceCorrelationId = combGuidGenerator.Generate();
             domainProcessId = combGuidGenerator.Generate();
             applicationNumber = 13244;
             x2workflowManger = new X2WorkflowManager(x2Service);

             serviceRequestMetadata = new DomainProcessServiceRequestMetadata(domainProcessId, requestInstanceCorrelationId);

             X2Response cannedErrorResponse = new X2Response(requestInstanceCorrelationId, string.Empty, 54756, true);
             x2Service
                .WhenToldTo(x => x.PerformCommand(Param.IsAny<X2CreateInstanceRequest>()))
                .Return<X2CreateInstanceRequest>(rqst => { rqst.Result = cannedErrorResponse; return systemMessages; });
         };

        private Because of = () =>
        {
            x2workflowManger.CreateWorkflowCase(applicationNumber, serviceRequestMetadata);
        };

        private It should_issue_a_create_workflow_case_request = () =>
        {
            x2Service.WasToldTo(x => x.PerformCommand(Param<X2CreateInstanceRequest>.Matches(m =>
                   m.CorrelationId == requestInstanceCorrelationId &&
                   m.ActivityName == X2Constants.WorkFlowActivities.ApplicationCreate &&
                   m.ProcessName == X2Constants.WorkflowProcesses.Origination &&
                   m.WorkflowName == X2Constants.WorkFlowNames.ApplicationCapture &&
                   !m.IgnoreWarnings &&
                  MatchMapVariables(m.MapVariables) &&
                    MatchMetadata(m.ServiceRequestMetadata as ServiceRequestMetadata)
                )));
        };

        private static bool MatchMetadata(IServiceRequestMetadata serviceRequestMeatadata)
        {
            return serviceRequestMeatadata.ContainsKey(CoreGlobals.DomainProcessIdName) &&
              serviceRequestMeatadata[CoreGlobals.DomainProcessIdName] == domainProcessId.ToString();
        }

        private static bool MatchMapVariables(Dictionary<string, string> mapVariables)
        {
            return mapVariables.ContainsKey("ApplicationKey") &&
                   mapVariables["ApplicationKey"] == applicationNumber.ToString() &&
                   mapVariables.ContainsKey("isEstateAgentApplication") &&
                   mapVariables["isEstateAgentApplication"] == "False";
        }

        private It should_not_issue_a_complete_request = () =>
        {
            x2Service.WasNotToldTo(x => x.PerformCommand(Param.IsAny<X2RequestForExistingInstance>()));
        };
    }
}