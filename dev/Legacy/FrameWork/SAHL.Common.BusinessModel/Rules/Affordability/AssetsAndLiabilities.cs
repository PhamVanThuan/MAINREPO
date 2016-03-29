using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Affordability
{
    [RuleDBTag("AssetValueMinimumValue",
    "Amount must be greater than zero",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Affordability.AssetValueMinimumValue")]
    [RuleInfo]
    public class AssetValueMinimumValue : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAssetLiability assetLiability = (IAssetLiability)Parameters[0];

            IAssetLiabilityFixedProperty assetFixedProp = assetLiability as IAssetLiabilityFixedProperty;

            if (assetFixedProp != null && assetFixedProp.AssetValue <= 0)
            {
                AddMessage("Asset Value must be greater than zero.", "Asset Value must be greater than zero ", Messages);
                return 0;
            }

            IAssetLiabilityInvestmentPrivate assetPrivate = assetLiability as IAssetLiabilityInvestmentPrivate;

            if (assetPrivate != null && assetPrivate.AssetValue <= 0)
            {
                AddMessage("Asset Value must be greater than zero.", "Asset Value must be greater than zero ", Messages);
                return 0;
            }

            IAssetLiabilityInvestmentPublic assetPublic = assetLiability as IAssetLiabilityInvestmentPublic;

            if (assetPublic != null && assetPublic.AssetValue <= 0)
            {
                AddMessage("Asset Value must be greater than zero.", "Asset Value must be greater than zero ", Messages);
                return 0;
            }

            IAssetLiabilityOther assetOther = assetLiability as IAssetLiabilityOther;

            if (assetOther != null && assetOther.AssetValue <= 0)
            {
                AddMessage("Asset Value must be greater than zero.", "Asset Value must be greater than zero ", Messages);
                return 0;
            }

            IAssetLiabilityLiabilitySurety assetSurety = assetLiability as IAssetLiabilityLiabilitySurety;

            if (assetSurety != null && assetSurety.AssetValue <= 0)
            {
                AddMessage("Asset Value must be greater than zero.", "Asset Value must be greater than zero ", Messages);
                return 0;
            }

            return 0;
        }
    }

    [RuleDBTag("AssetLiabilityDateAcquiredMax",
    "Date acquired can not be in the future",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Affordability.AssetLiabilityDateAcquiredMax")]
    [RuleInfo]
    public class AssetLiabilityDateAcquiredMax : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAssetLiability assetLiability = (IAssetLiability)Parameters[0];

            IAssetLiabilityFixedProperty assetFixedProp = assetLiability as IAssetLiabilityFixedProperty;
            if (assetFixedProp != null && assetFixedProp.DateAcquired > DateTime.Now.Date)
            {
                AddMessage("Date acquired can not be in the future", "Date acquired can not be in the future", Messages);
                return 0;
            }

            return 0;
        }
    }

    [RuleDBTag("AssetLiabilitySurrenderValueMin",
    "Surrender Value must be greater than zero",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Affordability.AssetLiabilitySurrenderValueMin")]
    [RuleInfo]
    public class AssetLiabilitySurrenderValueMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAssetLiability assetLiability = (IAssetLiability)Parameters[0];

            IAssetLiabilityLifeAssurance assetLife = assetLiability as IAssetLiabilityLifeAssurance;
            if (assetLife != null && assetLife.SurrenderValue <= 0)
            {
                AddMessage("Surrender Value must be greater than zero.", "Surrender Value must be greater than zero ", Messages);
                return 0;
            }

            return 0;
        }
    }

    [RuleDBTag("AssetLiabilityLiabilityValueMin",
    "Liability Value must be greater than zero",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Affordability.AssetLiabilityLiabilityValueMin")]
    [RuleInfo]
    public class AssetLiabilityLiabilityValueMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAssetLiability assetLiability = (IAssetLiability)Parameters[0];

            IAssetLiabilityFixedLongTermInvestment assetLongTerm = assetLiability as IAssetLiabilityFixedLongTermInvestment;
            if (assetLongTerm != null && assetLongTerm.LiabilityValue <= 0)
            {
                AddMessage("Liability Value must be greater than zero.", "Liability Value must be greater than zero ", Messages);
                return 0;
            }

            IAssetLiabilityLiabilityLoan assetLiabilityLiabilityLoan = assetLiability as IAssetLiabilityLiabilityLoan;
            if (assetLiabilityLiabilityLoan != null && assetLiabilityLiabilityLoan.LiabilityValue <= 0)
            {
                AddMessage("Liability Value must be greater than zero.", "Liability Value must be greater than zero ", Messages);
                return 0;
            }

            IAssetLiabilityLiabilitySurety assetLiabilityLiabilitySurety = assetLiability as IAssetLiabilityLiabilitySurety;
            if (assetLiabilityLiabilitySurety != null && assetLiabilityLiabilitySurety.LiabilityValue <= 0)
            {
                AddMessage("Liability Value must be greater than zero.", "Liability Value must be greater than zero ", Messages);
                return 0;
            }

            /* 
             http://sahls31:8181/trac/SAHL.db/ticket/9064
             Not required
             */
            //IAssetLiabilityFixedProperty assetLiabilityFixedProperty = assetLiability as IAssetLiabilityFixedProperty;
            //if (assetLiabilityFixedProperty != null && assetLiabilityFixedProperty.LiabilityValue <= 0)
            //{
            //    AddMessage("Liability Value must be greater than zero.", "Liability Value must be greater than zero ", Messages);
            //    return 0;
            //}

            //IAssetLiabilityOther assetLiabilityOther = assetLiability as IAssetLiabilityOther;
            //if (assetLiabilityOther != null && assetLiabilityOther.LiabilityValue <= 0)
            //{
            //    AddMessage("Liability Value must be greater than zero.", "Liability Value must be greater than zero ", Messages);
            //    return 0;
            //}

            return 0;
        }
    }
}
