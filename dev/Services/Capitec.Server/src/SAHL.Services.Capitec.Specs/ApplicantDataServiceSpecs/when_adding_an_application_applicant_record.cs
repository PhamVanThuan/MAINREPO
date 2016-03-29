using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_adding_an_application_applicant_record : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid applicationId;
        private static Guid applicantId;
        private static Guid applicationApplicantId;

        Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            applicantId = Guid.NewGuid();
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
        };

        Because of = () =>
        {
            service.AddApplicantToApplication(applicationId, applicantId);
        };

        It should_create_an_applicationApplicant_model_with_the_provided_application_and_applicant = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicationApplicantDataModel>(y => y.ApplicantId == applicantId && y.ApplicationId == applicationId)));
        };
    }
}