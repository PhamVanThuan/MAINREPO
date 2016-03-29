using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ComcorpApplicationPropertyDetailsModel
    {
        public ComcorpApplicationPropertyDetailsModel(string sellerIDNo, string sahlOccupancyType, string sahlPropertyType, string sahlTitleType, 
                                                      string sectionalTitleUnitNo, string complexName, string streetNo, string streetName, string suburb, string city, 
                                                      string province, string postalCode, string contactCellphone, string contactName, string namePropertyRegistered, 
                                                      string standErfNo, string portionNo)
        {
            SellerIDNo             = sellerIDNo;
            SAHLOccupancyType      = sahlOccupancyType;
            SAHLPropertyType       = sahlPropertyType;
            SAHLTitleType          = sahlTitleType;
            SectionalTitleUnitNo   = sectionalTitleUnitNo;
            ComplexName            = complexName;
            StreetNo               = streetNo;
            StreetName             = streetName;
            Suburb                 = suburb;
            City                   = city;
            Province               = province;
            PostalCode             = postalCode;
            ContactCellphone       = contactCellphone;
            ContactName            = contactName;
            NamePropertyRegistered = namePropertyRegistered;
            StandErfNo             = standErfNo;
            PortionNo              = portionNo;
        }

        [DataMember]
        public string SellerIDNo { get; set; }

        [DataMember]
        public string SAHLOccupancyType { get; set; }

        [DataMember]
        public string SAHLPropertyType { get; set; }

        [DataMember]
        public string SAHLTitleType { get; set; }

        [DataMember]
        public string SectionalTitleUnitNo { get; set; }

        [DataMember]
        public string ComplexName { get; set; }

        [DataMember]
        public string StreetNo { get; set; }

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
        public string ContactCellphone { get; set; }

        [DataMember]
        public string ContactName { get; set; }

        [DataMember]
        public string NamePropertyRegistered { get; set; }

        [DataMember]
        public string StandErfNo { get; set; }

        [DataMember]
        public string PortionNo { get; set; }
    }
}
