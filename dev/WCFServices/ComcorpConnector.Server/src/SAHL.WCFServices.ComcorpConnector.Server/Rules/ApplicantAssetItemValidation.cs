using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Server.Rules
{
    public class ApplicantAssetItemValidation : IDomainRule<Applicant>
    {
        private IValidationUtils validationUtils;
        private IRuleHelper ruleHelper = new RuleHelper();

        public ApplicantAssetItemValidation(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(ISystemMessageCollection messages, Applicant applicant)
        {
            if (applicant.AssetItems != null && applicant.AssetItems.Count > 0)
            {
                foreach (var asset in applicant.AssetItems)
                {
                    if (String.IsNullOrWhiteSpace(asset.SAHLAssetDesc))
                    {
                        continue;
                    }

                    string applicantName = ruleHelper.GetApplicantNameForErrorMessage(applicant);

                    AssetLiabilityType assetLiabilityType = validationUtils.ParseEnum<AssetLiabilityType>(asset.SAHLAssetDesc);
                    if (assetLiabilityType == AssetLiabilityType.FixedProperty)
                    {
                        if (String.IsNullOrWhiteSpace(asset.AssetPhysicalAddressLine1))
                        {
                            messages.AddMessage(new SystemMessage(applicantName + "Address Line 1 is required for Fixed Property Asset address.", SystemMessageSeverityEnum.Error));
                        }
                        if (String.IsNullOrWhiteSpace(asset.AssetPhysicalAddressSuburb))
                        {
                            messages.AddMessage(new SystemMessage(applicantName + "Suburb is required for Fixed Property Asset address.", SystemMessageSeverityEnum.Error));
                        }
                        if (String.IsNullOrWhiteSpace(asset.AssetPhysicalAddressCode))
                        {
                            messages.AddMessage(new SystemMessage(applicantName + "Post Code is required for Fixed Property Asset address.", SystemMessageSeverityEnum.Error));
                        }
                        if (String.IsNullOrWhiteSpace(asset.AssetPhysicalAddressCity))
                        {
                            messages.AddMessage(new SystemMessage(applicantName + "City is required for Fixed Property Asset address.", SystemMessageSeverityEnum.Error));
                        }
                        if (String.IsNullOrWhiteSpace(asset.AssetPhysicalProvince))
                        {
                            messages.AddMessage(new SystemMessage(applicantName + "Province is required for Fixed Property Asset address.", SystemMessageSeverityEnum.Error));
                        }
                    }
                }
            }
        }
    }
}