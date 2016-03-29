using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications.AggregatedApplicationsDetails
{
    public class AggregatedApplicationsDetailsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<AggregatedApplicationsDetailsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as AggregatedApplicationsDetailsModel;
            if (model == null) { return; }
            this.HeaderText = model.OfferType + " - " + model.ApplicationNumber;
        }
    }
}
