using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.AssetLiability.Default
{
    public class LegalEntityAssetLiabilitySummaryMinorTileModel : ITileModel
    {
        public decimal TotalAssets { get; set; }
        public decimal TotalLiability { get; set; }
        public decimal TotalNetAssets { get; set; }
    }
}