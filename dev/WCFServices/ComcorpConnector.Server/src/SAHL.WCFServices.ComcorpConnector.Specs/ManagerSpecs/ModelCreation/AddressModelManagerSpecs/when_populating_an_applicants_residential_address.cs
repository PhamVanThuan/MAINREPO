using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AddressModelManagerSpecs
{
    public class when_populating_an_applicants_residential_address : WithCoreFakes
    {
        private static AddressModelManager modelManager;
        private static Applicant applicant;
        private static AddressModel applicantResidentialAddress;
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
            applicantResidentialAddress = modelManager.PopulateAddresses(applicant, propertyAddress, null)
                .Where(x => x.AddressType == AddressType.Residential && x.AddressFormat == AddressFormat.FreeText)
                .FirstOrDefault();
        };

        private It should_be_in_free_text_format = () =>
        {
            applicantResidentialAddress.ShouldBeOfExactType<FreeTextAddressModel>();
        };

        private It should_have_the_type_set_to_free_text = () =>
        {
            applicantResidentialAddress.AddressFormat.ShouldEqual(AddressFormat.FreeText);
        };

        private It should_map_free_text_line_1_to_PhysicalAddressLine1 = () =>
        {
            applicantResidentialAddress.FreeText1.ShouldEqual(applicant.PhysicalAddressLine1);
        };

        private It should_map_free_text_line_2_to_PhysicalAddressLine2 = () =>
        {
            applicantResidentialAddress.FreeText2.ShouldEqual(applicant.PhysicalAddressLine2);
        };

        private It should_combine_suburb_and_postal_code_into_free_text_line_3 = () =>
        {
            applicantResidentialAddress.FreeText3.ShouldEqual(string.Format("{0} {1}", applicant.PhysicalAddressSuburb, applicant.PhysicalAddressCode));
        };

        private It should_map_free_text_line_4_to_PostalAddressCity = () =>
        {
            applicantResidentialAddress.FreeText4.ShouldEqual(applicant.PhysicalAddressCity);
        };

        private It should_map_free_text_line_5_to_PostalProvince = () =>
        {
            applicantResidentialAddress.FreeText5.ShouldEqual(validationUtils.MapComcorpToSAHLProvince(applicant.PhysicalProvince));
        };

        private It should_map_free_text_country_to_PostalCountry = () =>
        {
            applicantResidentialAddress.Country.ShouldEqual(applicant.PhysicalCountry);
        };
    }
}