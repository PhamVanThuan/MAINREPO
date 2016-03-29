using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Account.Property
{
    public class ActiveValuationModel : IHaloTileModel
    {
        public string Valuer { get; set; }

        public string ValuationAge { get; set; }

        public DateTime ValuationDate { get; set; }

        public double ValuationAmount { get; set; }

        public double ValuationMunicipal { get; set; }

        public double ValuationHOCValue { get; set; }

        public string HOCRoofType { get; set; }

        public string ValuationProvider { get; set; }
    }
}