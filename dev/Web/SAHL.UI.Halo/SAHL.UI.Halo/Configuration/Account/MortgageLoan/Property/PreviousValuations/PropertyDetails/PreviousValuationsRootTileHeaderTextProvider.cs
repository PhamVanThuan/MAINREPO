using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PropertyDetails
{
    public class PreviousValuationsRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PreviousValuationsRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Property Details";
        }
    }
}