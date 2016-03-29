using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("ManualValuationOutbuildingRoof",
"Validates Manual Validation Outbuilding Roof",
   "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationOutbuildingRoof")]
    [RuleInfo]
    public class ManualValuationOutbuildingRoof : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationOutbuildingRoof rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationOutbuilding))
                throw new ArgumentException("The ManualValuationOutbuildingRoof rule expects the following objects to be passed: IValuationOutbuilding.");

            IValuationOutbuilding valuationOutbuilding = (IValuationOutbuilding)Parameters[0];

            // only fire this rule for manual valuations
            if (valuationOutbuilding.Valuation == null || valuationOutbuilding.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            if (valuationOutbuilding.ValuationRoofType == null)
            {
                AddMessage("Outbuilding Roof Type is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationOutbuildingExtent",
"Validates Manual Validation Outbuilding Extent",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationOutbuildingExtent")]
    [RuleInfo]
    public class ManualValuationOutbuildingExtent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationOutbuildingExtent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationOutbuilding))
                throw new ArgumentException("The ManualValuationOutbuildingExtent rule expects the following objects to be passed: IValuationOutbuilding.");

            IValuationOutbuilding valuationOutbuilding = (IValuationOutbuilding)Parameters[0];

            // only fire this rule for manual valuations
            if (valuationOutbuilding.Valuation == null || valuationOutbuilding.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            double extent = valuationOutbuilding.Extent.HasValue ? valuationOutbuilding.Extent.Value : 0;

            if (extent == 0)
            {
                AddMessage("Outbuilding Extent (sq meters) is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationOutbuildingRate",
"Validates Manual Validation Outbuilding Rate",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationOutbuildingRate")]
    [RuleInfo]
    public class ManualValuationOutbuildingRate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationOutbuildingRate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationOutbuilding))
                throw new ArgumentException("The ManualValuationOutbuildingRate rule expects the following objects to be passed: IValuationOutbuilding.");

            IValuationOutbuilding valuationOutbuilding = (IValuationOutbuilding)Parameters[0];

            // only fire this rule for manual valuations
            if (valuationOutbuilding.Valuation == null || valuationOutbuilding.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            double rate = valuationOutbuilding.Rate.HasValue ? valuationOutbuilding.Rate.Value : 0;

            if (rate == 0)
            {
                AddMessage("Outbuilding Rate (R /sq meter) is required", "", Messages);
            }

            return 1;
        }
    }
}
