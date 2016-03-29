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
    public class when_populating_a_main_applicant : WithModelManagerFakes
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
            propertyAddress = new ResidentialAddressModel("45", "Chelsea Drive", string.Empty, string.Empty, string.Empty, "Durban North", "Durban", "Kwazulu-Natal", "4051",
                "South Africa", false);
            applicantModelManager = new ApplicantModelManager(_validationUtils, assetsManager, affordabilityManager, declarationsManager, addressManager, bankAccountManager,
                employmentManager, marketingOptionsManager);
        };

        private Because of = () =>
        {
            applicantModel = applicantModelManager.PopulateApplicantDetails(comcorpApplicant, propertyAddress);
        };

        private It should_set_role_type_to_main_applicant = () =>
        {
            applicantModel.ApplicantRoleType.ShouldEqual(LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
        };
        //ClintonS: this has to be done until the DomainProcessManager has been updated to not use an IDNumber to track the applicants.
        private It should_set_the_passport_number_to_null = () =>
        {
            applicantModel.PassportNumber.ShouldBeNull();
        };
    }
}