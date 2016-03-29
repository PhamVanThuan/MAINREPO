using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class ComcorpOfferPropertyDetails : IComcorpOfferPropertyDetails
    {
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
    }
}