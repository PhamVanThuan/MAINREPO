using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Detail
{
    public class ClientDetailRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ClientDetailRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ClientRootModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderText = model.LegalName;
        }
    }
}