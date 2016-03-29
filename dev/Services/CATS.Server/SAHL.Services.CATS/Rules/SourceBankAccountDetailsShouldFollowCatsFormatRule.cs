using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Models;
using System.Linq;

namespace SAHL.Services.CATS.Rules
{

    public class SourceBankAccountDetailsShouldFollowCatsFormatRule : IDomainRule<PaymentBatch>
    {
        public void ExecuteRule(ISystemMessageCollection messages, PaymentBatch ruleModel)
        {
            if (ruleModel.SourceAccount.AccountNumber.Trim().Length > 13)
            {
                messages.AddMessage(new SystemMessage("Invalid account number: " + ruleModel.SourceAccount.AccountNumber.Trim()
                    + ". The source account's account number should not be longer than 13 characters.", SystemMessageSeverityEnum.Error));
            }
            if (ruleModel.SourceAccount.ACBBranchCode.Trim().Length > 6)
            {
                messages.AddMessage(new SystemMessage("Invalid branch code: " + ruleModel.SourceAccount.ACBBranchCode.Trim()
                    + ". The source account's branch code should not be longer than 6 characters.", SystemMessageSeverityEnum.Error));
            }
            if (ruleModel.SourceAccount.AccountName.Trim().Length > 30)
            {
                messages.AddMessage(new SystemMessage("Invalid account name: " + ruleModel.SourceAccount.AccountName.Trim()
                    + ". The source account's account name should not be longer than 30 characters.", SystemMessageSeverityEnum.Error));
            }
            if (ruleModel.SourceAccount.ACBTypeNumber > 9 || ruleModel.SourceAccount.ACBTypeNumber < 0)
            {
                messages.AddMessage(new SystemMessage("Invalid acb type number: " + ruleModel.SourceAccount.ACBTypeNumber
                    + ". The source account's acb type number should be in the range of [0 - 9].", SystemMessageSeverityEnum.Error));
            }
        }
    }
}

