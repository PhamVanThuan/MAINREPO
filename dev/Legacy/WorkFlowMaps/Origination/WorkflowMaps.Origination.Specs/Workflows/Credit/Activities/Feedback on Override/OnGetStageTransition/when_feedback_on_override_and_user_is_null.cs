using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Feedback_on_Override.OnGetStageTransition
{
    [Subject("Activity => Feedback_On_Override => OnGetStageTransition")]
    internal class when_feedback_on_override_and_user_is_null : WorkflowSpecCredit
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
            result = workflow.GetStageTransition_Feedback_on_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_for_the_feedback_on_policy_override_user = () =>
        {
            wfa.WasToldTo(x => x.ReturnFeedbackOnverrideUser((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldEqual(msg);
        };
    }
}