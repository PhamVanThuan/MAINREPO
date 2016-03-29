using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Lookup;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Tests.DataManagers
{
    public class lookup_data_manager_get_lookup_schema : WithFakes
    {

        private static FakeDbFactory dbFactory;
        private static SupportedLookupDataManager supportedLookupDataManager;
        private static string lookup = "SomeLookup";

        Establish that = () =>
        {
            dbFactory = new FakeDbFactory();
            supportedLookupDataManager = new SupportedLookupDataManager(dbFactory);
        };

        private Because of = () =>
        {
            supportedLookupDataManager.GetLookupSchema(lookup);
        };

        private It should_call_the_database = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(
                x => x.SelectOne<LookupMetaDataModel>(Arg.Any<ISqlStatement<LookupMetaDataModel>>()));
        };

        private It should_set_the_search_value_correctly = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<LookupMetaDataModel>(Param<GetSchemaStatement>.Matches(m => m.LookupTableName == lookup)));
        };


    }
}
