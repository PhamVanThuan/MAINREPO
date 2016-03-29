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
    public class when_populating_a_fixed_long_term_investment : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<LiabilityItem> comcorpLiabilityItems;
        private static ApplicantAssetLiabilityModel result;
        private static LiabilityItem fixedLongTermInvestment;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpLiabilityItems = IntegrationServiceTestHelper.PopulateLiabilityItems();
            fixedLongTermInvestment = comcorpLiabilityItems.Where(x => x.SAHLLiabilityDesc == "Fixed Long Term Investment").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateLiabilities(comcorpLiabilityItems)
                .Where(x => x.AssetLiabilityType == AssetLiabilityType.FixedLongTermInvestment)
                .FirstOrDefault();
        };


        private It should_map_the_comcorp_item_as_a_fixed_long_term_investment = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_set_the_company_name_field_to_the_LiabilityCompanyName_field = () =>
        {
            result.CompanyName.ShouldEqual(fixedLongTermInvestment.LiabilityCompanyName);
        };

        private It should_set_the_liability_value_to_the_SAHLLiabilityValue_field = () =>
        {
            result.LiabilityValue.ShouldEqual(Convert.ToDouble(fixedLongTermInvestment.SAHLLiabilityValue));
        };

        private It should_not_set_the_date_repayable = () =>
        {
            result.Date.ShouldBeNull();
        };

        private It should_not_set_cost_of_the_liability = () =>
        {
            result.Cost.ShouldBeNull();
        };
    }
}