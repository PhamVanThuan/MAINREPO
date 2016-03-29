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
    public class when_populating_a_liability_surety : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<LiabilityItem> comcorpLiabilityItems;
        private static ApplicantAssetLiabilityModel result;
        private static LiabilityItem liabilitySurety;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpLiabilityItems = IntegrationServiceTestHelper.PopulateLiabilityItems();
            liabilitySurety = comcorpLiabilityItems.Where(x => x.SAHLLiabilityDesc == "Liability Surety").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateLiabilities(comcorpLiabilityItems)
                .Where(x => x.AssetLiabilityType == AssetLiabilityType.LiabilitySurety)
                .FirstOrDefault();
        };

        private It should_map_the_comcorp_item_as_a_liability_surety = () =>
        {
            result.AssetLiabilityType.ShouldEqual(AssetLiabilityType.LiabilitySurety);
        };

        private It should_set_the_liability_value_to_the_SAHLLiabilityValue_field = () =>
        {
            result.LiabilityValue.ShouldEqual(Convert.ToDouble(liabilitySurety.SAHLLiabilityValue));
        };

        private It should_set_the_asset_value_to_the_LiabilityAssetValue_field = () =>
        {
            result.AssetValue.ShouldEqual(Convert.ToDouble(liabilitySurety.LiabilityAssetValue));
        };
    }
}