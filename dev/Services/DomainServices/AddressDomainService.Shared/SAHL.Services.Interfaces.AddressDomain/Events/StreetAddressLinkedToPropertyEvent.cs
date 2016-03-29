using System;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Events
{
    public class StreetAddressLinkedToPropertyEvent : Event
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

        public int PropertyKey { get; set; }

        public StreetAddressLinkedToPropertyEvent(DateTime date, string unitNumber, string buildingNumber, string buildingName, string streetNumber,
            string streetName, string suburb, string city, string province, string postalCode, int propertyKey)
            : base(date)
        {
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.Suburb = suburb;
            this.City = city;
            this.Province = province;
            this.PostalCode = postalCode;

            this.PropertyKey = propertyKey;
        }
    }
}