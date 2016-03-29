using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Query.Models.Lookup
{
    public class LookupMetaDataModel : ILookupMetaDataModel
    {
        public string LookupType { get; set; }
        public string Db { get; set; }
        public string Schema { get; set; }
        public string LookupTable { get; set; }
        public string LookupKey { get; set; }
        public string LookupDescription { get; set; }
    }
}