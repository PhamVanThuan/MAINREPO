using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.MortgageRules
{
    /// <summary>
    /// Checks that any of the applicants is of 18 years. Main applicant, others or suretors
    /// Params: 
    /// 0: IApplication
    /// </summary>
    [RuleDBTag("MortgageLoanMinimumAge",
    "Miniumum age must be 18 for all legalentity's",
    "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.MortgageRules.MortgageLoanMinimumAge")]
    [RuleParameterTag(new string[] {"@MinimumAge,18,9"})]
    [RuleInfo]
    public class MortgageLoanMinimumAge : BusinessRuleBase
    {
        //
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into MinimumAge Rule", "", Messages);
                return -1;
            }

            IReadOnlyEventList<ILegalEntity> LEs = application.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant, OfferRoleTypes.Suretor });
            foreach (ILegalEntity LE in LEs)
            {
                ILegalEntityNaturalPerson lenp = LE as ILegalEntityNaturalPerson;
                foreach (IRuleParameter param in RuleItem.RuleParameters)
                {
                    if (param.Name == "@MinimumAge")
                    {
                        if (lenp.AgeNextBirthday <= Convert.ToInt32(param.Value))
                        {
                            AddMessage("Minimum age not met", "All applicants must be of the minimum age.", Messages);
                        }
                    }
                }
            }
            return 1;
        }
    }
}
