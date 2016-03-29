using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetPropertyByAddressQuery : ServiceQuery<GetPropertyByAddressQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetPropertyByAddressQueryResult>
    {
        public int AddressFormatKey { get; protected set; }

        public string UnitNumber { get; protected set; }

        public string BuildingNumber { get; protected set; }

        public string BuildingName { get; protected set; }

        public string StreetNumber { get; protected set; }

        public string StreetName { get; protected set; }

        public string Suburb { get; protected set; }

        public string City { get; protected set; }

        public string Province { get; protected set; }

        public string PostalCode { get; protected set; }

        public string ErfNumber { get; protected set; }

        public string ErfPortionNumber { get; protected set; }

        public GetPropertyByAddressQuery(string unitNumber, string buildingName, string buildingNumber, string streetNumber, string streetName, string suburb, string city, string province, string postalCode, string erfNumber, string erfPortionNumber)
        {
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.Suburb = suburb;
            this.City = city;
            this.PostalCode = postalCode;
            this.Province = province;
            this.ErfNumber = erfNumber;
            this.ErfPortionNumber = erfPortionNumber;
        }
    }
}