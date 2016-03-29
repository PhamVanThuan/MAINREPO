using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU.OnComplete
{
    [Subject("Activity => NTU => OnComplete")]
    internal class when_NTU : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
        {
            result = false;
            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_round_robin_or_reactivate_the_personal_loan_consultant = () =>
        {
            wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
            SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}