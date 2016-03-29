namespace Automation.DataModels
{
    public class OfferMailingAddress
    {
        public int AddressKey { get; set; }

        public int LegalEntityKey { get; set; }

        public bool OnlineStatement { get; set; }

        public string Language { get; set; }

        public string CorrespondenceMedium { get; set; }

        public string OnlineStatementFormat { get; set; }

        public string FormattedAddress { get; set; }

        public string FormattedAddressDelimited { get; set; }
    }
}