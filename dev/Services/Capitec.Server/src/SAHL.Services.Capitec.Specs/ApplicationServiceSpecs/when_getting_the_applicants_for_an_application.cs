using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Application.Models;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_getting_the_applicants_for_an_application : with_application_service
    {
        private static List<ApplicantModel> applicants;
        private static Guid applicationId;
        private static ApplicantModel applicantOne;
        private static ApplicantModel applicantTwo;
        private static List<ApplicantModel> applicantsResult;
        Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            applicantOne = new ApplicantModel() { ID = Guid.NewGuid(), Name = "Clint Speed" };
            applicantTwo = new ApplicantModel() { ID = Guid.NewGuid(), Name = "Tristan Zwart" };
            applicants = new List<ApplicantModel>() { applicantOne, applicantTwo };

            applicationDataService.WhenToldTo(x => x.GetApplicantsForApplication(applicationId)).Return(applicants);
        };

        Because of = () =>
        {
            applicantsResult = applicationService.GetApplicantsForApplication(applicationId);
        };

        It should_return_the_applicants_from_the_application_data_service = () =>
        {
            applicantsResult.ShouldEqual(applicants);
        };

        It should_contain_the_correct_applicant_details_for_the_first_applicant = () =>
        {
            applicantsResult.First().Name.ShouldEqual(applicants.First().Name);
            applicantsResult.First().ID.ShouldEqual(applicants.First().ID);
        };

        It should_contain_the_correct_applicant_details_for_the_second_applicant = () =>
        {
            applicantsResult.Last().Name.ShouldEqual(applicants.Last().Name);
            applicantsResult.Last().ID.ShouldEqual(applicants.Last().ID);
        };
    }
}
