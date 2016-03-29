using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Account.Property
{
    public class PropertyChildModel : IHaloTileModel
    {
        public string DataProvider { get; set; }

        public string PropertyAddress { get; set; }

        public string Occupancy { get; set; }

        public string PropertyType { get; set; }

        public string TitleType { get; set; }

        public string LegalDescription1 { get; set; }

        public string LegalDescription2 { get; set; }

        public string LegalDescription3 { get; set; }

        public string AreaClassification { get; set; }
    }
}