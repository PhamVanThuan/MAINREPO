using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.RelatedLegalEntities
{
    public class RelatedLegalEntitiesChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<RelatedLegalEntitiesChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Related Legal Entities";
        }
    }
}