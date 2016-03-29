using SAHL.UI.Halo.Models.Client.Detail.EmploymentSummary;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail.EmploymentSummary
{
    public class EmploymentSummaryChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ClientDetailRootTileConfiguration>,
                                                            IHaloTileModel<EmploymentSummaryChildModel>
    {
        public EmploymentSummaryChildTileConfiguration()
            : base("Employment Summary", "ClientEmploymentSummary", sequence: 3, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}
