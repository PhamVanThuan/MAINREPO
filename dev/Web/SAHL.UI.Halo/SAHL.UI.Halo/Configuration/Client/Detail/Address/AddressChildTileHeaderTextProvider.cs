using SAHL.UI.Halo.Models.Common.LegalEntityAddress;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Detail.Address
{
    public class AddressChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<AddressChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as LegalEntityAddressChildModel;
            if (model == null) { return; }
            this.HeaderText = string.Format("Address - {0}", model.AddressType);
        }
    }
}