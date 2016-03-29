using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Server.Specs.Factory;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.Helpers
{
    public class lookup_types_find_valid_lookup : WithFakes
    {

        private static ISupportedLookupDataManager supportedLookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;


        Establish that = () =>
        {
            supportedLookupDataManager = An<ISupportedLookupDataManager>();
            var supportedLookupModels = LookupMetaDataFactory.CreateSupportedLookupModels();
            supportedLookupDataManager.WhenToldTo(x => x.GetSupportedLookups()).Return(supportedLookupModels);
            lookupTypesHelper = new LookupTypesHelper(supportedLookupDataManager);
        };

        private Because of = () =>
        {
            lookupTypesHelper.LoadValidLookupTypes();
        };

        private It should_load_valid_lookup_types = () =>
        {
            lookupTypesHelper.ValidLookupTypes.Count().Equals(1);
        };

        private It should_have_lookup_with_no_metadata = () =>
        {
            var lookup = lookupTypesHelper.ValidLookupTypes["generickeytype"];
            lookup.MetaData.ShouldBeNull();
        };

    }

}
