namespace SAHL.Services.Interfaces.Search.Models
{
    /// <summary>
    /// Contains the properties the search result cares to display
    /// </summary>
    public class SearchForClientQueryResult
    {
        public int LegalEntityKey { get; set; }

        public string LegalEntityType { get; set; }

        public string LegalEntityStatus { get; set; }

        public string LegalName { get; set; }

        public string PreferredName { get; set; }

        public string LegalIdentity { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}