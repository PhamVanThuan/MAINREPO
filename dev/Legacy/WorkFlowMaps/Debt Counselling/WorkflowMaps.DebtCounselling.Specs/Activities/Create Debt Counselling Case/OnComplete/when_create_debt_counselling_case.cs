using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Create_Debt_Counselling_Case.OnComplete
{
    [Subject("Activity => Create_Debt_Counselling_Case => OnComplete")]
    internal class when_create_debt_counselling_case : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static IWorkflowAssignment wfa;
        private static string message;
        private static bool result;
        private static string instanceSubject;
        private static int accountKey;

        private Establish context = () =>
            {
                result = false;
                instanceSubject = "test";
                accountKey = 1234567;
                workflowData.AccountKey = accountKey;
                client = An<IDebtCounselling>();
                wfa = An<IWorkflowAssignment>();
                client.WhenToldTo(x => x.GetInstanceSubjectForDebtCounselling(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(instanceSubject);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Create_Debt_Counselling_Case(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_set_the_subject_instance_data_property = () =>
            {
                instanceData.Subject.ShouldEqual(instanceSubject);
            };

        private It should_set_the_name_instance_data_property = () =>
            {
                instanceData.Name.ShouldEqual(accountKey.ToString());
            };

        private It should_assign_the_debt_counselling_administrator = () =>
            {
                wfa.WasToldTo(x => x.AssignWorkflowRoleForADUser((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName, SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingAdminD,
                    workflowData.DebtCounsellingKey, ""));
            };

        private It should_check_for_open_personal_loan_applications_for_the_legal_entities_under_debt_counselling = () =>
            {
                client.WasToldTo(x => x.NTUOpenPersonalLoan(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
            };
    }
}