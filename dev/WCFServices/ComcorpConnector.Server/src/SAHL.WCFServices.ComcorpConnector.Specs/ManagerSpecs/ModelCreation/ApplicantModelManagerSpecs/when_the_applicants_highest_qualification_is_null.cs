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
    public class when_the_applicants_highest_qualification_is_null : WithModelManagerFakes
    {
        private static ApplicantModelManager applicantModelManager;
        private static Applicant comcorpApplicant;
        private static ApplicantModel applicantModel;
        private static ResidentialAddressModel propertyAddress;

        private Establish context = () =>
        {
            comcorpApplicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.CoApplicant
                , MaritalStatus.Married_CommunityofProperty
                , true
                , false
                , false
                , false
                , true);
            comcorpApplicant.SAHLHighestQualification = null;
            propertyAddress = new ResidentialAddressModel("45", "Chelsea Drive", string.Empty, string.Empty, string.Empty, "Durban North", "Durban", "Kwazulu-Natal", "4051",
                "South Africa", false);
            applicantModelManager = new ApplicantModelManager(_validationUtils, assetsManager, affordabilityManager, declarationsManager, addressManager, bankAccountManager,
                employmentManager, marketingOptionsManager);
        };

        private Because of = () =>
        {
            applicantModel = applicantModelManager.PopulateApplicantDetails(comcorpApplicant, propertyAddress);
        };

        private It should_set_the_applicants_education_level_to_unknown = () =>
        {
            applicantModel.Education.ShouldEqual(Education.Unknown);
        };
    }
}