using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.DebtCounselling
{
    public class DebtCounsellingChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<DebtCounsellingChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Debt Counselling";
        }
    }
}