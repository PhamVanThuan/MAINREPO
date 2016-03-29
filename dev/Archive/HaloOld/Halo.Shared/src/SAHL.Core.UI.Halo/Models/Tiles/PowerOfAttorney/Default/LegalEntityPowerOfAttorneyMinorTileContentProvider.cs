using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.PowerOfAttorney.Default
{
    public class LegalEntityPowerOfAttorneyMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityPowerOfAttorneyMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT count(RelatedLegalEntityKey) as PowerOfAttorneyCount
                                    from [2AM].[dbo].[LegalEntity] le
                                    join [2AM].[dbo].[LegalEntityRelationship] ler on ler.LegalEntityKey = le.LegalEntityKey
	                                    and ler.RelationshipTypeKey = 5
                                    where le.legalentitykey = {0}", businessKey.Key);
        }
    }
}