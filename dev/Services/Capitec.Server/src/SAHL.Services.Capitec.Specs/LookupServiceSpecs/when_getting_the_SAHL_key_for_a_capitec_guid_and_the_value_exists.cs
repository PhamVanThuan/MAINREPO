using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.CapitecApplication.Models;
using SAHL.Services.Capitec.Managers.CapitecApplication.Statements;
using SAHL.Services.Capitec.Managers.Lookup;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs
{
    public class when_getting_the_SAHL_key_for_a_capitec_guid_and_the_value_exists : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static Guid capitecId;
        private static List<CapitecGuidToSAHL_KeyMappingModel> list;
        private static CapitecGuidToSAHL_KeyMappingModel mapping;
        private static int sahlKey;
        private static int result;

        private Establish context = () =>
            {
                capitecId = Guid.NewGuid();
                sahlKey = 5;
                mapping = new CapitecGuidToSAHL_KeyMappingModel(capitecId, sahlKey);
                list = new List<CapitecGuidToSAHL_KeyMappingModel>() { mapping };
                dbFactory = new FakeDbFactory();
                dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select(Param.IsAny<GetCapitecGuidToSAHL_KeyMappingQuery>())).Return(list);
                lookupService = new LookupManager(dbFactory);
            };

        private Because of = () =>
            {
                result = lookupService.GetSahlKeyByCapitecGuid(capitecId);
            };

        private It should_return_the_mapped_SAHL_Key_for_the_guid = () =>
            {
                result.ShouldEqual(sahlKey);
            };
    }
}