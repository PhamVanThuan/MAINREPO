using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System;

namespace SAHL.Core.UI.Halo.Tiles.AssetLiability.Default
{
    public class LegalEntityAssetLiabilitySummaryMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityAssetLiabilitySummaryMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return String.Format(@"select SUM(al.AssetValue) as  TotalAssets, SUM(al.LiabilityValue) as TotalLiability, SUM(al.AssetValue) - SUM(al.LiabilityValue) as TotalNetAssets
                    from [2am].dbo.LegalEntityAssetLiability leal
                    join [2am].dbo.AssetLiability al on leal.AssetLiabilityKey = al.AssetLiabilityKey
                    where leal.GeneralStatusKey = 1 --active
                    and leal.LegalEntityKey = {0}", businessKey.Key.ToString());
        }
    }
}