using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Declaration;
using System;

namespace SAHL.Services.Capitec.Specs.DeclarationDataServiceSpecs
{
    public class when_getting_a_declaration_definition_that_does_not_exist : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static DeclarationDefinitionDataModel model;
        private static Guid id;
        private static Guid result;
        private static string declarationText;
        private static GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery query;
        private static DeclarationDataManager service;
        private static System.Exception exception;

        Establish context = () =>
        {
            id = new Guid("5BE6C2E7-9EC3-44A8-A572-A2D500AB5A76");
            declarationText = "Has Capitec Transaction Account";
            model = null;
            query = new GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery(id, declarationText);
            dbFactory = new FakeDbFactory();
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery>())).Return(model);
            service = new DeclarationDataManager(dbFactory);
            
        };
        Because of = () =>
        {
            exception = Catch.Exception(() =>
                {
                    result = service.GetDeclarationDefinition(id, declarationText);
                });
        };

        It should_throw_a_null_reference_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(NullReferenceException));
        };

        It should_throw_a_custom_message = () =>
        {
            exception.Message.ShouldEqual("declaration definition does not exist");
        };
    }
}