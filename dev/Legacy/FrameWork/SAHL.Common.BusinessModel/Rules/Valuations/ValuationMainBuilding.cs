using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("ValuationMainBuildingValuationMandatory",
"Valuation MainBuilding must have a Valuation.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ValuationMainBuildingValuationMandatory")]
    [RuleInfo]
    public class ValuationMainBuildingValuationMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValuationMainBuildingValuationMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationMainBuilding))
                throw new ArgumentException("The ValuationMainBuildingValuationMandatory rule expects the following objects to be passed: IValuationMainBuilding.");

            IValuationMainBuilding valuationMainBuilding = (IValuationMainBuilding)Parameters[0];

            if (valuationMainBuilding.Valuation == null)
            {
                AddMessage("Valuation MainBuilding must have a Valuation.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ManualValuationMainBuildingRoof",
 "Validates Manual Validation MainBuilding Roof",
   "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationMainBuildingRoof")]
    [RuleInfo]
    public class ManualValuationMainBuildingRoof : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationMainBuildingRoof rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationMainBuilding))
                throw new ArgumentException("The ManualValuationMainBuildingRoof rule expects the following objects to be passed: IValuationMainBuilding.");

            IValuationMainBuilding valuationMainBuilding = (IValuationMainBuilding)Parameters[0];

            // only fire this rule for manul valuations
            if (valuationMainBuilding.Valuation == null || valuationMainBuilding.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            if (valuationMainBuilding.ValuationRoofType == null)
            {
                AddMessage("MainBuilding Roof Type is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationMainBuildingExtent",
"Validates Manual Validation MainBuilding Extent",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationMainBuildingExtent")]
    [RuleInfo]
    public class ManualValuationMainBuildingExtent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationMainBuildingExtent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationMainBuilding))
                throw new ArgumentException("The ManualValuationMainBuildingExtent rule expects the following objects to be passed: IValuationMainBuilding.");

            IValuationMainBuilding valuationMainBuilding = (IValuationMainBuilding)Parameters[0];

            // only fire this rule for manul valuations
            if (valuationMainBuilding.Valuation == null || valuationMainBuilding.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            double extent = valuationMainBuilding.Extent.HasValue ? valuationMainBuilding.Extent.Value : 0;

            if (extent == 0)
            {
                AddMessage("MainBuilding Extent (sq meters) is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationMainBuildingRate",
"Validates Manual Validation MainBuilding Rate",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationMainBuildingRate")]
    [RuleInfo]
    public class ManualValuationMainBuildingRate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationMainBuildingRate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationMainBuilding))
                throw new ArgumentException("The ManualValuationMainBuildingRate rule expects the following objects to be passed: IValuationMainBuilding.");

            IValuationMainBuilding valuationMainBuilding = (IValuationMainBuilding)Parameters[0];

            // only fire this rule for manul valuations
            if (valuationMainBuilding.Valuation == null || valuationMainBuilding.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            double rate = valuationMainBuilding.Rate.HasValue ? valuationMainBuilding.Rate.Value : 0;

            if (rate == 0)
            {
                AddMessage("MainBuilding Rate (R /sq meter) is required", "", Messages);
            }

            return 1;
        }
    }
}
