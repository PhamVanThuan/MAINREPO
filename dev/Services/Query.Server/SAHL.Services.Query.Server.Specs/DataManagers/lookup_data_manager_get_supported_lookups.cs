using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
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
    public class lookup_data_manager_get_supported_lookups : WithFakes
    {

        private static FakeDbFactory dbFactory;
        private static ISupportedLookupDataManager supportedLookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;

        Establish that = () =>
        {
            dbFactory = new FakeDbFactory();
            supportedLookupDataManager = new SupportedLookupDataManager(dbFactory);
        };

        private Because of = () =>
        {
            supportedLookupDataManager.GetSupportedLookups();
        };

        private It should_send_a_call_to_the_database = () =>
        {
            dbFactory.NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(x => x.Select<SupportedLookupModel>(Arg.Any<ISqlStatement<SupportedLookupModel>>()));
        };

    }
}
