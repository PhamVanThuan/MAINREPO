using SAHL.UI.Halo.Models.Client.ITC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.ITC
{
    public class ITCChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                      IHaloTileModel<ITCChildModel>
    {
        public ITCChildTileConfiguration()
            : base("ITC", "ITC", 6, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}