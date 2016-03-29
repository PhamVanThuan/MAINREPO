using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Rules;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Specs.Rules.DoesBankAccountPassCDVValidation
{
    public class when_the_bank_account_passes_cdv_validation : WithFakes
    {
        private static BankAccountMustAdhereToCDVValidationRule rule;
        private static BankAccountModel bankAccountModel;
        private static ISystemMessageCollection messages;
        private static ICdvValidationManager validationManager;
        private Establish context = () =>
        {
            validationManager = An<ICdvValidationManager>();
            messages = SystemMessageCollection.Empty();
            rule = new BankAccountMustAdhereToCDVValidationRule(validationManager);
            bankAccountModel = new BankAccountModel("250655", "FNB", "62379700414", Core.BusinessModel.Enums.ACBType.Current, "CT Speed", "System");
            validationManager.WhenToldTo(x => x.ValidateAccountNumber(bankAccountModel.BranchCode, (int)bankAccountModel.AccountType, bankAccountModel.AccountNumber)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, bankAccountModel);
        };

        private It should_not_return_a_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}