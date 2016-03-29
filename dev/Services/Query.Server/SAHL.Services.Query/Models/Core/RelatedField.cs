using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.Core
{
    public class RelatedField : IRelatedField
    {
        public string LocalKey { get; set; }
        public string RelatedKey { get; set; }
        public string Value { get; set; }
    }
}
