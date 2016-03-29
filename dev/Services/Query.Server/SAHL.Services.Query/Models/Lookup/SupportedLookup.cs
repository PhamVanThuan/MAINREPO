using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Query.Models.Lookup
{
    public class SupportedLookup : ISupportedLookup
    {
        public string Lookup { get; set; }
        public ILookupMetaDataModel MetaData { get; set; }
    }
}