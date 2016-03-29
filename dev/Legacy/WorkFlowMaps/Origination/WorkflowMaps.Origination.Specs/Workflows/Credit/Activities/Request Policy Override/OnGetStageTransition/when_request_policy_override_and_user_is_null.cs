using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Request_Policy_Override.OnGetStageTransition
{
    [Subject("Activity => Request_Policy_Override => OnGetStageTransition")]
    internal class when_request_policy_override_and_user_is_null : WorkflowSpecCredit
    {
        private static string result;
        private static string msg;
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
        {
            result = "abcd";
            msg = string.Empty;
            ((InstanceDataStub)instanceData).ID = 1;

            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            wfa.WhenToldTo(x => x.ReturnPolicyOverrideUser(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>())).Return(string.Empty);
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Policy_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_for_the_policy_override_user = () =>
        {
            wfa.WasToldTo(x => x.ReturnPolicyOverrideUser((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldEqual(msg);
        };
    }
}