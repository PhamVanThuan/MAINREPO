using SAHL.UI.Halo.Models.ThirdParty;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.ThirdParty
{
    public class ThirdPartyRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ThirdPartyRootModel;
            if (model == null) { return; }

            this.HeaderText = model.LegalName;
        }
    }
}