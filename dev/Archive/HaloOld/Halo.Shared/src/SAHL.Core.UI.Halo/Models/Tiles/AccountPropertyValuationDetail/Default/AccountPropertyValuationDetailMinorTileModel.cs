using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Halo.Tiles.AccountPropertyValuationDetail.Default
{
    public class AccountPropertyValuationDetailMinorTileModel : ITileModel
    {
        public DateTime LastValuationDate { get; set; }
        public string LastValuationAmount { get; set; }
    }
}