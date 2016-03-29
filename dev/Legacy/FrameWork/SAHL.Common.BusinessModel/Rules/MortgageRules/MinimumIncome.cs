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
    /// Checks that the combined applicants income is enough
    /// Params: 
    /// 0: IApplication
    /// </summary>
    [RuleDBTag("MortgageLoanMinimumIncome",
    "Miniumum income must be 6000 per application",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.MortgageRules.MortgageLoanMinimumIncome")]
    [RuleParameterTag(new string[] {"@MinimumIncome,6000,7"})]
    [RuleInfo]
    public class MortgageLoanMinimumIncome : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into MinimumAge Rule", "", Messages);
                return -1;
            }
            double Income = application.GetHouseHoldIncome();
            //foreach (IRuleParameter param in RuleItem.RuleParameters)
            IEnumerator<IRuleParameter> enumerator = RuleItem.RuleParameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IRuleParameter param = enumerator.Current;
                if (param.Name == "@MinimumIncome")
                {
                    if (Income < Convert.ToDouble(param.Value))
                    {
                        AddMessage("Insufficient Household income", "", Messages);
                        return -1;
                    }
                    break;
                }
            }
            enumerator.Dispose();
            return 1;
        }
    }
}
