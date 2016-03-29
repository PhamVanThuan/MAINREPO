using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ComcorpOfferPropertyDetailsDataModel :  IDataModel
    {
        public ComcorpOfferPropertyDetailsDataModel(int offerKey, string sellerIDNo, string sAHLOccupancyType, string sAHLPropertyType, string sAHLTitleType, string sectionalTitleUnitNo, string complexName, string streetNo, string streetName, string suburb, string city, string province, string postalCode, string contactCellphone, string contactName, string namePropertyRegistered, string standErfNo, string portionNo, DateTime insertDate, DateTime? changeDate)
        {
            this.OfferKey = offerKey;
            this.SellerIDNo = sellerIDNo;
            this.SAHLOccupancyType = sAHLOccupancyType;
            this.SAHLPropertyType = sAHLPropertyType;
            this.SAHLTitleType = sAHLTitleType;
            this.SectionalTitleUnitNo = sectionalTitleUnitNo;
            this.ComplexName = complexName;
            this.StreetNo = streetNo;
            this.StreetName = streetName;
            this.Suburb = suburb;
            this.City = city;
            this.Province = province;
            this.PostalCode = postalCode;
            this.ContactCellphone = contactCellphone;
            this.ContactName = contactName;
            this.NamePropertyRegistered = namePropertyRegistered;
            this.StandErfNo = standErfNo;
            this.PortionNo = portionNo;
            this.InsertDate = insertDate;
            this.ChangeDate = changeDate;
		
        }		

        public int OfferKey { get; set; }

        public string SellerIDNo { get; set; }

        public string SAHLOccupancyType { get; set; }

        public string SAHLPropertyType { get; set; }

        public string SAHLTitleType { get; set; }

        public string SectionalTitleUnitNo { get; set; }

        public string ComplexName { get; set; }

        public string StreetNo { get; set; }

        public string StreetName { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public string ContactCellphone { get; set; }

        public string ContactName { get; set; }

        public string NamePropertyRegistered { get; set; }

        public string StandErfNo { get; set; }

        public string PortionNo { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? ChangeDate { get; set; }
    }
}