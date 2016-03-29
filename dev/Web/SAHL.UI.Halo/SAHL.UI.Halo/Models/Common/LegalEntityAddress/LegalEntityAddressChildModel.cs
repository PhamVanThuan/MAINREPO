using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Common.LegalEntityAddress
{
    public class LegalEntityAddressChildModel : IHaloTileModel
    {
        public string AddressType { get; set; }

        public string Address { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string IsDomicilium { get; set; }

        public string Notification { get; set; }
    }
}