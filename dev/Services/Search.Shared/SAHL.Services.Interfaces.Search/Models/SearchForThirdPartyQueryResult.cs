namespace SAHL.Services.Interfaces.Search.Models
{
    public class SearchForThirdPartyQueryResult
    {
        public int LegalEntityKey { get; set; }

        public string LegalEntityType { get; set; }

        public string ThirdPartyType { get; set; }

        public string ThirdPartySubType { get; set; }

        public string LegalName { get; set; }

        public string TradingName { get; set; }

        public string LegalIdentity { get; set; }

        public string Contact { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}