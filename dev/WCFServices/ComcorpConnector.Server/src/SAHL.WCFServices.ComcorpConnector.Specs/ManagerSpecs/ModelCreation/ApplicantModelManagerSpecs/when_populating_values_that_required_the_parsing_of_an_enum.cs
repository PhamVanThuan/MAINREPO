using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicantModelManagerSpecs
{
    public class when_populating_values_that_required_the_parsing_of_an_enum : WithModelManagerFakes
    {
        private static ApplicantModelManager applicantModelManager;
        private static Applicant comcorpApplicant;
        private static ApplicantModel applicantModel;
        private static ResidentialAddressModel propertyAddress;

        private Establish context = () =>
        {
            comcorpApplicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant
                , MaritalStatus.Married_CommunityofProperty
                , false
                , false
                , false
                , false
                , true);
            comcorpApplicant.Title = "Mr";
            comcorpApplicant.EthnicGroup = "White";
            comcorpApplicant.SAHLSACitizenType = "SA Citizen";
            comcorpApplicant.HomeLanguage = "English";
            comcorpApplicant.CorrespondenceLanguage = "English";
            comcorpApplicant.SAHLHighestQualification = "Matric";
            propertyAddress = new ResidentialAddressModel("45", "Chelsea Drive", string.Empty, string.Empty, string.Empty, "Durban North", "Durban", "Kwazulu-Natal", "4051",
                "South Africa", false);
            applicantModelManager = new ApplicantModelManager(_validationUtils, assetsManager, affordabilityManager, declarationsManager, addressManager, bankAccountManager,
                employmentManager, marketingOptionsManager);
        };

        private Because of = () =>
        {
            applicantModel = applicantModelManager.PopulateApplicantDetails(comcorpApplicant, propertyAddress);
        };

        private It should_set_the_applicants_salutation_to_the_corresponding_enum = () =>
        {
            applicantModel.Salutation.ShouldEqual(SalutationType.Mr);
        };

        private It should_set_the_applicants_gender_to_the_corresponding_enum = () =>
        {
            applicantModel.Gender.ShouldEqual(Gender.Male);
        };

        private It should_set_the_applicants_marital_status_to_the_corresponding_enum = () =>
        {
            applicantModel.MaritalStatus.ShouldEqual(MaritalStatus.Married_CommunityofProperty);
        };

        private It should_set_the_applicants_population_group_to_the_corresponding_enum = () =>
        {
            applicantModel.PopulationGroup.ShouldEqual(PopulationGroup.White);
        };

        private It should_set_the_applicants_citizenship_type_to_the_corresponding_enum = () =>
        {
            applicantModel.CitizenshipType.ShouldEqual(CitizenType.SACitizen);
        };

        private It should_set_the_applicants_home_language_to_the_corresponding_enum = () =>
        {
            applicantModel.HomeLanguage.ShouldEqual(Language.English);
        };

        private It should_set_the_applicants_correspondence_language_to_the_corresponding_enum = () =>
        {
            applicantModel.CorrespondenceLanguage.ShouldEqual(CorrespondenceLanguage.English);
        };

        private It should_set_the_applicants_highest_education_level_to_the_corresponding_enum = () =>
        {
            applicantModel.Education.ShouldEqual(Education.Matric);
        };
    }
}