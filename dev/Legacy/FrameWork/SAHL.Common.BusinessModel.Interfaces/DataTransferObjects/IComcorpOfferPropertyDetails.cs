namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public interface IComcorpOfferPropertyDetails
    {
        int OfferKey { get; set; }

        string SellerIDNo { get; set; }

        string SAHLOccupancyType { get; set; }

        string SAHLPropertyType { get; set; }

        string SAHLTitleType { get; set; }

        string SectionalTitleUnitNo { get; set; }

        string ComplexName { get; set; }

        string StreetNo { get; set; }

        string StreetName { get; set; }

        string Suburb { get; set; }

        string City { get; set; }

        string Province { get; set; }

        string PostalCode { get; set; }

        string ContactCellphone { get; set; }

        string ContactName { get; set; }

        string NamePropertyRegistered { get; set; }

        string StandErfNo { get; set; }

        string PortionNo { get; set; }
    }
}