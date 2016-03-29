using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.MortgageRules
{
    /// <summary>
    /// Checks the LTV Requirement
    /// Params: 
    /// 0: IApplication
    /// </summary>
    [RuleDBTag("MortgageLoanLTVRequirement",
      "LTV must not be less tan 102%",
        "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.MortgageRules.MortgageLoanLTVRequirement")]
    [RuleParameterTag(new string[] {"@LTV,102,7"})]
    [RuleInfo()]
    public class MortgageLoanLTVRequirement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into MinimumAge Rule", "", Messages);
                return -1;
            }

            IApplicationProductMortgageLoan apml = application.CurrentProduct as IApplicationProductMortgageLoan;
            if (null == apml)
            {
                AddMessage("Application is not a Mortgage Loan", "", Messages);
                return -1;
            }
            ISupportsVariableLoanApplicationInformation vli = apml as ISupportsVariableLoanApplicationInformation;
            if (null != vli)
            {
                foreach (IRuleParameter prm in RuleItem.RuleParameters)
                {
                    if (prm.Name == "@LTV")
                    {
                        double LTV = Convert.ToDouble(prm.Value);
                        if (vli.VariableLoanInformation.LTV < LTV)
                        {
                            AddMessage(string.Format("LTV:({1}) less than than allowed value for application:{0}", application.Key, LTV), "", Messages);
                            return -1;
                        }
                        break;
                    }
                }
            }
            return 1;
        }
    }
}
