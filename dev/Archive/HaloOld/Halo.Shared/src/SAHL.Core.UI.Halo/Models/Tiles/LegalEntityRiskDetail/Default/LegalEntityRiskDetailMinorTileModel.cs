using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntityRiskDetail.Default
{
    public class LegalEntityRiskDetailMinorTileModel : ITileModel
    {
        public DateTime LastITCDate { get; set; }
        public string EmpiricaScore { get; set; }
        public int BaselScore { get; set; }
    }
}