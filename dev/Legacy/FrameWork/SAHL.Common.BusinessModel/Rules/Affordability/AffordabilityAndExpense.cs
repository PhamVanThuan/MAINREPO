using System;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Affordability
{
    [RuleDBTag("AffordabilityDescriptionMandatory",
    "Description must be completed",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory")]
    [RuleInfo]
    public class AffordabilityDescriptionMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAffordability leAffordability = Parameters[0] as ILegalEntityAffordability;
            if (leAffordability == null)
                throw new ArgumentException("Parameter[0] is not of type ILegalEntityAffordability.");

            // if the affordability type hasn't been set then just exit here - we can't check the rule and the
            // mandatory value will fail
            if (leAffordability.AffordabilityType == null)
                return 1;

            if ((leAffordability.AffordabilityType.DescriptionRequired) && String.IsNullOrWhiteSpace(leAffordability.Description))
            {
                AddMessage("Enter description for " + leAffordability.AffordabilityType.Description + ".", "Enter description for " + leAffordability.AffordabilityType.Description + ".", Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("AffordabilityNegativeValue",
   "Amount can not be negative",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue")]
    [RuleInfo]
    public class AffordabilityNegativeValue : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAffordability leAffordability = Parameters[0] as ILegalEntityAffordability;

            if (leAffordability == null)
                throw new ArgumentException("Parameter[0] is not of type ILegalEntityAffordability.");

            if (leAffordability.Amount < 0)
                AddMessage("Negative amounts are not allowed.", "Negative amounts are not allowed", Messages);

            return 0;
        }
    }

    [RuleDBTag("AffordabilityAtLeastOneIncome",
    "At least one income is required",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome")]
    [RuleInfo]
    public class AffordabilityAtLeastOneIncome : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEventList<ILegalEntityAffordability> le = Parameters[0] as IEventList<ILegalEntityAffordability>;
            if (le == null)
                throw new ArgumentException("Parameter[0] is not of type IEventList<ILegalEntityAffordability>");

            IApplication app = Parameters[1] as IApplication;
            if (app == null)
                throw new ArgumentException("Parameter[1] is not of type IApplication");

            if (le.Count == 0)
                return 1;

            if (le.Any(x => x.Application.Key == app.Key && (x.AffordabilityType != null && !x.AffordabilityType.IsExpense)))
            {
                return 1;
            }

            // if we get this far, no income affordability types were found and an error exists
            AddMessage("At least one income amount is required.", "At least one income amount is required.", Messages);
            return 1;
        }
    }

    [RuleDBTag("AffordabilityStatementMandatory",
  "At least one income and expenditure statement is required",
  "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory")]
    [RuleInfo]
    public class AffordabilityStatementMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("Parameter[0] is not of type IApplicationMortgageLoan.");

            IApplicationMortgageLoan applicationMortgageLoan = (IApplicationMortgageLoan)Parameters[0];

            var affordabilities = applicationMortgageLoan.ApplicationRoles
                                            .Where(x => x.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant ||
                                                        x.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor ||
                                                        x.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant ||
                                                        x.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.Suretor)
                                            .Where(x => x.LegalEntity.LegalEntityAffordabilities == null || x.LegalEntity.LegalEntityAffordabilities.Count == 0).ToArray();

            for (int i = 0; i < affordabilities.Count(); i++)
            {
                string description = affordabilities[i].LegalEntity.DisplayName + " has no affordability and expenses.";
                AddMessage(description, description, Messages);
            }

            return 0;
        }
    }
}