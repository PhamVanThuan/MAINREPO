using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("ValuationAutomatedAdCheckValuation",
   "AdCheck Valuation can not be updated",
    "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Valuations.ValuationAutomatedAdCheckValuation")]
    [RuleInfo]
    public class ValuationAutomatedAdCheckValuation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];
            
            if ((val is IValuationDiscriminatedAdCheckDesktop || val is IValuationDiscriminatedAdCheckPhysical) && val.Property.CanPerformPropertyAdCheckValuation() == false)
                AddMessage("AdCheck Valuation can not be updated", "", Messages);

            return 1;
        }
    }
}
