using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicationDebitOrderModelManagerSpecs
{
    public class when_no_bank_accounts_exist : WithCoreFakes
    {
        private static ApplicationDebitOrderModelManager applicationModelManager;
        private static List<ApplicantModel> applicants;
        private static List<BankAccountModel> bankAccounts;
        private static ApplicationDebitOrderModel applicationDebitOrder;
        private static ApplicantModel applicant;

        private Establish context = () =>
        {
            bankAccounts = new List<BankAccountModel>();
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