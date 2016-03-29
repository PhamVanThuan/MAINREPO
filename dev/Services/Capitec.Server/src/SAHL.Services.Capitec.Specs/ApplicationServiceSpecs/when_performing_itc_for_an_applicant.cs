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
    public class when_performing_itc_for_an_applicant : with_application_service
    {
        private static Dictionary<Guid, Applicant> applicants;
        private static ItcProfile itcProfile;
        private static Guid applicantID, itcID;
        private static string identityNumber;
        private static bool result;

        private Establish context = () =>
            {
                applicantID = Guid.Parse("{E7402393-E4DA-489D-9CA9-5DC1C42FBA35}");
                itcID = Guid.Parse("{AC85B30B-2541-4AB8-B812-885AEB1F2CF1}");
                applicants = new Dictionary<Guid, Applicant>();
                var applicant = new ApplicantStubs();
                applicant.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.YES);
                applicants.Add(applicantID, applicant.GetApplicant);
                identityNumber = applicants.First().Value.Information.IdentityNumber;
                itcProfile = new ItcProfile(399, null, null, null, null, null, true);
                var itcModel = new ITCDataModel(itcID, new DateTime(2015, 01, 08), "<Response></Response>");

                applicantITCService.WhenToldTo(x => x.GetValidITCModelForPerson(identityNumber)).Return(itcModel);
                applicantITCService.WhenToldTo(x => x.GetITCProfile(itcID)).Return(itcProfile);
                applicantITCService.WhenToldTo(x => x.DoesITCPassQualificationRules(itcProfile, applicantID)).Return(true);
            };

        private Because of = () =>
        {
            result = applicationService.PerformITCs(applicants);
        };

        private It should_get_an_itc_proile_for_the_applicant = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<PerformITCForApplicantCommand>.Matches(m=>
                m.Applicant == applicants.First().Value &&
                m.ApplicantID == applicantID), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_get_the_latest_valid_itc = () =>
        {
            applicantITCService.WasToldTo(x => x.GetValidITCModelForPerson(identityNumber));
        };

        private It should_use_the_latest_itc_model_to_get_the_itc_profile = () =>
        {
            applicantITCService.WasToldTo(x => x.GetITCProfile(itcID));
        };

        private It should_check_if_the_itc_passes_the_credit_criteria = () =>
        {
            applicantITCService.WasToldTo(x => x.DoesITCPassQualificationRules(itcProfile, applicantID));
        };

        private It should_return_whether_the_ITC_passed = () =>
        {
            result.ShouldBeTrue();
        };
    }
}