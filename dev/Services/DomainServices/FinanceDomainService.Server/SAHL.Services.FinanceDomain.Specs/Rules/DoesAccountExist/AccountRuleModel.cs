using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.DoesAccountExist
{
    public class AccountRuleModel : IAccountRuleModel
    {
        public int AccountNumber { get; protected set; }

        public AccountRuleModel(int accountNumber)
        {
            this.AccountNumber = accountNumber;
        }
    }
}