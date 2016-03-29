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
    public class when_populating_values_that_map_directly_to_the_comcorp_applicant : WithModelManagerFakes
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

        private It should_populate_the_applicant_first_name_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.FirstName.ShouldEqual(comcorpApplicant.FirstName);
        };

        private It should_populate_the_applicant_surname_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.Surname.ShouldEqual(comcorpApplicant.Surname);
        };

        private It should_populate_the_applicant_idNumber_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.IDNumber.ShouldEqual(comcorpApplicant.IdentificationNo);
        };

        private It should_populate_the_applicant_passport_number_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.PassportNumber.ShouldEqual(comcorpApplicant.PassportNo);
        };

        private It should_populate_the_applicant_preferred_name_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.PreferredName.ShouldEqual(comcorpApplicant.PreferredName);
        };

        private It should_populate_the_applicant_date_of_birth_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.DateOfBirth.ShouldEqual(comcorpApplicant.DateOfBirth);
        };

        private It should_populate_the_applicant_home_phone_number_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.HomePhone.ShouldEqual(comcorpApplicant.HomePhone);
        };

        private It should_populate_the_applicant_home_phone_code_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.HomePhoneCode.ShouldEqual(comcorpApplicant.HomePhoneCode);
        };

        private It should_populate_the_applicant_work_phone_number_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.WorkPhone.ShouldEqual(comcorpApplicant.WorkPhone);
        };

        private It should_populate_the_applicant_work_phone_code_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.WorkPhoneCode.ShouldEqual(comcorpApplicant.WorkPhoneCode);
        };

        private It should_populate_the_applicant_cellphone_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.CellPhone.ShouldEqual(comcorpApplicant.Cellphone);
        };

        private It should_populate_the_applicant_email_address_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.EmailAddress.ShouldEqual(comcorpApplicant.EmailAddress);
        };

        private It should_populate_the_applicant_fax_number_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.FaxNumber.ShouldEqual(comcorpApplicant.FaxNo);
        };

        private It should_populate_the_applicant_fax_code_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.FaxCode.ShouldEqual(comcorpApplicant.FaxCode);
        };

        private It should_populate_the_income_contributor_indicator_using_the_value_provided_by_comcorp = () =>
        {
            applicantModel.IncomeContributor.ShouldEqual(comcorpApplicant.IncomeContributor);
        };
    }
}