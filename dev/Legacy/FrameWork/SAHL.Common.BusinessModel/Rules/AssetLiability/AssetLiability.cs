using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.AssetLiability
{
    [RuleDBTag("AssetLiabilityLoanLiabilityMin",
    "The Liability Value must be greater than the min value",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AssetLiability.AssetLiabilityLoanLiabilityMin")]
    [RuleParameterTag(new string[] { "@MinAssetLiabilityLoanLiabilityAmt,0,9" })]
    [RuleInfo]
    public class AssetLiabilityLoanLiabilityMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AssetLiabilityLoanLiabilityMin rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAssetLiabilityLiabilityLoan))
                throw new ArgumentException("The AssetLiabilityLoanLiabilityMin rule expects the following objects to be passed: IAssetLiabilityLiabilityLoan.");

            #endregion

            # region Rule Check

            IAssetLiabilityLiabilityLoan _asset = Parameters[0] as IAssetLiabilityLiabilityLoan;
            double minAssetLiabilityLoanLiabilityAmt = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (_asset.LiabilityValue <= minAssetLiabilityLoanLiabilityAmt)
                AddMessage(String.Format("Liability value must be greater than {0}.", minAssetLiabilityLoanLiabilityAmt), "", Messages);
            else
                return 0;

            #endregion

            return 0;
        }
    }

    [RuleDBTag("AssetLiabilityCompanyName",
    "The Company Name must not be null",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AssetLiability.AssetLiabilityCompanyName")]
    [RuleInfo]
    public class AssetLiabilityCompanyName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AssetLiabilityCompanyName rule expects a Domain object to be passed.");

            IAssetLiability assetLiability = Parameters[0] as IAssetLiability;

            if (assetLiability == null)
                throw new ArgumentException("The AssetLiabilityCompanyName rule expects the following objects to be passed: IAssetLiability.");

            #endregion

            # region Rule Check

            string errorMsg = AssetLiabilityCompanyNameHelper(assetLiability);
            if (!string.IsNullOrEmpty(errorMsg))
                AddMessage(errorMsg, errorMsg, Messages);

            #endregion

            return 0;
        }

        public string AssetLiabilityCompanyNameHelper(IAssetLiability assetLiability)
        {
            string msg = string.Empty;

            IAssetLiabilityLifeAssurance alLifeAssurance = assetLiability as IAssetLiabilityLifeAssurance;

            if (alLifeAssurance != null && string.IsNullOrEmpty(alLifeAssurance.CompanyName.Trim()))
                return string.Format("Please enter Company Name for {0}.", AssetLiabilityTypes.LifeAssurance.ToString());

            IAssetLiabilityInvestmentPrivate alInvestmentPrivate = assetLiability as IAssetLiabilityInvestmentPrivate;

            if (alInvestmentPrivate != null && string.IsNullOrEmpty(alInvestmentPrivate.CompanyName.Trim()))
                return string.Format("Please enter Company Name for {0}.", AssetLiabilityTypes.UnlistedInvestments.ToString());

            IAssetLiabilityInvestmentPublic alInvestmentPublic = assetLiability as IAssetLiabilityInvestmentPublic;
            if (alInvestmentPublic != null && string.IsNullOrEmpty(alInvestmentPublic.CompanyName.Trim()))
                return string.Format("Please enter Company Name for {0}.", AssetLiabilityTypes.ListedInvestments.ToString());

            IAssetLiabilityFixedLongTermInvestment assetLongTerm = assetLiability as IAssetLiabilityFixedLongTermInvestment;
            if (assetLongTerm != null && string.IsNullOrEmpty(assetLongTerm.CompanyName.Trim()))
                return string.Format("Please enter Company Name for {0}.", AssetLiabilityTypes.FixedLongTermInvestment.ToString());

            return msg;
        }
    }

    [RuleDBTag("AssetLiabilityDescription",
    "The Description must not be null",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AssetLiability.AssetLiabilityDescription")]
    [RuleInfo]
    public class AssetLiabilityDescription : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AssetLiabilityDescription rule expects a Domain object to be passed.");

            IAssetLiability assetLiability = Parameters[0] as IAssetLiability;

            if (assetLiability == null)
                throw new ArgumentException("The AssetLiabilityDescription rule expects the following objects to be passed: IAssetLiability.");

            #endregion

            # region Rule Check

            string errorMsg = AssetLiabilityDescriptionHelper(assetLiability);
            if (!string.IsNullOrEmpty(errorMsg))
                AddMessage(errorMsg, errorMsg, Messages);

            #endregion

            return 0;
        }

        public string AssetLiabilityDescriptionHelper(IAssetLiability assetLiability)
        {
            string msg = string.Empty;

            IAssetLiabilityLiabilitySurety alLiabilitySurety = assetLiability as IAssetLiabilityLiabilitySurety;

            if (alLiabilitySurety != null && string.IsNullOrEmpty(alLiabilitySurety.Description.Trim()))
                return string.Format("Please enter the description for {0}.", AssetLiabilityTypes.LiabilitySurety.ToString());

            IAssetLiabilityOther alOther = assetLiability as IAssetLiabilityOther;

            if (alOther != null && string.IsNullOrEmpty(alOther.Description.Trim()))
                return string.Format("Please enter the description for {0}.", AssetLiabilityTypes.OtherAsset.ToString());

            return msg;
        }
    }

    [RuleDBTag("AssetLiabilityFinancialInstitution",
    "The Description must not be null",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.AssetLiability.AssetLiabilityFinancialInstitution")]
    [RuleInfo]
    public class AssetLiabilityFinancialInstitution : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AssetLiabilityFinancialInstitution rule expects a Domain object to be passed.");

            IAssetLiability assetLiability = Parameters[0] as IAssetLiability;

            if (assetLiability == null)
                throw new ArgumentException("The AssetLiabilityFinancialInstitution rule expects the following objects to be passed: IAssetLiability.");

            #endregion

            # region Rule Check

            string errorMsg = AssetLiabilityFinancialInstitutionHelper(assetLiability);
            if (!string.IsNullOrEmpty(errorMsg))
                AddMessage(errorMsg, errorMsg, Messages);

            #endregion

            return 0;
        }

        public string AssetLiabilityFinancialInstitutionHelper(IAssetLiability assetLiability)
        {
            string msg = string.Empty;

            IAssetLiabilityLiabilityLoan alLiabilityLoan = assetLiability as IAssetLiabilityLiabilityLoan;

            if (alLiabilityLoan != null && string.IsNullOrEmpty(alLiabilityLoan.FinancialInstitution.Trim()))
                return string.Format("Please enter the FinancialInstitution for {0}.", AssetLiabilityTypes.LiabilityLoan.ToString());

            return msg;
        }
    }


}
