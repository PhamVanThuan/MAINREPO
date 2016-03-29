using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Applications.ApplicationDetails
{
    public class ApplicationDetailsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ApplicationDetailsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ApplicationDetailsModel;
            if (model == null) { return; }
            this.HeaderText = model.OfferType + " - " + model.ApplicationNumber;
        }
    }
}