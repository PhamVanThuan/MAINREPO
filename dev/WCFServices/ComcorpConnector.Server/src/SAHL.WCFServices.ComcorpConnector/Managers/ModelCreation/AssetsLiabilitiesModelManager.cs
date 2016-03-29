using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class AssetsLiabilitiesModelManager : IAssetsLiabilitiesModelManager
    {
        private IValidationUtils validationUtils;

        public AssetsLiabilitiesModelManager(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public List<ApplicantAssetLiabilityModel> PopulateAssets(List<AssetItem> comcorpAssetItems)
        {
            List<ApplicantAssetLiabilityModel> applicantAssets = new List<ApplicantAssetLiabilityModel>();

            foreach (var asset in comcorpAssetItems)
            {
                if (String.IsNullOrWhiteSpace(asset.SAHLAssetDesc))
                {
                    continue;
                }

                AssetLiabilityType assetLiabilityType = validationUtils.ParseEnum<AssetLiabilityType>(asset.SAHLAssetDesc);
                switch (assetLiabilityType)
                {
                    case AssetLiabilityType.FixedProperty:

                        FreeTextAddressModel assetAddress = new FreeTextAddressModel(AddressType.Residential, asset.AssetPhysicalAddressLine1,
                            asset.AssetPhysicalAddressLine2, asset.AssetPhysicalAddressSuburb + " " + asset.AssetPhysicalAddressCode, asset.AssetPhysicalAddressCity,
                            validationUtils.MapComcorpToSAHLProvince(asset.AssetPhysicalProvince), string.IsNullOrWhiteSpace(asset.AssetPhysicalCountry) ? "South Africa" : asset.AssetPhysicalCountry);

                        ApplicantFixedPropertyAssetModel applicantFixedPropertyAssetModel = new ApplicantFixedPropertyAssetModel(assetAddress, Convert.ToDouble(asset.SAHLAssetValue),
                            Convert.ToDouble(asset.AssetOutstandingLiability), asset.DateAssetAcquired);
                        applicantAssets.Add(applicantFixedPropertyAssetModel);
                        break;

                    case AssetLiabilityType.LifeAssurance:
                        ApplicantLifeAssuranceAssetModel applicantLifeAssuranceAssetModel = new ApplicantLifeAssuranceAssetModel(Convert.ToDouble(asset.SAHLAssetValue), asset.AssetCompanyName);
                        applicantAssets.Add(applicantLifeAssuranceAssetModel);
                        break;

                    case AssetLiabilityType.ListedInvestments:
                        ApplicantListedInvestmentAssetModel applicantListedInvestmentAssetModel = new ApplicantListedInvestmentAssetModel(
                            Convert.ToDouble(asset.SAHLAssetValue),
                            asset.AssetCompanyName
                            );
                        applicantAssets.Add(applicantListedInvestmentAssetModel);
                        break;

                    case AssetLiabilityType.OtherAsset:
                        ApplicantOtherAssetModel applicantOtherAssetModel = new ApplicantOtherAssetModel(
                            Convert.ToDouble(asset.SAHLAssetValue),
                            Convert.ToDouble(asset.AssetOutstandingLiability),
                            asset.AssetDescription
                            );
                        applicantAssets.Add(applicantOtherAssetModel);
                        break;

                    case AssetLiabilityType.UnlistedInvestments:
                        ApplicantUnListedInvestmentAssetModel applicantUnListedInvestmentAssetModel = new ApplicantUnListedInvestmentAssetModel(
                            Convert.ToDouble(asset.SAHLAssetValue),
                            asset.AssetCompanyName
                            );
                        applicantAssets.Add(applicantUnListedInvestmentAssetModel);
                        break;

                    default:
                        break;
                }
            }
            return applicantAssets;
        }

        public List<ApplicantAssetLiabilityModel> PopulateLiabilities(List<LiabilityItem> comcorpLiabilityItems)
        {
            List<ApplicantAssetLiabilityModel> applicantLiabilities = new List<ApplicantAssetLiabilityModel>();

            foreach (var liability in comcorpLiabilityItems)
            {
                if (String.IsNullOrWhiteSpace(liability.SAHLLiabilityDesc))
                {
                    continue;
                }

                AssetLiabilityType assetLiabilityType = validationUtils.ParseEnum<AssetLiabilityType>(liability.SAHLLiabilityDesc);

                switch (assetLiabilityType)
                {
                    case AssetLiabilityType.FixedLongTermInvestment:
                        ApplicantFixedLongTermLiabilityModel applicantFixedLongTermLiabilityModel = new ApplicantFixedLongTermLiabilityModel(
                            Convert.ToDouble(liability.SAHLLiabilityValue),
                            liability.LiabilityCompanyName);
                        applicantLiabilities.Add(applicantFixedLongTermLiabilityModel);
                        break;

                    case AssetLiabilityType.LiabilityLoan:
                        AssetLiabilitySubType assetLiabilitySubType = validationUtils.ParseEnum<AssetLiabilitySubType>(liability.LiabilityLoanType);

                        ApplicantLiabilityLoanModel applicantLiabilityLoanModel = new ApplicantLiabilityLoanModel(
                            assetLiabilitySubType,
                            liability.LiabilityCompanyName,
                            Convert.ToDouble(liability.SAHLLiabilityCost),
                            Convert.ToDouble(liability.SAHLLiabilityValue),
                            liability.LiabilityDateRepayable
                            );
                        applicantLiabilities.Add(applicantLiabilityLoanModel);
                        break;

                    case AssetLiabilityType.LiabilitySurety:
                        ApplicantLiabilitySuretyModel applicantLiabilitySuretyModel = new ApplicantLiabilitySuretyModel(
                            Convert.ToDouble(liability.LiabilityAssetValue),
                            Convert.ToDouble(liability.SAHLLiabilityValue),
                            liability.LiabilityDescription
                            );
                        applicantLiabilities.Add(applicantLiabilitySuretyModel);
                        break;

                    default:
                        break;
                }
            }
            return applicantLiabilities;
        }
    }
}