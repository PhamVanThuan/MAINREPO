using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("LightStoneAutomatedValuationUpdate",
   "LightStone Valuation can not be updated",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Valuations.LightStoneAutomatedValuationUpdate")]
    [RuleInfo]
    public class LightStoneAutomatedValuationUpdate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];
            
            if (val is IValuationDiscriminatedLightstoneAVM && val.Property.CanPerformPropertyLightStoneValuation() == false)
           
                AddMessage("LightStone Valuation can not be updated", "", Messages);

           return 1;
        }
    }
}
