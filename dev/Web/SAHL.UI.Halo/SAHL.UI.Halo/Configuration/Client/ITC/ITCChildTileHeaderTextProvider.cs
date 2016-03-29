using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.ITC
{
    public class ITCChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ITCChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "ITC Information";
        }
    }
}