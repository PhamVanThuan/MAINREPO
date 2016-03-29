using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class AddressModelManager : IAddressModelManager
    {
        private IValidationUtils validationUtils;

        public AddressModelManager(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public List<AddressModel> PopulateAddresses(Applicant comcorpApplicant, ResidentialAddressModel propertyAddress, List<AssetItem> comcorpAssetItems)
        {
            var addresses = new List<AddressModel>();
            //Residential Address
            if (!String.IsNullOrWhiteSpace(comcorpApplicant.PhysicalAddressSuburb))
            {
                FreeTextAddressModel residentialAddress = new FreeTextAddressModel(AddressType.Residential, comcorpApplicant.PhysicalAddressLine1,
                    comcorpApplicant.PhysicalAddressLine2, string.Format("{0} {1}", comcorpApplicant.PhysicalAddressSuburb, comcorpApplicant.PhysicalAddressCode),
                    comcorpApplicant.PhysicalAddressCity, validationUtils.MapComcorpToSAHLProvince(comcorpApplicant.PhysicalProvince),
                    String.IsNullOrWhiteSpace(comcorpApplicant.PhysicalCountry) ? "South Africa" : comcorpApplicant.PhysicalCountry);
                addresses.Add(residentialAddress);
            }
            //Postal Address
            if (!String.IsNullOrWhiteSpace(comcorpApplicant.PostalAddressSuburb))
            {
                FreeTextAddressModel postalAddress = new FreeTextAddressModel(AddressType.Postal, comcorpApplicant.PostalAddressLine1, comcorpApplicant.PostalAddressLine2,
                    string.Format("{0} {1}", comcorpApplicant.PostalAddressSuburb, comcorpApplicant.PostalAddressCode), comcorpApplicant.PostalAddressCity,
                    validationUtils.MapComcorpToSAHLProvince(comcorpApplicant.PostalProvince),
                    String.IsNullOrWhiteSpace(comcorpApplicant.PostalCountry) ? "South Africa" : comcorpApplicant.PostalCountry);
                addresses.Add(postalAddress);
            }
            //Property Address
            addresses.Add(propertyAddress);

            //Fixed Property Asset Address
            if (comcorpAssetItems != null)
            {
                foreach (var asset in comcorpAssetItems)
                {
                    if (String.IsNullOrWhiteSpace(asset.SAHLAssetDesc))
                    {
                        continue;
                    }

                    AssetLiabilityType assetLiabilityType = validationUtils.ParseEnum<AssetLiabilityType>(asset.SAHLAssetDesc);

                    if (assetLiabilityType == AssetLiabilityType.FixedProperty)
                    {
                        FreeTextAddressModel assetAddress = new FreeTextAddressModel(AddressType.Residential, asset.AssetPhysicalAddressLine1,
                                                  asset.AssetPhysicalAddressLine2, asset.AssetPhysicalAddressSuburb + " " + asset.AssetPhysicalAddressCode, asset.AssetPhysicalAddressCity,
                                                  validationUtils.MapComcorpToSAHLProvince(asset.AssetPhysicalProvince),
                                                  String.IsNullOrWhiteSpace(asset.AssetPhysicalCountry) ? "South Africa" : asset.AssetPhysicalCountry);
                        addresses.Add(assetAddress);
                    }
                }
            }
            return addresses;
        }

        public ResidentialAddressModel PopulateResidentialAddressFromComcorpProperty(Property comcorpProperty)
        {
            ResidentialAddressModel propertyAddress = new ResidentialAddressModel(comcorpProperty.StreetNo, comcorpProperty.StreetName,
                null, null, null, comcorpProperty.AddressSuburb, comcorpProperty.AddressCity, validationUtils.MapComcorpToSAHLProvince(comcorpProperty.Province),
                comcorpProperty.PostalCode, "South Africa", comcorpProperty.UsePropertyAddressAsDomiciliumAddress
                );
            return propertyAddress;
        }

        public PropertyAddressModel PopulatePropertyAddressFromComcorpProperty(Property comcorpProperty)
        {
            var propertyAddressModel = new PropertyAddressModel(string.Empty, string.Empty, string.Empty,
                comcorpProperty.StreetNo, comcorpProperty.StreetName, comcorpProperty.AddressSuburb,
                comcorpProperty.AddressCity, validationUtils.MapComcorpToSAHLProvince(comcorpProperty.Province), comcorpProperty.PostalCode,
                comcorpProperty.StandErfNo, comcorpProperty.PortionNo, "South Africa",
                comcorpProperty.UsePropertyAddressAsDomiciliumAddress
                );
            return propertyAddressModel;
        }
    }
}