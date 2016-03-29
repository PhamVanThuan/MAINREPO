using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class AddressModel : IDataModel
    {
        public AddressModel(AddressType addressType, AddressFormat addressFormat, string boxNumber, string unitNumber, string buildingNumber, 
                            string buildingName, string streetNumber, string streetName, string postOffice, string postNetSuiteNumber, string freeText1, 
                            string freeText2, string freeText3, string freeText4, string freeText5, string suburb, string city, string postalCode, 
                            string province, string country, bool isDomicilium)
        {
            this.AddressType        = addressType;
            this.AddressFormat      = addressFormat;
            this.BoxNumber          = boxNumber;
            this.UnitNumber         = unitNumber;
            this.BuildingNumber     = buildingNumber;
            this.BuildingName       = buildingName;
            this.StreetNumber       = streetNumber;
            this.StreetName         = streetName;
            this.PostOffice         = postOffice;
            this.Country            = country;
            this.Province           = province;
            this.City               = city;
            this.Suburb             = suburb;
            this.PostalCode         = postalCode;
            this.PostNetSuiteNumber = postNetSuiteNumber;
            this.FreeText1          = freeText1;
            this.FreeText2          = freeText2;
            this.FreeText3          = freeText3;
            this.FreeText4          = freeText4;
            this.FreeText5          = freeText5;
            this.IsDomicilium       = isDomicilium;
        }

        [DataMember]
        public AddressType AddressType { get; set; }

        [DataMember]
        public AddressFormat AddressFormat { get; set; }

        [DataMember]
        public string BoxNumber { get; set; }

        [DataMember]
        public string UnitNumber { get; set; }

        [DataMember]
        public string BuildingNumber { get; set; }

        [DataMember]
        public string BuildingName { get; set; }

        [DataMember]
        public string StreetNumber { get; set; }

        [DataMember]
        public string StreetName { get; set; }

        [DataMember]
        public string PostOffice { get; set; }

        [DataMember]
        public string PostNetSuiteNumber { get; set; }

        [DataMember]
        public string FreeText1 { get; set; }

        [DataMember]
        public string FreeText2 { get; set; }

        [DataMember]
        public string FreeText3 { get; set; }

        [DataMember]
        public string FreeText4 { get; set; }

        [DataMember]
        public string FreeText5 { get; set; }

        [DataMember]
        public string Suburb { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public bool IsDomicilium { get; set; }
    }
}
