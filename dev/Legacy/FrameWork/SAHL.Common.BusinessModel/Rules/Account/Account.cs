using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.Account
{
    [RuleDBTag("AccountIsAlphaHousing",
    "Determines whether an account is alpha housing.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing")]
    public class AccountIsAlphaHousing : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var account = parameters[0] as IAccount;

            var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            if (account.Details == null)
            {
                return 1;
            }

            var alphaHousingDetailTypes = account.Details.Where(x => x.DetailType.DetailClass.Key == (int)DetailClasses.AlphaHousing);

            if (alphaHousingDetailTypes.Count() > 0)
            {
                string errorMessage = "Account is an Alpha Housing Loan";
                AddMessage(errorMessage, errorMessage, messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ActiveSubsidyAndSalaryStopOrderConditionExistsError",
    "Check if an active subsidy record exists for the account and then check for the existence of either the 222 or 223 condition linked to the accounts accepted offers.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsError")]
    [RuleInfo]
    public class ActiveSubsidyAndSalaryStopOrderConditionExistsError : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0)
                throw new ArgumentException("The ActiveSubsidyAndSalaryStopOrderConditionExists rule expects a Domain object to be passed.");

            if (!(parameters[0] is IAccount))
                throw new ArgumentException("The ActiveSubsidyAndSalaryStopOrderConditionExists rule expects the following objects to be passed: IAccount.");

            IAccount account = parameters[0] as IAccount;

            if (account.HasActiveSubsidy)
            {
                IEnumerable<IApplication> acceptedApplications = account.Applications.Where(x => x.ApplicationStatus.Key == (int)OfferStatuses.Accepted);

                foreach (IApplication application in acceptedApplications)
                {
                    if (application.HasCondition("222") || application.HasCondition("223"))
                    {
                        AddMessage("It is a condition of the loan that the instalment shall be paid by way of a salary stop order.", "", messages);
                        return 0;
                    }
                }
            }

            return 1;
        }
    }

    [RuleDBTag("ActiveSubsidyAndSalaryStopOrderConditionExistsWarning",
    "Check if an active subsidy record exists for the account and then check for the existence of either the 222 or 223 condition linked to the accounts accepted offers.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsWarning")]
    [RuleInfo]
    public class ActiveSubsidyAndSalaryStopOrderConditionExistsWarning : ActiveSubsidyAndSalaryStopOrderConditionExistsError
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            return base.ExecuteRule(Messages, Parameters);
        }
    }
}