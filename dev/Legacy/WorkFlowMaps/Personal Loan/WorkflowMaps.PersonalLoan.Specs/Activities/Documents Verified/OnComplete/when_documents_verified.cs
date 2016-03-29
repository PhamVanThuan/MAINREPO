using Machine.Fakes;
using Machine.Specifications;
using Personal_Loan;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Documents_Verified.OnComplete
{
    [Subject("Activity => Documents_Verified => OnComplete")]
    internal class when_documents_verified : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static IPersonalLoan personalLoan;

        private Establish context = () =>
        {
            result = false;
            wfa = An<IWorkflowAssignment>();
            personalLoan = An<IPersonalLoan>();
            workflowData = new X2Personal_Loans_Data();
            workflowData.ApplicationKey = Param.IsAny<int>();

            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(personalLoan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Documents_Verified(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_activate_pending_domicilium_address = () =>
        {
            personalLoan.WasToldTo(x => x.ActivatePendingDomiciliumAddress(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_round_robin_assign_the_personal_loans_supervisor = () =>
        {
            wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
                SAHL.Common.Globals.WorkflowRoleTypes.PLSupervisorD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLSupervisor));
        };
    }
}