using SAHL.Core.Validation;

namespace SAHL.Services.Interfaces.PropertyDomain.Models
{
    public class ComcorpOfferPropertyDetailsModel : ValidatableModel
    {
        public ComcorpOfferPropertyDetailsModel(string sellerIDNo, string sahlOccupancyType, string sahlPropertyType, string sahlTitleType, string sectionalTitleUnitNo, string complexName, 
            string streetNo, string streetName, string suburb, string city, string province, string postalCode, string contactCellphone, string contactName, string namePropertyRegistered, 
            string standErfNo, string portionNo)
        {
            SellerIDNo = sellerIDNo;
            SAHLOccupancyType = sahlOccupancyType;
            SAHLPropertyType = sahlPropertyType;
            SAHLTitleType = sahlTitleType;
            SectionalTitleUnitNo = sectionalTitleUnitNo;
            ComplexName = complexName;
            StreetNo = streetNo;
            StreetName = streetName;
            Suburb = suburb;
            City = city;
            Province = province;
            PostalCode = postalCode;
            ContactCellphone = contactCellphone;
            ContactName = contactName;
            NamePropertyRegistered = namePropertyRegistered;
            StandErfNo = standErfNo;
            PortionNo = portionNo;
        }

        public string SellerIDNo { get; protected set; }

        public string SAHLOccupancyType { get; protected set; }

        public string SAHLPropertyType { get; protected set; }

        public string SAHLTitleType { get; protected set; }

        public string SectionalTitleUnitNo { get; protected set; }

        public string ComplexName { get; protected set; }

        public string StreetNo { get; protected set; }

        public string StreetName { get; protected set; }

        public string Suburb { get; protected set; }

        public string City { get; protected set; }

        public string Province { get; protected set; }

        public string PostalCode { get; protected set; }

        public string ContactCellphone { get; protected set; }

        public string ContactName { get; protected set; }

        public string NamePropertyRegistered { get; protected set; }

        public string StandErfNo { get; protected set; }

        public string PortionNo { get; protected set; }
    }
}