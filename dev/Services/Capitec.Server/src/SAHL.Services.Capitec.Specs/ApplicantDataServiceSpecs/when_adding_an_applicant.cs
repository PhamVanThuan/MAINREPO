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
    public class when_adding_an_applicant : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid applicantId, personId;
        private static bool isMainContact;

        Establish context = () =>
        {
            applicantId = Guid.NewGuid();
            personId = Guid.NewGuid();
            isMainContact = true;
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
        };

        Because of = () =>
        {
            service.AddApplicant(applicantId, personId, isMainContact);
        };

        It should_add_an_applicant_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicantDataModel>(y => y.Id == applicantId && y.PersonID == personId && y.MainContact == isMainContact)));
        };
    }
}