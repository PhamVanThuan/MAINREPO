using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_getting_a_declaration_for_an_applicant : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid applicantId;
        private static string declarationText;
        private static ApplicantDeclarationDataModel model;
        private static Guid declarationId;
        private static Guid result;

        Establish context = () =>
        {
            declarationId = Guid.NewGuid();
            model = new ApplicantDeclarationDataModel(Guid.NewGuid(), applicantId, declarationId);
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetApplicantDeclarationByDeclarationText>())).Return(model);
        };

        Because of = () =>
        {
            result = service.GetDeclarationForApplicant(applicantId, declarationText);  
        };

        It should_return_the_applicationDeclaration_Id = () =>
        {
            result.ShouldEqual(model.ID);
        };
    }
}