using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Disburse_Funds.OnComplete
{
    [Subject("Activity => Disburse_Funds => OnComplete")]
    internal class when_disbursing_funds : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;
        private static IWorkflowAssignment wfa;
        private static string message;
        private static ICommon commonClient;

        private Establish context = () =>
            {
                workflowData.ApplicationKey = 1;
                result = false;
                client = An<IPersonalLoan>();
                wfa = An<IWorkflowAssignment>();
                commonClient = An<ICommon>();
                domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Disburse_Funds(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_create_the_personal_loan_account = () =>
            {
                client.WasToldTo(x => x.CreateAndOpenPersonalLoan((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
            };

        private It should_disburse_the_personal_loan_application = () =>
            {
                client.WasToldTo(x => x.DisburseFunds((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
            };

        private It should_assign_the_personal_loans_supervisor = () =>
            {
                wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
                    SAHL.Common.Globals.WorkflowRoleTypes.PLSupervisorD, workflowData.ApplicationKey,
                    instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLSupervisor));
            };
    }
}