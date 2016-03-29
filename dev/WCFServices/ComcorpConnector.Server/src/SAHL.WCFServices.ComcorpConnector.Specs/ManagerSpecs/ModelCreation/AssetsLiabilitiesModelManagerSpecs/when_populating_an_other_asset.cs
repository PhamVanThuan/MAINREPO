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
    public class when_populating_an_other_asset : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<AssetItem> comcorpAssetItems;
        private static ApplicantAssetLiabilityModel result;
        private static AssetItem otherAsset;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpAssetItems = IntegrationServiceTestHelper.PopulateAssetItems();
            otherAsset = comcorpAssetItems.Where(x => x.SAHLAssetDesc == "Other Asset").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateAssets(comcorpAssetItems)
                .Where(x => x.AssetLiabilityType == AssetLiabilityType.OtherAsset)
                .FirstOrDefault();
        };

        private It should_set_the_asset_value_to_SAHLAssetValue_field = () =>
        {
            result.AssetValue.ShouldEqual(Convert.ToDouble(otherAsset.SAHLAssetValue));
        };

        private It should_not_set_the_company_name_field = () =>
        {
            result.CompanyName.ShouldBeNull();
        };

        private It should_set_the_liability_value = () =>
        {
            result.LiabilityValue.ShouldEqual(Convert.ToDouble(otherAsset.AssetOutstandingLiability));
        };

        private It should_set_the_other_asset_description = () =>
        {
            result.Description.ShouldEqual(otherAsset.AssetDescription);
        };
    }
}