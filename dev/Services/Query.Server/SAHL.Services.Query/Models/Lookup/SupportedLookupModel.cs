using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Query.Models.Lookup
{
    public class SupportedLookupModel : ISupportedLookupModel 
    {
        public string LookupKey { get; set; }    
        public string LookupTable { get; set; }    
    }
}