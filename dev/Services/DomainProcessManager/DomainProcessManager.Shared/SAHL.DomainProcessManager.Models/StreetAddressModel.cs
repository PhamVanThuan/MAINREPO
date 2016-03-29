using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class StreetAddressModel : AddressModel
    {
        public StreetAddressModel(string streetNumber, string streetName, string unitNumber, string buildingNumber, string buildingName, string suburb, 
                                       string city, string province, string postalCode, string country, bool isDomicilium)
            : base(AddressType.Residential, AddressFormat.Street, null, null, buildingNumber, buildingName, streetNumber, streetName, 
                   null, null, null, null, null, null, null, suburb, city, postalCode, province, country, isDomicilium)
        {
        }
    }
}