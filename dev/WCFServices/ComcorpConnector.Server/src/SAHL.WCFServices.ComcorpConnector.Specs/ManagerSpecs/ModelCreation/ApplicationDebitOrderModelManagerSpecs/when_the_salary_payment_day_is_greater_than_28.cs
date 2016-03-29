using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicationDebitOrderModelManagerSpecs
{
    public class when_the_salary_payment_day_is_greater_than_28 : WithCoreFakes
    {
        private static ApplicationDebitOrderModelManager applicationModelManager;
        private static List<ApplicantModel> applicants;
        private static List<BankAccountModel> bankAccounts;
        private static ApplicationDebitOrderModel applicationDebitOrder;
        private static ApplicantModel applicant;

        private Establish context = () =>
        {
            bankAccounts = new List<BankAccountModel>()
                {
                    new BankAccountModel("632005", "ABSA Electronic", "88888888", ACBType.Current, "Test", "System", true)
                };
            applicants = new List<ApplicantModel>();
            applicationModelManager = new ApplicationDebitOrderModelManager();
            applicant = IntegrationServiceTestHelper.PopulateApplicant(30, bankAccounts);
            applicants.Add(applicant);
        };

        private Because of = () =>
        {
            applicationDebitOrder = applicationModelManager.PopulateApplicationDebitOrder(applicants);
        };

        private It should_set_the_debit_order_day_to_the_first = () =>
        {
            applicationDebitOrder.DebitOrderDay.ShouldEqual(1);
        };
    }
}