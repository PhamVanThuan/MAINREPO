using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.ActiveValuation
{
    public class ActiveValuationChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ActiveValuationChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Active Valuation";
        }
    }
}