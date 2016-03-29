using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicationDebitOrderModelManagerSpecs
{
    public class when_no_debit_order_bank_account_exists : WithCoreFakes
    {
        private static ApplicationDebitOrderModelManager applicationModelManager;
        private static List<ApplicantModel> applicants;
        private static List<BankAccountModel> bankAccounts;
        private static ApplicationDebitOrderModel applicationDebitOrder;
        private static ApplicantModel applicant;

        private Establish context = () =>
        {
            bankAccounts = new List<BankAccountModel>(){
                new BankAccountModel("632005", "ABSA Electronic", "77777777", ACBType.Current, "Test", "System", false)
                };
            applicants = new List<ApplicantModel>();
            applicationModelManager = new ApplicationDebitOrderModelManager();
            applicant = IntegrationServiceTestHelper.PopulateApplicant(25, bankAccounts);
            applicants.Add(applicant);
        };

        private Because of = () =>
        {
            applicationDebitOrder = applicationModelManager.PopulateApplicationDebitOrder(applicants);
        };

        private It should_return_null = () =>
        {
            applicationDebitOrder.ShouldBeNull();
        };
    }
}