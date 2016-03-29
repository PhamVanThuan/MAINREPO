using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Feedback_on_Override.OnGetStageTransition
{
    [Subject("Activity => Feedback_On_Override => OnGetStageTransition")]
    internal class when_feedback_on_override : WorkflowSpecCredit
    {
        private static string result;
        private static string user;
        private static string msg;
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
        {
            result = string.Empty;
            user = @"sahl\tester";
            msg = string.Format("Feedback on Policy Override made by {0}", user);
            ((InstanceDataStub)instanceData).ID = 1;

            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            wfa.WhenToldTo(x => x.ReturnFeedbackOnverrideUser(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>())).Return(user);
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Feedback_on_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_returnthe_user_for_the_feedback_on_policy_override = () =>
        {
            wfa.WasToldTo(x => x.ReturnFeedbackOnverrideUser((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_string_feedback_on_policy_override_made_by_user = () =>
        {
            result.ShouldEqual(msg);
        };
    }
}