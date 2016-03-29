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
    /// Checks that investment property applications meet the LTV requirements
    /// Params: 
    /// 0: IApplication
    /// </summary>
    [RuleInfo]
    public class InvestmentPropertyLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication application = Parameters[0] as IApplication;
            if (null == application)
            {
                AddMessage("Wrong type passed into MinimumAge Rule", "", Messages);
                return -1;
            }

            // go find whether this is an investment product. If so make sure the LTV is less than 100%
            foreach (IRuleParameter param in RuleItem.RuleParameters)
            {
                if (param.Name == "@LTV")
                {
                    AddMessage("Rule Not Complete", "", Messages);
                }
            }
            return 1;
        }
    }
}
