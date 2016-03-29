using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AssetsLiabilitiesModelManagerSpecs
{
    public class when_populating_a_fixed_property_asset : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<AssetItem> comcorpAssetItems;
        private static ApplicantAssetLiabilityModel result;
        private static AssetItem fixedPropertyAsset;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpAssetItems = IntegrationServiceTestHelper.PopulateAssetItems();
            fixedPropertyAsset = comcorpAssetItems.Where(x => x.SAHLAssetDesc == "Fixed Property").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateAssets(comcorpAssetItems)
                .Where(x => x.AssetLiabilityType == AssetLiabilityType.FixedProperty)
                .FirstOrDefault();
        };

        private It should_add_the_fixed_property = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_set_the_asset_value_to_SAHL_Asset_Value = () =>
        {
            result.AssetValue.ShouldEqual(Convert.ToDouble(fixedPropertyAsset.SAHLAssetValue));
        };

        private It should_set_the_liability_value_to_the_asset_outstanding_liability_value = () =>
        {
            result.LiabilityValue.ShouldEqual(Convert.ToDouble(fixedPropertyAsset.AssetOutstandingLiability));
        };

        private It should_set_the_date_the_fixed_property_was_acquired = () =>
        {
            result.Date.ShouldEqual(fixedPropertyAsset.DateAssetAcquired);
        };

        private It should_add_the_fixed_property_address_as_free_format_address = () =>
        {
            result.Address.ShouldBeOfExactType<FreeTextAddressModel>();
        };

        private It should_use_the_physical_address_line_1_as_the_first_free_text_field = () =>
        {
            result.Address.FreeText1.ShouldEqual(fixedPropertyAsset.AssetPhysicalAddressLine1);
        };

        private It should_use_the_physical_address_line_2_as_the_second_free_text_field = () =>
        {
            result.Address.FreeText2.ShouldEqual(fixedPropertyAsset.AssetPhysicalAddressLine2);
        };

        private It should_concatenate_the_physical_address_suburb_and_postal_code_as_the_third_free_text_field = () =>
        {
            result.Address.FreeText3.ShouldEqual(string.Format("{0} {1}", fixedPropertyAsset.AssetPhysicalAddressSuburb, fixedPropertyAsset.AssetPhysicalAddressCode));
        };

        private It should_use_the_physical_address_city_as_the_fourth_free_text_field = () =>
        {
            result.Address.FreeText4.ShouldEqual(fixedPropertyAsset.AssetPhysicalAddressCity);
        };

        private It should_use_the_physical_address_province_as_the_fifth_free_text_field = () =>
        {
            result.Address.FreeText5.ShouldEqual(validationUtils.MapComcorpToSAHLProvince(fixedPropertyAsset.AssetPhysicalProvince));
        };
    }
}