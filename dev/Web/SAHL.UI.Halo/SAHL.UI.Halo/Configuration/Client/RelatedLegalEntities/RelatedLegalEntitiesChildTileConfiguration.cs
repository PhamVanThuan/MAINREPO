using SAHL.UI.Halo.Models.Common.RelatedLegalEntities;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.RelatedLegalEntities
{
    public class RelatedLegalEntitiesChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                            IHaloTileModel<RelatedLegalEntitiesChildModel>
    {
        public RelatedLegalEntitiesChildTileConfiguration()
            : base("Related Legal Entities", "RelatedLegalEntities", 9, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}