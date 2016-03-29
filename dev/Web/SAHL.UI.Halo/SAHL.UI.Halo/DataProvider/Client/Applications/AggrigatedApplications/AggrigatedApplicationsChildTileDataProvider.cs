using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsChildDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<AggregatedApplicationsChildTileConfiguration>
    {
        public AggregatedApplicationsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {

            return string.Format(@"select DISTINCT DATEPART ( year , o.OfferStartDate)  AS Context, ro.LegalEntityKey AS BusinessKey,  0 as BusinessKeyType
                from
                    [2am].[dbo].[offerrole] ro
                join
                    [2am].[dbo].[offerroletype] rot on rot.OfferRoleTypeKey=ro.OfferRoleTypeKey
                join
                    [2am].[dbo].[offer] o on o.offerkey=ro.OfferKey
                where
                    ro.LegalEntityKey={0}
                    and rot.OfferRoleTypeGroupKey=3 -- client roles only
                    and ro.GeneralStatusKey=1 -- active role only
                    and o.OfferEndDate is not null -- only offers not yet finalised
                    and (select count(ro.LegalEntityKey) as NumOpenApplications
						    from
							    [2am].[dbo].[offerrole] ro
						    join
							    [2am].[dbo].[offerroletype] rot on rot.OfferRoleTypeKey=ro.OfferRoleTypeKey
						    join
							    [2am].[dbo].[offer] o on o.offerkey=ro.OfferKey
						    where
							    ro.LegalEntityKey={0}
							    and rot.OfferRoleTypeGroupKey=3 -- client roles only
							    and ro.GeneralStatusKey=1 -- active role only
							    and o.OfferEndDate is not null -- only offers not yet finalised
						                    group by
							                    ro.LegalEntityKey) > {1}", businessContext.BusinessKey.Key, 10);
        }

    }
}