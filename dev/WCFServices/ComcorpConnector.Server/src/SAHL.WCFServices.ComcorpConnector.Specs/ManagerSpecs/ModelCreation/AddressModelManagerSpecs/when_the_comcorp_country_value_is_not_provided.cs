using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AddressModelManagerSpecs
{
    public class when_the_comcorp_country_value_is_not_provided : WithFakes
    {
        private static AddressModelManager modelManager;
        private static Applicant applicant;
        private static List<AddressModel> addresses;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AddressModelManager(validationUtils);
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.CoApplicant, MaritalStatus.Married_CommunityofProperty,
                false, false, false, false, true);
            applicant.PhysicalCountry = string.Empty;
            applicant.PostalCountry = string.Empty;
        };

        private Because of = () =>
        {
            addresses = modelManager.PopulateAddresses(applicant, null, null);

        };

        private It should_use_south_africa_as_the_default_country_for_the_postal_address = () =>
        {
            addresses.Where(x => x.AddressType == AddressType.Postal).First().Country.ShouldEqual("South Africa");
        };

        private It should_use_south_africa_as_the_default_country_for_the_residential_address = () =>
        {
            addresses.Where(x => x.AddressType == AddressType.Residential).First().Country.ShouldEqual("South Africa");
        };
    }
}