using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.Detail.EmploymentSummary
{
    public class EmploymentSummaryChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<EmploymentSummaryChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Employment Summary";
        }
    }
}