using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations
{
    public class PreviousValuationsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PreviousValuationsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Previous Valuations";
        }
    }
}