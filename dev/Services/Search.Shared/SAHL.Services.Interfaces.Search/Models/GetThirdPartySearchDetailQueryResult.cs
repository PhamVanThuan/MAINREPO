namespace SAHL.Services.Interfaces.Search.Models
{
    public class GetThirdPartySearchDetailQueryResult
    {
        public GetThirdPartySearchDetailQueryResult(string type, string addressType, string address, bool isActive, string contact, bool isLitigationAttorney, 
                                                    bool isRegistrationAttorney, string deedsOffice)
        {
            this.Type = type;
            this.Address = address;
            this.AddressType = addressType;
            this.IsActive = isActive;
            this.Contact = contact;
            this.IsLitigationAttorney = isLitigationAttorney;
            this.IsRegistrationAttorney = isRegistrationAttorney;
            this.DeedsOffice = deedsOffice;
        }

        public string Type { get; protected set; }

        public string AddressType { get; protected set; }

        public string Address { get; protected set; }

        public bool IsActive { get; protected set; }

        public string Contact { get; protected set; }

        public string DeedsOffice { get; protected set; }

        public bool IsLitigationAttorney { get; protected set; }

        public bool IsRegistrationAttorney { get; protected set; }
    }
}