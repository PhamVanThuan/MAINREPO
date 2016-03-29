using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;

using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.Application.Statements;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_getting_applicants_for_an_application : WithFakes
    {
        private static ApplicationDataManager service;
        private static List<ApplicantModel> applicants;
        private static Guid applicationID;
        private static ApplicantModel applicant;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            applicant = new ApplicantModel();
            applicationID = Guid.NewGuid();
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select(Param.IsAny<GetApplicantsForApplicationQuery>())).Return(new List<ApplicantModel> { applicant });
        };

        private Because of = () =>
        {
            applicants = service.GetApplicantsForApplication(applicationID);
        };

        private It should_return_the_applicants = () =>
        {
            applicants.ShouldContain(applicant);
        };
    }
}