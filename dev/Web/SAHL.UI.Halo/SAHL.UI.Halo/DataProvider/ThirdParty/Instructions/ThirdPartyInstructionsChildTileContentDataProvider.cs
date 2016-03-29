using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Instructions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Instructions
{
    public class ThirdPartyInstructionsChildTileContentDataProvider : HaloTileBaseContentDataProvider<InstructionChildModel>
    {
        public ThirdPartyInstructionsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"WITH openInstructions AS (
                                    --HOW MANY OPEN OFFERS IS A COVEYNACING ATTORNEY ON?
                                    select
                                    ort.Description as role, ro.offerkey as Generickey, gkt.GenericKeyTypeKey, gkt.Description, count(o.OfferKey) as OpenInstructionsCount
                                    from [2AM].[dbo].[offerrole] ro
                                        join [2AM].[dbo].OfferRoleType ort on ort.OfferRoleTypeKey=ro.OfferRoleTypeKey
                                        join [2AM].[dbo].GenericKeyType gkt on gkt.GenericKeyTypeKey=2
                                        join [2AM].[dbo].offer o on o.offerkey=ro.offerkey and OfferEndDate is null
                                    where ro.OfferRoleTypeKey=4
                                        and ro.GeneralStatusKey = 1
                                        and ro.LegalEntityKey = {0}
                                    group by ort.Description, ro.OfferKey, gkt.GenericKeyTypeKey, gkt.Description

                                    UNION
                                    --HOW MANY DEBT-COUNSELLING CASES IS HE AN ATTOREY ON? -- ROLE MAY BE LITIGATION
                                    select
                                    ert.Description as role, er.GenericKey, gkt.GenericKeyTypeKey, gkt.Description, count(dc.AccountKey) as OpenInstructionsCount
                                    from [2AM].[dbo].ExternalRole er
                                        join [2AM].[dbo].ExternalRoleType ert on ert.ExternalRoleTypeKey=er.ExternalRoleTypeKey
                                        join [2AM].[dbo].GenericKeyType gkt on gkt.GenericKeyTypeKey=er.GenericKeyTypeKey and gkt.GenericKeyTypeKey=27
                                        join [2AM].debtcounselling.DebtCounselling dc on dc.DebtCounsellingKey=er.GenericKey and dc.DebtCounsellingStatusKey in (1)
                                    where er.ExternalRoleTypeKey in (5,6,7,8,9)
                                        and er.LegalEntityKey = {0}
                                    group by ert.Description, er.GenericKey, gkt.GenericKeyTypeKey, gkt.Description
                                )
                                SELECT
                                    SUM(openInstructions.OpenInstructionsCount) AS TotalOpenInstructions
                                FROM openInstructions", businessContext.BusinessKey.Key);
        }
    }
}