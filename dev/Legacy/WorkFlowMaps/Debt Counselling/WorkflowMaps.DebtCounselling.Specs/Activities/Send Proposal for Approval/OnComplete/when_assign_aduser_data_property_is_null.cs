using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Proposal_for_Approval.OnComplete
{
    [Subject("Activity => Send_Proposal_for_Approval => OnComplete")]
    internal class when_assign_aduser_data_property_is_null : WorkflowSpecDebtCounselling
    {
        private static string message;
        private static bool result;
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
            {
                wfa = An<IWorkflowAssignment>();
                result = true;
                workflowData.AssignADUserName = null;
                workflowData.AssignWorkflowRoleTypeKey = 1;
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Send_Proposal_for_Approval(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };

        private It should_not_assign_the_workflow_role = () =>
            {
                wfa.WasNotToldTo(x => x.AssignWorkflowRoleForADUser(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<int>(), Param.IsAny<string>()));
            };
    }
}