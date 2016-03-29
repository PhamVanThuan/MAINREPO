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
    public class when_getting_a_declaration_for_an_applicant_and_the_declaration_does_not_exist : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Exception exception;
        private static Guid applicantId;
        private static string declarationText;
        private static ApplicantDeclarationDataModel model;

        Establish context = () =>
        {
            model = null;
            applicantId = Guid.NewGuid();
            declarationText = "Declaration Text";
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetApplicantDeclarationByDeclarationText>())).Return(model);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                service.GetDeclarationForApplicant(applicantId, declarationText);
            });
        };

        It should_throw_a_null_reference_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(NullReferenceException));
        };

        It should_throw_a_custom_exception_message = () =>
        {
            exception.Message.ShouldEqual(string.Format("Declaration {0} does not exist for applicant {1}", declarationText, applicantId));
        };
    }
}