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
    public class when_the_physical_address_suburb_is_empty : WithCoreFakes
    {
        private static AddressModelManager modelManager;
        private static Applicant applicant;
        private static AddressModel applicantResidentialAddress;
        private static List<AddressModel> applicantAddresses;
        private static ResidentialAddressModel propertyAddress;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AddressModelManager(validationUtils);
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.CoApplicant, MaritalStatus.Married_CommunityofProperty,
                false, false, false, false, true);
            propertyAddress = new ResidentialAddressModel("2", "Oakland Avenue", string.Empty, string.Empty, string.Empty, "Hillcrest", "Hillcrest", "Kwazulu-Natal", "3610", "South Africa",
                true);
            applicant.PhysicalAddressSuburb = string.Empty;
        };

        private Because of = () =>
        {
            applicantAddresses = modelManager.PopulateAddresses(applicant, propertyAddress, null);
            applicantResidentialAddress = applicantAddresses.Where(x => x.AddressType == AddressType.Residential && x.AddressFormat == AddressFormat.FreeText)
                .FirstOrDefault();
        };

        private It should_not_add_the_residential_address = () =>
        {
            applicantResidentialAddress.ShouldBeNull();
        };

        private It should_still_add_the_postal_address = () =>
        {
            applicantAddresses.Where(x => x.AddressType == AddressType.Postal && x.AddressFormat == AddressFormat.FreeText)
            .FirstOrDefault()
            .ShouldNotBeNull();
        };

        private It should_still_add_the_property_address = () =>
        {
            applicantAddresses.ShouldContain(propertyAddress);
        };
    }
}