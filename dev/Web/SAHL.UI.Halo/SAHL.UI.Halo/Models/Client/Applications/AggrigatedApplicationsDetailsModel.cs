using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Client.Applications
{
    public class ApplicationDetailsModel : IHaloTileModel
    {

        public int ApplicationNumber { get; set; }

        public string OfferType { get; set; }

        public string Product { get; set; }

        public int AccountNumber { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

      
    }
}