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
    public class when_checking_if_an_applicant_answered_yes_to_a_declaration_and_no_answer_exists : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid applicantId;
        private static string declarationText;
        private static DeclarationTypeEnumDataModel declarationType;
        private static bool result;
        private static System.Exception ex;

        private Establish context = () =>
        {
            declarationType = null;
            applicantId = Guid.NewGuid();
            declarationText = "declaration text";
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetDeclarationTypeEnumForApplicantDeclarationByDeclarationText>())).Return(declarationType);
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                service.DidApplicantAnswerYesToDeclaration(applicantId, declarationText);
            });
        };

        private It should_throw_a_null_reference_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(NullReferenceException));
        };

        private It should_return_custom_exception_message = () =>
        {
            ex.Message.ShouldEqual(string.Format("Declaration {0} does not exist for applicant {1}", declarationText, applicantId));
        };
    }
}
