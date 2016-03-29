using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<AggregatedApplicationsRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as AggregatedApplicationsRootModel;
            if (model == null) { return; }
            this.HeaderText = model.LegalName;
        }
    }
}