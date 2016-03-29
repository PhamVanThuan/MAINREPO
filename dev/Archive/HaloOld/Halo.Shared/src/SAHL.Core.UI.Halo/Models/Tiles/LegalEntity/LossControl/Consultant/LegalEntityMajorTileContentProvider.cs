using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntity.LossControl.Consultant
{
    public class LegalEntityMajorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityMajorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format("select [2AM].[dbo].[LegalEntityLegalName]({0}, 0) as LegalName", businessKey.Key);
        }
    }
}