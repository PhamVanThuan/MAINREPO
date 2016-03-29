using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.FinancialProfile
{
    public class FinancialProfileChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<FinancialProfileChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Financial Profile";
        }
    }
}