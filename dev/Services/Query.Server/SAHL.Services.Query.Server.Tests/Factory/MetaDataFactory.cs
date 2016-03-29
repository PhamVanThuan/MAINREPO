using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.Server.Tests.Factory
{
    public static class MetaDataFactory
    {

        public static ILookupMetaDataModel GetGenericKeyMetaDataModel()
        {
            return new LookupMetaDataModel()
            {
                LookupType = "GenericKeyType",
                Db = "2AM",
                LookupDescription = "Description",
                LookupKey = "GenericKeyTypeKey",
                LookupTable = "GenericKeyType",
                Schema = "dbo"
            };
  
        }

        public static ISupportedLookup GetGenericKeySupportedLookup()
        {
            var supportedLookup = new SupportedLookup() { Lookup = "GenericKeyType" };
            supportedLookup.MetaData = GetGenericKeyMetaDataModel();
            return supportedLookup;
        }

        public static ILookupMetaDataModel GetSomkeKeyTypeMetaDataModel()
        {
            return new LookupMetaDataModel()
            {
                LookupType = "",
                Db = "",
                LookupDescription = "",
                LookupKey = "",
                LookupTable = "",
                Schema = ""
            };
  
        }

        public static ISupportedLookup GetSomkeKeyTypeSupportedLookup()
        {
            var supportedLookup = new SupportedLookup() { Lookup = "SomkeKeyType" };
            supportedLookup.MetaData = GetSomkeKeyTypeMetaDataModel();
            return supportedLookup;
        }

         
    }
}