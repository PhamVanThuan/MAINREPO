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
    public class when_populating_the_application_debit_order : WithCoreFakes
    {
        private static ApplicationDebitOrderModelManager applicationModelManager;
        private static List<ApplicantModel> applicants;
        private static BankAccountModel nonDebitOrderAccount;
        private static BankAccountModel debitOrderAccount;
        private static List<BankAccountModel> bankAccounts;
        private static ApplicationDebitOrderModel applicationDebitOrder;
        private static ApplicantModel applicant;

        private Establish context = () =>
        {
            bankAccounts = new List<BankAccountModel>();
            applicants = new List<ApplicantModel>();
            applicationModelManager = new ApplicationDebitOrderModelManager();
            nonDebitOrderAccount = new BankAccountModel("632005", "ABSA Electronic", "77777777", ACBType.Current, "Test", "System", false);
            debitOrderAccount = new BankAccountModel("632005", "ABSA Electronic", "88888888", ACBType.Current, "Test", "System", true);
            bankAccounts.Add(nonDebitOrderAccount);
            bankAccounts.Add(debitOrderAccount);
            applicant = IntegrationServiceTestHelper.PopulateApplicant(25, bankAccounts);
            applicants.Add(applicant);
        };

        private Because of = () =>
        {
            applicationDebitOrder = applicationModelManager.PopulateApplicationDebitOrder(applicants);
        };

        private It should_set_the_application_debit_order_bank_account_to_the_debit_order_account = () =>
        {
            applicationDebitOrder.BankAccount.ShouldEqual(debitOrderAccount);
        };

        private It should_set_the_financial_service_payment_type_to_debit_order = () =>
        {
            applicationDebitOrder.PaymentType.ShouldEqual(FinancialServicePaymentType.DebitOrderPayment);
        };

        private It should_set_the_debit_order_day_to_the_salary_payment_day_of_the_first_active_employment_record = () =>
        {
            applicationDebitOrder.DebitOrderDay.ShouldEqual(
                applicant.Employments.Where(x => x.EmploymentStatus == EmploymentStatus.Current).First().SalaryPaymentDay);
        };
    }
}