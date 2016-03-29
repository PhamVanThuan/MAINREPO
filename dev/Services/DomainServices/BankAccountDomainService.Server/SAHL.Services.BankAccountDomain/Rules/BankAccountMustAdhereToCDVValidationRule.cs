using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Rules
{
    public class BankAccountMustAdhereToCDVValidationRule : IDomainRule<BankAccountModel>
    {
        private ICdvValidationManager validationManager;
        public BankAccountMustAdhereToCDVValidationRule(ICdvValidationManager validationManager)
        {
            this.validationManager = validationManager;
        }
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, BankAccountModel ruleModel)
        {
            if (!validationManager.ValidateAccountNumber(ruleModel.BranchCode, (int)ruleModel.AccountType, ruleModel.AccountNumber))
            {
                messages.AddMessage(new SystemMessage("Bank Account failed CDV validation.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}