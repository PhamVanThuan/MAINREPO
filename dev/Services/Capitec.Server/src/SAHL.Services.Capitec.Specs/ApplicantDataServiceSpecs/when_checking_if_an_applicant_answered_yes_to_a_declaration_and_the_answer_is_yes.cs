using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Interfaces.Capitec.Common;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_checking_if_an_applicant_answered_yes_to_a_declaration_and_the_answer_is_yes : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid applicantId;
        private static string declarationText;
        private static DeclarationTypeEnumDataModel declarationType;
        private static bool result;

        Establish context = () =>
        {
            declarationType = new DeclarationTypeEnumDataModel(Enumerations.DeclarationTypeEnums.Yes.ToString(), true);
            applicantId = Guid.NewGuid();
            declarationText = "declaration text";
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetDeclarationTypeEnumForApplicantDeclarationByDeclarationText>())).Return(declarationType);
        };

        Because of = () =>
        {
            result = service.DidApplicantAnswerYesToDeclaration(applicantId, declarationText);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}