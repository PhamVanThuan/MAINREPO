﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Lookup;
using System;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs
{
    public class when_getting_the_declaration_result_for_yes : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static bool result;
        private static Guid declarationType;

        private Establish context = () =>
        {
            declarationType = Guid.Parse(DeclarationTypeEnumDataModel.YES);
            dbFactory = new FakeDbFactory();
            lookupService = new LookupManager(dbFactory);
            result = false;
        };

        private Because of = () =>
        {
            result = lookupService.GetDeclarationResultByCapitecGuid(declarationType);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}