using System;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.AddressDomain.Events
{
    public class PostalAddressLinkedToClientEvent : Event
    {
        public int ClientKey { get; protected set; }

        public int ClientAddressKey { get; protected set; }

        public string BoxNumber { get; protected set; }

        public string PostNetSuiteNumber { get; protected set; }

        public string City { get; protected set; }

        public string Province { get; protected set; }

        public string PostalCode { get; protected set; }

        public string PostOffice { get; protected set; }

        public AddressFormat AddressFormat { get; protected set; }

        public PostalAddressLinkedToClientEvent(DateTime date, string boxNumber, string postNetSuiteNumber, string postOffice, string city, 
            string province, string postalCode, AddressFormat addressFormat, int clientKey, int clientAddressKey)
            : base(date)
        {
            this.ClientAddressKey = clientAddressKey;
            this.BoxNumber = boxNumber;
            this.PostNetSuiteNumber = postNetSuiteNumber;
            this.PostOffice = postalCode;
            this.City = city;
            this.Province = province;
            this.PostalCode = postalCode;
            this.AddressFormat = addressFormat;

            this.ClientKey = clientKey;
        }
    }
}