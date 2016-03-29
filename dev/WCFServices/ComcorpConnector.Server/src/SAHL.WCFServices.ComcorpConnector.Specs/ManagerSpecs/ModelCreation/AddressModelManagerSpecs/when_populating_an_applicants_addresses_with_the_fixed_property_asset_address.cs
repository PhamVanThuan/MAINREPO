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

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AddressModelManagerSpecs
{
    public class when_populating_an_applicants_addresses_with_the_fixed_property_asset_address : WithCoreFakes
    {
        private static AddressModelManager modelManager;
        private static Applicant applicant;
        private static AddressModel applicantPropertyAddress;
        private static ResidentialAddressModel propertyAddress;
        private static List<AssetItem> assetItems;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AddressModelManager(validationUtils);
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.CoApplicant, MaritalStatus.Married_CommunityofProperty,
                false, false, false, false, true);
            propertyAddress = new ResidentialAddressModel("2", "Oakland Avenue", string.Empty, string.Empty, string.Empty, "Hillcrest", "Hillcrest", "Kwazulu-Natal", "3610", "South Africa",
                true);
            assetItems = IntegrationServiceTestHelper.PopulateAssetItems();
        };

        private Because of = () =>
        {
            applicantPropertyAddress = modelManager.PopulateAddresses(applicant, propertyAddress, assetItems)
                .Where(x => x.AddressType == AddressType.Residential && x.AddressFormat == AddressFormat.Street)
                .FirstOrDefault();
        };

        private It should_add_the_property_address = () =>
        {
            applicantPropertyAddress.ShouldNotBeNull();
        };

        private It should_be_a_residential_address = () =>
        {
            applicantPropertyAddress.ShouldBeOfExactType<ResidentialAddressModel>();
        };

        private It should_be_the_same_as_property_address_provided = () =>
        {
            applicantPropertyAddress.ShouldEqual(propertyAddress);
        };

        private It should_be_in_street_format = () =>
        {
            applicantPropertyAddress.AddressFormat.ShouldEqual(AddressFormat.Street);
        };
    }
}