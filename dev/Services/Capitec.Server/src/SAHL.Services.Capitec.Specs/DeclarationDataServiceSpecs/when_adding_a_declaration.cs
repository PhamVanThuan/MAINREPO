using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Declaration;
using System;

namespace SAHL.Services.Capitec.Specs.DeclarationDataServiceSpecs
{
    public class when_adding_a_declaration : WithFakes
    {
        private static DeclarationDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid declarationId, declarationDefinitionId;
        private static DateTime declarationDate;
        private static DeclarationDataModel modelToInsert;

        Establish context = () =>
        {
            declarationId = Guid.NewGuid();
            declarationDefinitionId = Guid.NewGuid();
            declarationDate = DateTime.Now;
            dbFactory = new FakeDbFactory();
            service = new DeclarationDataManager(dbFactory);
            modelToInsert = new DeclarationDataModel(declarationId, declarationDefinitionId, declarationDate);
        };

        Because of = () =>
        {
            service.AddDeclaration(declarationId, declarationDefinitionId, declarationDate);
        };

        It should_insert_the_declaration_data_model_with_the_provided_model = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<DeclarationDataModel>(y=>y.DeclarationDefinitionId == declarationDefinitionId)));
        };
    }
}