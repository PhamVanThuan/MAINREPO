using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.DebtCounselling;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.DebtCounselling
{
    public class DebtCounsellingChildTileContentDataProvider : HaloTileBaseContentDataProvider<DebtCounsellingChildModel>
    {
        public DebtCounsellingChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
                count(dc.DebtCounsellingKey) as NumOpenDebtCounsellingCases,
                count(dcp.ProposalKey) as NumActiveDebtCounsellingProposals,
                isnull(dbo.LegalEntityLegalName(der.LegalEntityKey, 0),'') as DebtCounsellor
                from
                    [2am].dbo.externalrole cer
                join
                    [2am].[debtcounselling].[DebtCounselling] dc on dc.DebtCounsellingKey=cer.GenericKey
                left join
                    [2am].[debtcounselling].[Proposal] dcp on dcp.DebtCounsellingKey=dc.DebtCounsellingKey
                left join
                    [2am].[dbo].[externalrole] der on der.GenericKeyTypeKey=cer.GenericKeyTypeKey and der.GenericKey=cer.GenericKey
                where
                    cer.legalentitykey={0}
                and
                    dc.DebtCounsellingStatusKey = 1 -- open debtcounselling case
                and
                    cer.GenericKeyTypeKey=27 -- debcounselling case
                and
                    cer.ExternalRoleTypeKey=1 -- client role
                and
                    cer.GeneralStatusKey=1 -- client active role
                and
                    dcp.ProposalStatusKey=1 -- Active proposal
                and
                    der.ExternalRoleTypeKey=2 -- debt counsellor
                and
                    der.GeneralStatusKey=1 -- debt counsellor active role
                group by
                    dc.DebtCounsellingKey,
                    dcp.ProposalKey,
                    der.LegalEntityKey;", businessContext.BusinessKey.Key);
        }
    }
}