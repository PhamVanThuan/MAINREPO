using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_performing_itcs_and_an_applicant_is_not_contributor : with_application_service
    {
        private static Dictionary<Guid, Applicant> applicants;
        private static ItcProfile firstApplicantITCProfile;
        private static Guid firstApplicantID, firstApplicantITCID;
        private static Applicant firstApplicant;
        private static Guid secondApplicantID;
        private static Applicant secondApplicant;
        private static bool result;

        private Establish context = () =>
            {
                firstApplicantID = Guid.Parse("{B5D0814D-F1A0-4E3B-AC96-26C4F8B40D0F}");
                var firstApplicantStub = new ApplicantStubs();
                firstApplicantStub.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.YES);
                firstApplicant = firstApplicantStub.GetApplicant;

                secondApplicantID = Guid.Parse("{0F369E94-242D-4548-831F-958BA86993D5}");
                var secondApplicantStub = new ApplicantStubs();
                secondApplicantStub.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.NO);
                secondApplicant = secondApplicantStub.GetApplicant;

                applicants = new Dictionary<Guid, Applicant>();
                applicants.Add(firstApplicantID, firstApplicant);
                applicants.Add(secondApplicantID, secondApplicant);

                firstApplicantITCModel = new ITCDataModel(firstApplicantITCID, new DateTime(2015, 01, 8), "<Response></Response>");
                firstApplicantITCProfile = new ItcProfile( 10, null, null, null, null, null, false);
                applicantITCService.WhenToldTo(x => x.GetValidITCModelForPerson(firstApplicant.Information.IdentityNumber))
                                   .Return(firstApplicantITCModel);
                applicantITCService.WhenToldTo(x => x.GetITCProfile(firstApplicantITCID)).Return(firstApplicantITCProfile);
                applicantITCService.WhenToldTo(x => x.DoesITCPassQualificationRules(firstApplicantITCProfile, firstApplicantID))
                                   .Return(true);
            };

        private Because of = () =>
            {
                result = applicationService.PerformITCs(applicants);
            };

        private It should_get_an_itc_profile_for_the_first_applicant = () =>
            {
                serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<PerformITCForApplicantCommand>.Matches(m =>
                   m.Applicant == applicants.First().Value &&
                   m.ApplicantID == firstApplicantID), Param.IsAny<IServiceRequestMetadata>()));
            };

        private It should_get_the_latest_itc_for_the_first_applicant = () =>
        {
            applicantITCService.WasToldTo(x => x.GetValidITCModelForPerson(firstApplicant.Information.IdentityNumber));
        };

        private It should_get_the_itc_profile_for_the_first_applicant = () =>
        {
            applicantITCService.WasToldTo(x => x.GetITCProfile(firstApplicantITCID));
        };

        private It should_check_if_the_first_applicants_itc_qualifies = () =>
            {
                applicantITCService.WasToldTo(x => x.DoesITCPassQualificationRules(firstApplicantITCProfile, firstApplicantID));
            };

        private It should_not_get_an_itc_for_the_second_applicant = () =>
            {
                serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param<PerformITCForApplicantCommand>.Matches(m =>
                   m.Applicant == applicants.First().Value &&
                   m.ApplicantID == secondApplicantID), Param.IsAny<IServiceRequestMetadata>()));
            };

        private It should_not_check_if_the_second_applicants_itc_qualifies = () =>
            {
                applicantITCService.WasNotToldTo(x => x.DoesITCPassQualificationRules(Param.IsAny<ItcProfile>(), secondApplicantID));
            };

        private It should_return_that_the_itc_passed = () =>
            {
                result.ShouldBeTrue();
            };
        private static ITCDataModel firstApplicantITCModel;
    }
}