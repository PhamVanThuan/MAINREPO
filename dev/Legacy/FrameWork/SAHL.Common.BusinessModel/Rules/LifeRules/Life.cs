using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.LifeRules
{
    [RuleDBTag("ValidateAssuredLifeMinimumRequired",
    "At least one assured life role must exist",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LifeRules.ValidateAssuredLifeMinimumRequired")]
    [RuleInfo]
    public class ValidateAssuredLifeMinimumRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateLegalEntityMinimumRequired rule expects a Domain object to be passed.");

            IAccountLifePolicy accountLifePolicy = Parameters[0] as IAccountLifePolicy;
            if (accountLifePolicy == null)
                throw new ArgumentException("The ValidateLegalEntityMinimumRequired rule expects the following objects to be passed: IAccountLifePolicy.");

            // if we are closing the account then no validation required
            if (accountLifePolicy.AccountStatus.Key == (int)Globals.AccountStatuses.Closed)
                return 1;

            int assuredLifeRoles = 0;
            foreach (IRole role in accountLifePolicy.Roles)
            {
                if (role.RoleType.Key == (int)SAHL.Common.Globals.RoleTypes.AssuredLife)
                    assuredLifeRoles++;              
            }

            if (assuredLifeRoles <= 0)
            {
                AddMessage("At least one Assured Life must exist.", "At least one Assured Life must exist.", Messages);
                return 0;
            }

            return 1;

        }
    }

    [RuleDBTag("LifeApplicationCreateDebtCounselling",
    "A Life Application cannot be created if its parent loan is under going Debt Counseling",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LifeRules.LifeApplicationCreateDebtCounselling")]
    [RuleInfo]
    public class LifeApplicationCreateDebtCounselling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The LifeApplicationCreateDebtCounselling rule expects a Domain object to be passed.");

            IApplicationLife applicationLife = Parameters[0] as IApplicationLife;

            IAccount account = null;
            string msg = string.Empty;

            //AccountLifePolicy
            if (Parameters[0] is IAccountLifePolicy)
            {
                IAccountLifePolicy accountLifePolicy = Parameters[0] as IAccountLifePolicy;
				if (accountLifePolicy.LifePolicy != null &&
					 accountLifePolicy.LifePolicy.LifePolicyStatus.Key != (int)LifePolicyStatuses.Prospect)
				{
					return 0;
				}
                account = accountLifePolicy.ParentAccount;  //RelatedParentAccounts[0];
            }
            //Account
            else if (Parameters[0] is IAccount)
            {
                account = Parameters[0] as IAccount;
            }
            //Life Application
            else if (Parameters[0] is IApplicationLife)
            {
                IApplicationLife lifeApplication = Parameters[0] as IApplicationLife;
                IAccountLifePolicy accountLifePolicy = lifeApplication.Account as IAccountLifePolicy;
                account = accountLifePolicy.ParentAccount; // RelatedParentAccounts[0];
            }

            if (account == null)
                return 0;

            //Check if the Account is under Debt Counselling
            if (account.UnderDebtCounselling)
            {
                msg = "This Account is undergoing Debt Counselling.";
                AddMessage(msg, msg, Messages);
            }

            // Check if any Legal Entities against the Account is under debt counselling
            foreach (IRole role in account.Roles)
            {
                if (role.LegalEntity.DebtCounsellingCases != null)
                {
                    foreach (IDebtCounselling dc in role.LegalEntity.DebtCounsellingCases)
                    {
                        msg = string.Format("{0} ({1}) on account ({2}) is under debt counselling.", role.LegalEntity.DisplayName, role.RoleType.Description, account.Key);
                        AddMessage(msg, msg, Messages);
                    }
                }
            }

            if (!string.IsNullOrEmpty(msg))
                return 1;

            return 0;
        }
    }

    [RuleDBTag("LifeApplicationCheckMonthsInArrears",
    "Check whether the loan account is currently 3 or more months in arrears.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LifeRules.LifeApplicationCheckMonthsInArrears")]
    [RuleParameterTag(new string[] { "@MaxMonthsInArrears,-2,9" })]
    [RuleInfo]
    public class LifeApplicationCheckMonthsInArrears : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The LifeApplicationCheckMonthsInArrears rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IMortgageLoanAccount))
                throw new ArgumentException("The LifeApplicationCheckMonthsInArrears rule expects the following objects to be passed: IMortgageLoanAccount.");

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            int maxMonthsInArrears = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            // Get the number of months in arrears
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            double monthsInArrears = accRepo.GetCurrentMonthsInArrears(mortgageLoanAccount.Key);

            if (monthsInArrears >= maxMonthsInArrears)
            {
                string errorMessage = string.Format("A life lead cannot be created since the loan is currently {0} or more months in arrears.", maxMonthsInArrears);
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }
}
