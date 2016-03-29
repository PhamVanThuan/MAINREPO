using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Instructions
{
    public class ThirdPartyInstructionsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInstructionsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Instructions";
        }
    }
}