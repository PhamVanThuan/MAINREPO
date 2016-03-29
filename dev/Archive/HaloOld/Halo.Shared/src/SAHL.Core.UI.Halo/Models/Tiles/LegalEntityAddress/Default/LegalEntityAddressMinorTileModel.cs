using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default
{
    public class LegalEntityAddressMinorTileModel : ITileModel
    {
        public string AddressType { get; set; }
        public string Address { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string IsDomicilium { get; set; }
        public string Notification { get; set; }
    }
}