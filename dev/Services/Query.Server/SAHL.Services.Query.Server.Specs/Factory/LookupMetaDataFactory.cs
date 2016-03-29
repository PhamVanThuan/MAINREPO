using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Resources.Lookup;

namespace SAHL.Services.Query.Server.Specs.Factory
{
    public static class LookupMetaDataFactory
    {
        public static ILookupMetaDataModel GetGenericKeyMetaDataModel()
        {
            return new LookupMetaDataModel
            {
                LookupType = "GenericKeyType",
                Db = "2AM",
                LookupDescription = "Description",
                LookupKey = "GenericKeyTypeKey",
                LookupTable = "GenericKeyType",
                Schema = "dbo"
            };
        }

        public static ILookupMetaDataModel GetSomkeKeyTypeMetaDataModel()
        {
            return new LookupMetaDataModel
            {
                LookupType = "",
                Db = "",
                LookupDescription = "",
                LookupKey = "",
                LookupTable = "",
                Schema = ""
            };
        }

        public static IEnumerable<SupportedLookupModel> CreateSupportedLookupModels()
        {
            var supportedLookupModels = new List<SupportedLookupModel>();

            supportedLookupModels.Add(new SupportedLookupModel
            {
                LookupKey = "generickeytype",
                LookupTable = "GenericKeyType"
            });
            return supportedLookupModels;
        }

        public static LookupTypeListRepresentation GetLookupTypeListRepresentation()
        {
            var lookupTypeRepresentations = new List<LookupTypeRepresentation>();

            lookupTypeRepresentations.Add(new LookupTypeRepresentation
            {
                Id = "generickeytype",
                Description = "GenericKeyType"
            });

            return new LookupTypeListRepresentation(lookupTypeRepresentations);
        }
    }
}
