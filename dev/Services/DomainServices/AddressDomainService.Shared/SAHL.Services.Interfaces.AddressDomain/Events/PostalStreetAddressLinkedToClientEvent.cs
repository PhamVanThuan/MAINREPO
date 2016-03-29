using System;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.Interfaces.AddressDomain.Events
{
    public class PostalStreetAddressLinkedToClientEvent : Event
    {
        public string UnitNumber { get; protected set; }

        public string BuildingNumber { get; protected set; }

        public string BuildingName { get; protected set; }

        public string StreetNumber { get; protected set; }

        public string StreetName { get; protected set; }

        public string Suburb { get; protected set; }

        public string City { get; protected set; }

        public string Province { get; protected set; }

        public string PostalCode { get; protected set; }

        public int ClientKey { get; protected set; }

        public PostalStreetAddressLinkedToClientEvent(DateTime date, string unitNumber, string buildingNumber, string buildingName, string streetNumber,
            string streetName, string suburb, string city, string province, string postalCode, int clientKey)
            : base(date)
        {
            this.ClientKey = clientKey;
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.Suburb = suburb;
            this.City = city;
            this.Province = province;
            this.PostalCode = postalCode;

            this.ClientKey = clientKey;
        }
    }
}