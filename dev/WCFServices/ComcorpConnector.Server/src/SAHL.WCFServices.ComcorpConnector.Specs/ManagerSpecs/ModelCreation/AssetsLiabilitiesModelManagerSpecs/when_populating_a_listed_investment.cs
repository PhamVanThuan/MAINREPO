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
    public class when_populating_a_listed_investment : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<AssetItem> comcorpAssetItems;
        private static ApplicantAssetLiabilityModel result;
        private static AssetItem listedInvestment;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpAssetItems = IntegrationServiceTestHelper.PopulateAssetItems();
            listedInvestment = comcorpAssetItems.Where(x => x.SAHLAssetDesc == "Listed Investments").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateAssets(comcorpAssetItems)
                .Where(x => x.AssetLiabilityType == AssetLiabilityType.ListedInvestments)
                .FirstOrDefault();
        };

        private It should_set_the_asset_value_to_SAHLAssetValue_field = () =>
        {
            result.AssetValue.ShouldEqual(Convert.ToDouble(listedInvestment.SAHLAssetValue));
        };

        private It should_set_the_company_name_to_the_AssetCompanyName_field = () =>
        {
            result.CompanyName.ShouldEqual(listedInvestment.AssetCompanyName);
        };

        private It should_not_set_any_liability_value = () =>
        {
            result.LiabilityValue.ShouldEqual(0);
        };

        private It should_not_set_any_description = () =>
        {
            result.Description.ShouldBeNull();
        };
    }
}