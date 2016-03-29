using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Application_Withdrawn.OnComplete
{
    [Subject("Activity => Court_Application_Withdrawn => OnComplete")]
    internal class when_court_application_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static List<string> statesToExclude = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };
        private static IWorkflowAssignment workflowAssignment;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.DebtCounsellingKey = 1;
            workflowData.CourtCase = true;

            client = An<IDebtCounselling>();
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            client.WhenToldTo(x => x.UpdateHearingDetailStatusToInactive(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Court_Application_Withdrawn(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_assign_or_load_balance_the_case_to_the_debt_counselling_consultant = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Court Application Withdrawn", statesToExclude, false, workflowData.CourtCase));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}