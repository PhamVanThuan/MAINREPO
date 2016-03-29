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

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_performing_itc_for_two_applicantcs_and_first_fails : with_application_service
    {
        private static Dictionary<Guid, Applicant> applicants;
        private static Applicant firstApplicant;
        private static Applicant secondApplicant;
        private static ItcProfile firstApplicantITCProfile;
        private static ItcProfile secondApplicantITCProfile;
        private static ITCDataModel firstApplicantITCModel;
        private static ITCDataModel secondApplicantITCModel;
        private static Guid firstApplicantID, firstApplicantITCID;
        private static Guid secondApplicantID, secondApplicantITCID;
        private static bool result;

        private Establish context = () =>
        {
            var applicantStubs = new ApplicantStubs();
            applicantStubs.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.YES);

            firstApplicantID = Guid.Parse("{4736CD92-B726-4103-8F1D-D2F90B421265}");
            firstApplicantITCID = Guid.Parse("{F0EEBA4B-2D7C-4D52-B71F-D1D85648E3D2}");
            firstApplicant = applicantStubs.GetApplicant;

            secondApplicantID = Guid.Parse("{1A300C2A-C670-4EA3-BD4D-476A8BDC0CC5}");
            secondApplicantITCID = Guid.Parse("{63578774-6309-4337-A123-EBE1FDCC1AF5}");
            secondApplicant = applicantStubs.GetSecondApplicant;

            applicants = new Dictionary<Guid, Applicant>();
            applicants.Add(firstApplicantID, firstApplicant);
            applicants.Add(secondApplicantID, secondApplicant);

            firstApplicantITCModel = new ITCDataModel(firstApplicantITCID, new DateTime(2015, 01, 8), "<Response></Response>");
            firstApplicantITCProfile = new ItcProfile( 10, null, null, null, null, null, false);
            applicantITCService.WhenToldTo(x => x.GetValidITCModelForPerson(firstApplicant.Information.IdentityNumber))
                               .Return(firstApplicantITCModel);
            applicantITCService.WhenToldTo(x => x.GetITCProfile(firstApplicantITCID)).Return(firstApplicantITCProfile);
            applicantITCService.WhenToldTo(x => x.DoesITCPassQualificationRules(firstApplicantITCProfile, firstApplicantID))
                               .Return(false);

            secondApplicantITCModel = new ITCDataModel(secondApplicantITCID, new DateTime(2015, 01, 8), "<Response></Response>");
            secondApplicantITCProfile = new ItcProfile( 200, null, null, null, null, null, false);
            applicantITCService.WhenToldTo(x => x.GetValidITCModelForPerson(secondApplicant.Information.IdentityNumber))
                               .Return(secondApplicantITCModel);
            applicantITCService.WhenToldTo(x => x.GetITCProfile(secondApplicantITCID)).Return(secondApplicantITCProfile);
            applicantITCService.WhenToldTo(x => x.DoesITCPassQualificationRules(secondApplicantITCProfile, secondApplicantID))
                               .Return(true);
        };

        private Because of = () =>
        {
            result = applicationService.PerformITCs(applicants);
        };

        private It should_perform_an_itc_for_the_first_applicant = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<PerformITCForApplicantCommand>.Matches(m =>
                m.Applicant == firstApplicant &&
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

        private It should_check_if_the_first_applicant_itc_qualifies = () =>
        {
            applicantITCService.WasToldTo(x => x.DoesITCPassQualificationRules(firstApplicantITCProfile, firstApplicantID));
        };

        private It should_still_perform_an_itc_for_the_second_applicant = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<PerformITCForApplicantCommand>.Matches(m =>
                m.Applicant == secondApplicant &&
                m.ApplicantID == secondApplicantID), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_get_the_latest_itc_for_the_second_applicant = () =>
        {
            applicantITCService.WasToldTo(x => x.GetValidITCModelForPerson(secondApplicant.Information.IdentityNumber));
        };

        private It should_get_the_itc_profile_for_the_second_applicant = () =>
        {
            applicantITCService.WasToldTo(x => x.GetITCProfile(secondApplicantITCID));
        };

        private It should_check_if_the_second_applicant_itc_qualifies = () =>
        {
            applicantITCService.WasToldTo(x => x.DoesITCPassQualificationRules(secondApplicantITCProfile, secondApplicantID));
        };

        private It should_return_that_the_itc_failed = () =>
        {
            result.ShouldBeFalse();
        };
    }
}