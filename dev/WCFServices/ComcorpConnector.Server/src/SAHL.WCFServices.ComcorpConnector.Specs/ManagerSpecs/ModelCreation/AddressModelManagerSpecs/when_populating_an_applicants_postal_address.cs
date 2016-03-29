using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AddressModelManagerSpecs
{
    public class when_populating_an_applicants_postal_address : WithCoreFakes
    {
        private static AddressModelManager modelManager;
        private static Applicant applicant;
        private static AddressModel applicantPostalAddress;
        private static ResidentialAddressModel propertyAddress;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AddressModelManager(validationUtils);
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.CoApplicant, MaritalStatus.Married_CommunityofProperty,
                false, false, false, false, true);
            propertyAddress = new ResidentialAddressModel("2", "Oakland Avenue", string.Empty, string.Empty, string.Empty,
                "Hillcrest", "Hillcrest", "Kwazulu-Natal", "3610", "South Africa", true);
        };

        private Because of = () =>
        {
            applicantPostalAddress = modelManager.PopulateAddresses(applicant, propertyAddress, null)
                .Where(x => x.AddressType == AddressType.Postal).First();
        };

        private It should_be_in_free_text_format = () =>
        {
            applicantPostalAddress.ShouldBeOfExactType<FreeTextAddressModel>();
        };

        private It should_have_the_type_set_to_free_text = () =>
        {
            applicantPostalAddress.AddressFormat.ShouldEqual(AddressFormat.FreeText);
        };

        private It should_map_free_text_line_1_to_PostalAddressLine1 = () =>
        {
            applicantPostalAddress.FreeText1.ShouldEqual(applicant.PostalAddressLine1);
        };

        private It should_map_free_text_line_2_to_PostalAddressLine2 = () =>
        {
            applicantPostalAddress.FreeText2.ShouldEqual(applicant.PostalAddressLine2);
        };

        private It should_combine_suburb_and_postal_code_into_free_text_line_3 = () =>
        {
            applicantPostalAddress.FreeText3.ShouldEqual(string.Format("{0} {1}", applicant.PostalAddressSuburb, applicant.PostalAddressCode));
        };

        private It should_map_free_text_line_4_to_PostalAddressCity = () =>
        {
            applicantPostalAddress.FreeText4.ShouldEqual(applicant.PostalAddressCity);
        };

        private It should_map_free_text_line_5_to_PostalProvince = () =>
        {
            applicantPostalAddress.FreeText5.ShouldEqual(validationUtils.MapComcorpToSAHLProvince(applicant.PostalProvince));
        };

        private It should_map_free_text_country_to_PostalCountry = () =>
        {
            applicantPostalAddress.Country.ShouldEqual(applicant.PostalCountry);
        };
    }
}