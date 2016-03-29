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
    public class when_adding_a_declaration_to_an_applicant : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid applicantId, declarationId; 

        Establish context = () =>
        {
            applicantId = Guid.NewGuid();
            declarationId = Guid.NewGuid();
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
        };

        Because of = () =>
        {
            service.AddDeclarationToApplicant(applicantId, declarationId);
        };

        It should_insert_a_record_with_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicantDeclarationDataModel>(y => y.ApplicantId == applicantId && y.DeclarationId == declarationId)));
        };
    }
}