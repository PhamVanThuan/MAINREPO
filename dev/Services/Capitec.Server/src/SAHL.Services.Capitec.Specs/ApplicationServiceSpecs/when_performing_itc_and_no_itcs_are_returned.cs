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
    public class when_performing_itc_and_no_itcs_are_returned : with_application_service
    {
        private static Dictionary<Guid, Applicant> applicants;
        private static Guid applicantID;
        private static bool result;

        private Establish context = () =>
            {
                applicantID = Guid.Parse("{E7402393-E4DA-489D-9CA9-5DC1C42FBA35}");
                applicants = new Dictionary<Guid, Applicant>();
                var applicant = new ApplicantStubs();
                applicant.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.YES);
                applicants.Add(applicantID, applicant.GetApplicant);
            };

        private Because of = () =>
        {
            result = applicationService.PerformITCs(applicants);
        };

        private It should_perform_an_applicant_itc = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<PerformITCForApplicantCommand>.Matches(m=>
                m.Applicant == applicants.First().Value &&
                m.ApplicantID == applicantID), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_try_and_get_the_latest_itc_for_the_person = () =>
        {
            applicantITCService.WasToldTo(x => x.GetValidITCModelForPerson(applicants.First().Value.Information.IdentityNumber));
        };

        private It should_not_get_the_itc_profile = () =>
        {
            applicantITCService.WasNotToldTo(x => x.GetITCProfile(Param.IsAny<Guid>()));
        };

        private It should_not_check_if_the_itc_passes_the_credit_criteria = () =>
        {
            applicantITCService.WasNotToldTo(x => x.DoesITCPassQualificationRules(Param.IsAny<ItcProfile>(), applicantID));
        };

        private It should_set_the_empirica_score_on_the_applicant = () =>
        {
            applicants.First().Value.EmpiricaScore.ShouldEqual(-999);
        };

        private It should_return_that_the_itc_passed = () =>
        {
            result.ShouldBeTrue();
        };
    }
}