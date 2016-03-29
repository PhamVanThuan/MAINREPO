using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("ValuationCottageValuationMandatory",
"Valuation Cottage must have a Valuation.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ValuationCottageValuationMandatory")]
    [RuleInfo]
    public class ValuationCottageValuationMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValuationCottageValuationMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationCottage))
                throw new ArgumentException("The ValuationCottageValuationMandatory rule expects the following objects to be passed: IValuationCottage.");

            IValuationCottage valuationCottage = (IValuationCottage)Parameters[0];

            if (valuationCottage.Valuation == null)
            {
                AddMessage("Valuation Cottage must have a Valuation.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ManualValuationCottageRoof",
 "Validates Manual Validation Cottage Roof",
   "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationCottageRoof")]
    [RuleInfo]
    public class ManualValuationCottageRoof : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationCottageRoof rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationCottage))
                throw new ArgumentException("The ManualValuationCottageRoof rule expects the following objects to be passed: IValuationCottage.");

            IValuationCottage valuationCottage = (IValuationCottage)Parameters[0];

            // only fire this rule for manul valuations
            if (valuationCottage.Valuation == null || valuationCottage.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            if (valuationCottage.ValuationRoofType == null)
            {
                AddMessage("Cottage Roof Type is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationCottageExtent",
"Validates Manual Validation Cottage Extent",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationCottageExtent")]
    [RuleInfo]
    public class ManualValuationCottageExtent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationCottageExtent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationCottage))
                throw new ArgumentException("The ManualValuationCottageExtent rule expects the following objects to be passed: IValuationCottage.");

            IValuationCottage valuationCottage = (IValuationCottage)Parameters[0];

            // only fire this rule for manul valuations
            if (valuationCottage.Valuation == null || valuationCottage.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            double extent = valuationCottage.Extent.HasValue ? valuationCottage.Extent.Value : 0;

            if (extent == 0)
            {
                AddMessage("Cottage Extent (sq meters) is required", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ManualValuationCottageRate",
"Validates Manual Validation Cottage Rate",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationCottageRate")]
    [RuleInfo]
    public class ManualValuationCottageRate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationCottageRate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationCottage))
                throw new ArgumentException("The ManualValuationCottageRate rule expects the following objects to be passed: IValuationCottage.");

            IValuationCottage valuationCottage = (IValuationCottage)Parameters[0];

            // only fire this rule for manul valuations
            if (valuationCottage.Valuation == null || valuationCottage.Valuation.ValuationDataProviderDataService.Key != (int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation)
                return 1;

            double rate = valuationCottage.Rate.HasValue ? valuationCottage.Rate.Value : 0;

            if (rate == 0)
            {
                AddMessage("Cottage Rate (R /sq meter) is required", "", Messages);
            }

            return 1;
        }
    }
}
