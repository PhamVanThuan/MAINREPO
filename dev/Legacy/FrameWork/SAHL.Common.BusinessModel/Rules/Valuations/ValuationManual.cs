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

    [RuleDBTag("ManualValuationClassification",
"Validate Manual Validation Classification",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ManualValuationClassification")]
    [RuleInfo]
    public class ManualValuationClassification : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualValuationClassification rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IValuationDiscriminatedSAHLManual))
                throw new ArgumentException("The ManualValuationClassification rule expects the following objects to be passed: IValuationDiscriminatedSAHLManual.");

            IValuationDiscriminatedSAHLManual val = (IValuationDiscriminatedSAHLManual)Parameters[0];

            // if there is no classification and this is a new valuation or an existing active valuation then validate
            if (val.ValuationClassification == null && (val.Key == 0 || val.IsActive == true))
            {
                AddMessage("Valuation Classification is required", "", Messages);
            }

            return 1;
        }
    }
}
