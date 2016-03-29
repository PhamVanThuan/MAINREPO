using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<AggregatedApplicationsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "";
        }
    }
}