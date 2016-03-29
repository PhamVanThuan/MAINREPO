using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ApplicationsRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
          
            var model = dataModel as ApplicationRootModel;
            if (model == null) { return; }
            this.HeaderText = model.LegalName;
        }
    }
}