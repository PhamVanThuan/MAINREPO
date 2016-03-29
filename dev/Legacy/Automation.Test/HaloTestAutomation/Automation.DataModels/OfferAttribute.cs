namespace Automation.DataModels
{
    public sealed class OfferAttribute
    {
        public int OfferAttributeKey { get; set; }

        public int OfferKey { get; set; }

        public Common.Enums.OfferAttributeTypeEnum OfferAttributeTypeKey { get; set; }
    }
}