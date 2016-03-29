using Machine.Specifications;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_creating_an_applicant_itc_request_details_model : with_applicant_itc_service
    {
        private static Applicant applicant;
        private static ApplicantITCRequestDetailsModel result;

        private Establish context = () =>
        {
            var applicantStub = new ApplicantStubs();
            applicant = applicantStub.GetApplicant;
        };

        private Because of = () =>
        {
            result = applicantITCService.CreateApplicantITCRequest(applicant);
        };

        private It should_return_the_details_created_from_the_applicant = () =>
        {
            result.ShouldMatch(m =>
                m.AddressLine1 == applicant.ResidentialAddress.BuildingNumber + " " + applicant.ResidentialAddress.BuildingName &&
                m.AddressLine2 == applicant.ResidentialAddress.StreetNumber + " " + applicant.ResidentialAddress.StreetName &&
                m.FirstName == applicant.Information.FirstName &&
                m.Surname == applicant.Information.Surname &&
                m.IdentityNumber == applicant.Information.IdentityNumber &&
                m.DateOfBirth == applicant.Information.DateOfBirth
            );
        };
    }
}