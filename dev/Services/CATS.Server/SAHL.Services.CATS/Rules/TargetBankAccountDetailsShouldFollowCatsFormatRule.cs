using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Models;
using System.Linq;

namespace SAHL.Services.CATS.Rules
{

    public class TargetBankAccountDetailsShouldFollowCatsFormatRule : IDomainRule<PaymentBatch>
    {

        public void ExecuteRule(ISystemMessageCollection messages, PaymentBatch ruleModel)
        {
            foreach (Payment payment in ruleModel.Payments)
            {
                if (payment.TargetAccount.AccountNumber.Trim().Length > 13)
                {
                    messages.AddMessage(new SystemMessage("Invalid account number: " + payment.TargetAccount.AccountNumber.Trim()
                        + ". The target account's account number should not be longer than 13 characters.", SystemMessageSeverityEnum.Error));
                }
                if (payment.TargetAccount.ACBBranchCode.Trim().Length > 6)
                {
                    messages.AddMessage(new SystemMessage("Invalid branch code: " + payment.TargetAccount.ACBBranchCode.Trim()
                        + ". The target account's branch code should not be longer than 6 characters.", SystemMessageSeverityEnum.Error));
                }
                if (payment.TargetAccount.AccountName.Trim().Length > 30)
                {
                    messages.AddMessage(new SystemMessage("Invalid account name: " + payment.TargetAccount.AccountName.Trim()
                        + ". The target account's account name should not be longer than 30 characters.", SystemMessageSeverityEnum.Error));
                }
                if (payment.TargetAccount.ACBTypeNumber > 9 || ruleModel.SourceAccount.ACBTypeNumber < 0)
                {
                    messages.AddMessage(new SystemMessage("Invalid acb type number: " + payment.TargetAccount.ACBTypeNumber
                        + ". The Target bank account's acb type number should be in the range of [0 - 9].", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}

