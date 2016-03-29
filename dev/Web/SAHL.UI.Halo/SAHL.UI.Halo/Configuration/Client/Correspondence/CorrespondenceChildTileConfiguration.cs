using SAHL.UI.Halo.Models.Common.Correspondence;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Correspondence
{
    public class CorrespondenceChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                            IHaloTileModel<CorrespondenceChildModel>
    {
        public CorrespondenceChildTileConfiguration()
            : base("Correspondence", "Correspondence", 7, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}