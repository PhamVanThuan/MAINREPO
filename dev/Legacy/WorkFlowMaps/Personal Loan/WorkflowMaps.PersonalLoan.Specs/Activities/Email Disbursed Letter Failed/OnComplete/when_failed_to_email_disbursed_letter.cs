using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Email_Disbursed_Letter_Failed.OnComplete
{
    [Subject("Activity => Email_Disbursed_Letter_Failed => OnComplete")]
    internal class when_failed_to_email_disbursed_letter : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowAssignment.WhenToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLSupervisorD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLSupervisor)).Return(message);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Email_Disbursed_Letter_Failed(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}