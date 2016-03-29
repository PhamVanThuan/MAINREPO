using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PropertyAddressModel
    {
        public PropertyAddressModel(string unitNumber, string buildingName, string buildingNumber, string streetNumber, string streetName, 
                                    string suburb, string city, string province, string postalCode, string erfNumber, string erfPortionNumber, 
                                    string country, bool isDomicilium)
        {
            this.BuildingName     = buildingName;
            this.BuildingNumber   = buildingNumber;
            this.City             = city;
            this.PostalCode       = postalCode;
            this.Province         = province;
            this.StreetName       = streetName;
            this.StreetNumber     = streetNumber;
            this.Suburb           = suburb;
            this.UnitNumber       = unitNumber;
            this.ErfNumber        = erfNumber;
            this.ErfPortionNumber = erfPortionNumber;
            this.Country          = country;
            this.IsDomicilium     = IsDomicilium;
        }

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
        public string Suburb { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string ErfNumber { get; set; }

        [DataMember]
        public string ErfPortionNumber { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public bool IsDomicilium { get; set; }
    }
}