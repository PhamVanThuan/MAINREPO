using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Client.Applications
{
    public class AggregatedApplicationsModel : IHaloTileModel
    {
        public int Year { get; set; }

        public int NumberOfApplications { get; set; }
    }
}