
using Common.Enums;

namespace Automation.DataModels
{
    public sealed class OfferAccountRelationship : IDataModel
    {
        public int OfferAccountRelationshipKey { get; set; }
        public int AccountKey { get; set; }
        public int OfferKey { get; set; }
    }
}

