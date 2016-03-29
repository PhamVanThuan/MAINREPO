using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors
{
    [Behaviors]
    public class DomainProcessThatHasAddedBankAccounts 
    {
        protected static IBankAccountDomainServiceClient bankAccountDomainService;
        protected static IApplicationStateMachine applicationStateMachine;
        protected static IApplicationDomainServiceClient applicationDomainService;
        protected static int applicationNumber, clientKey, clientBankAccountKey;
        protected static BankAccountModel bankAccount;
        protected static ApplicantModel applicant;
        protected static ApplicationDebitOrderModel applicationDebitOrder;

        private It should_add_the_client_bank_account = () =>
        {
            bankAccountDomainService.WasToldTo(x => x.PerformCommand(Param<LinkBankAccountToClientCommand>.Matches(m =>
                m.ClientKey == clientKey &&
                m.BankAccountModel.AccountName == bankAccount.AccountName &&
                m.BankAccountModel.AccountNumber == bankAccount.AccountNumber &&
                m.BankAccountModel.AccountType == bankAccount.AccountType &&
                m.BankAccountModel.BranchCode == bankAccount.BranchCode &&
                m.BankAccountModel.BranchName == bankAccount.BranchName
            ), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
        private It should_add_the_bank_account_to_the_debit_order_collection = () =>
        {
            bankAccount.IsDebitOrderBankAccount.ShouldBeTrue();
            applicationStateMachine.ClientDebitOrderBankAccountCollection[applicant.IDNumber].ShouldEqual(clientBankAccountKey);
        };

        private It should_add_the_application_debit_order = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<AddApplicationDebitOrderCommand>.Matches(m =>
                m.ApplicationNumber == applicationNumber &&
                m.ApplicationDebitOrderModel.ApplicationNumber == applicationNumber &&
                m.ApplicationDebitOrderModel.ClientBankAccountKey == clientBankAccountKey &&
                m.ApplicationDebitOrderModel.DebitOrderDay == applicationDebitOrder.DebitOrderDay &&
                m.ApplicationDebitOrderModel.PaymentType == applicationDebitOrder.PaymentType
            ), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}
