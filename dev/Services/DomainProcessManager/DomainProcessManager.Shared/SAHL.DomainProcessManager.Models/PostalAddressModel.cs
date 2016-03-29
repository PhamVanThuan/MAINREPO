using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PostalAddressModel : AddressModel
    {
        public PostalAddressModel(AddressFormat addressFormat, string boxNumber, string unitNumber, string buildingNumber, string buildingName, 
                                  string streetNumber, string streetName, string suiteNumber, string postOffice, string suburb, string city, string postalCode, 
                                  string province, string country)
            : base(AddressType.Postal, addressFormat, boxNumber, unitNumber, buildingNumber, buildingName, streetNumber, streetName, postOffice, suiteNumber, 
                   null, null, null, null, null, suburb, city, postalCode, province, country, false)
        {
            this.AddressFormat      = addressFormat;
            this.BoxNumber          = boxNumber;
            this.UnitNumber         = unitNumber;
            this.BuildingNumber     = buildingNumber;
            this.BuildingName       = buildingName;
            this.StreetNumber       = streetNumber;
            this.StreetName         = streetName;
            this.PostNetSuiteNumber = suiteNumber;
            this.PostOffice         = postOffice;
            this.Suburb             = suburb;
            this.City               = city;
            this.PostalCode         = postalCode;
            this.Province           = province;
            this.Country            = country;
        }
    }
}
