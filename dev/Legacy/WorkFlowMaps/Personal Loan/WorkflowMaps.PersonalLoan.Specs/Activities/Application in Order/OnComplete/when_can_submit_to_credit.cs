using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Application_in_Order.OnComplete
{
    [Subject("Activity => Application_in_Order => OnComplete")]
    internal class when_can_submit_to_credit : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;
        private static IWorkflowAssignment wfa;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            client = An<IPersonalLoan>();
            wfa = An<IWorkflowAssignment>();
            client.WhenToldTo(x => x.CheckCreditSubmissionRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);

            client.WhenToldTo(x => x.CheckCreditSubmissionClientRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);

            domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Application_in_Order(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_round_robin_assign_a_pl_credit_analyst = () =>
        {
            wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
                SAHL.Common.Globals.WorkflowRoleTypes.PLCreditAnalystD, workflowData.ApplicationKey, instanceData.ID,
                SAHL.Common.Globals.RoundRobinPointers.PLCreditAnalyst));
        };
    }
}