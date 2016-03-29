using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PostalAddressStreetModel : PostalAddressModel
    {
        public PostalAddressStreetModel(string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, string suburb, 
                                        string city, string province, string postalCode, string country)
            : base(AddressFormat.Street, null, unitNumber, buildingNumber, buildingName, streetNumber, streetName, null, null, suburb, city, postalCode, province, country)
        {
        }
    }
}
