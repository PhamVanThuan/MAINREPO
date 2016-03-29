using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Request_Policy_Override.OnGetStageTransition
{
    [Subject("Activity => Request_Policy_Override => OnGetStageTransition")]
    internal class when_request_policy_override : WorkflowSpecCredit
    {
        private static string result;
        private static string user;
        private static string msg;
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
        {
            result = string.Empty;
            user = @"sahl\tester";
            msg = string.Format("Policy Override Request sent to {0}", user);

            ((InstanceDataStub)instanceData).ID = 1;

            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            wfa.WhenToldTo(x => x.ReturnPolicyOverrideUser(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>())).Return(user);
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Policy_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_policy_override_user = () =>
        {
            wfa.WasToldTo(x => x.ReturnPolicyOverrideUser((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_string_policy_override_request_sent_to_user = () =>
        {
            result.ShouldEqual(msg);
        };
    }
}