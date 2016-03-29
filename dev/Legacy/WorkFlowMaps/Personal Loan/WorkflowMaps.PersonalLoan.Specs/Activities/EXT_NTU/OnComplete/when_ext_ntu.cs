using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.EXT_NTU.OnComplete
{
    [Subject("Activity => EXT_NTU => OnComplete")]
    internal class when_ext_ntu : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            wfa = An<IWorkflowAssignment>();
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_round_robin_or_reactivate_the_personal_loan_consultant = () =>
        {
            wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
            SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant));
        };

        private It should_add_ntu_under_debtcounselling_reason = () =>
        {
            common.WasToldTo(x => x.AddReasons((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.ReasonDescriptions.PersonalLoansNTUUnderDebtCounselling, (int)SAHL.Common.Globals.ReasonTypes.NTUPersonalLoan));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}