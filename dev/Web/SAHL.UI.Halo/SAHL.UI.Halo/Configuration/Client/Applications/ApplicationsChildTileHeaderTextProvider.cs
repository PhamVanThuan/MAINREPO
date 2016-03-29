using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ApplicationsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Applications";
        }
    }
}