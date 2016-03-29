using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Config.Services.Query.Server;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Server.Specs.Factory;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.Startable
{
    public class query_service_startup_when_starting_should_load_lookups : WithFakes
    {

        private static LookupTypeStartable lookupTypeStartable;
        private static ILookupTypesHelper lookupTypesHelper;
        private static IDbFactory dbFactory;
        private static ISupportedLookupDataManager supportedLookupDataManager;
        
        Establish that = () =>
        {
            supportedLookupDataManager = An<ISupportedLookupDataManager>();

            var supportedLookupModels = LookupMetaDataFactory.CreateSupportedLookupModels();
            supportedLookupDataManager.WhenToldTo(x => x.GetSupportedLookups()).Return(supportedLookupModels);

            lookupTypesHelper = new LookupTypesHelper(supportedLookupDataManager);
            lookupTypeStartable = new LookupTypeStartable(lookupTypesHelper);
        };
        
        private Because of = () =>
        {
            lookupTypeStartable.Start();
        };

        private It should_load_valid_lookup_types = () =>
        {
            supportedLookupDataManager.WasToldTo(x => x.GetSupportedLookups());
        };


    }
}
