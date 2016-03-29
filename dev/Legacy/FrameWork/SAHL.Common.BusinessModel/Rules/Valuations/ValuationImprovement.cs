using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("ManualValuationImprovementDate",
"Validate Manual Validation Improvement Date",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationImprovementDate")]
    [RuleInfo]
    public class ManualValuationImprovementDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationImprovementDate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationImprovement))
                throw new ArgumentException("The ManualValuationImprovementDate rule expects the following objects to be passed: IValuationImprovement.");

            IValuationImprovement valuationImprovement = (IValuationImprovement)Parameters[0];

            // only fire this rule for manual valuations
            if (valuationImprovement.Valuation == null || valuationImprovement.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            if (valuationImprovement.ImprovementDate.HasValue == false)
            {
                AddMessage("Improvement Date is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationImprovementValue",
"Validate Manual Validation Improvement Value",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationImprovementValue")]
    [RuleInfo]
    public class ManualValuationImprovementValue : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationImprovementValue rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationImprovement))
                throw new ArgumentException("The ManualValuationImprovementValue rule expects the following objects to be passed: IValuationImprovement.");

            IValuationImprovement valuationImprovement = (IValuationImprovement)Parameters[0];

            // only fire this rule for manual valuations
            if (valuationImprovement.Valuation == null || valuationImprovement.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            if (valuationImprovement.ImprovementValue <= 0)
            {
                AddMessage("Improvement Value is required", "", Messages);
            }

            return 1;
        }
    }

}
