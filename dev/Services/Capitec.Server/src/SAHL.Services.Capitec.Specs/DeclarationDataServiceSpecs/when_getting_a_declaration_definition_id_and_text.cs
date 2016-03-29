using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Declaration;
using System;

namespace SAHL.Services.Capitec.Specs
{
    public class when_getting_a_declaration_definition_id_and_text : WithFakes
    {
        private static DeclarationDataManager service;
        private static FakeDbFactory dbFactory;
        private static System.Guid declarationDefinitionId;
        private static DeclarationDefinitionDataModel model;
        private static Guid id;
        private static Guid result;
        private static string declarationText;
        private static GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery query;

        Establish context = () =>
        {
            id = new Guid("5BE6C2E7-9EC3-44A8-A572-A2D500AB5A76");
            declarationDefinitionId = Guid.NewGuid();
            declarationText = "Has Capitec Transaction Account";
            query = new GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery(id, declarationText);
            model = new DeclarationDefinitionDataModel(declarationDefinitionId, id, declarationText);
            dbFactory = new FakeDbFactory();
            service = new DeclarationDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery>())).Return(model);
        };

        Because of = () =>
        {
            result = service.GetDeclarationDefinition(id, declarationText);
        };

        It should_return_declaration_definition_id = () =>
        {
            result.ShouldEqual(declarationDefinitionId);
        };
    }
}