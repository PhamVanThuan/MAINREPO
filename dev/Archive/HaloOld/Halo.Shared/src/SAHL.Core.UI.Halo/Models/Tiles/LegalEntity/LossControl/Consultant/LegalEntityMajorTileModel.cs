using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntity.LossControl.Consultant
{
    public class LegalEntityMajorTileModel : IAlternateTileModel<SAHL.Core.UI.Halo.Tiles.LegalEntity.Default.LegalEntityMajorTileModel>, ITileModel
    {
        public string LegalName { get; set; }
    }
}