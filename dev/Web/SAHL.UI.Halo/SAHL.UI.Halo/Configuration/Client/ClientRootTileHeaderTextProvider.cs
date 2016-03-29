using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client
{
    public class ClientRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ClientRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ClientRootModel;
            if (model == null) { return; }
            this.HeaderText = model.LegalName;
        }
    }
}