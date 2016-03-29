using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Applications.ApplicationDetails;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.ApplicationDetails
{
    public class AggregatedApplicationsChildDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<ApplicationDetailsChildTileConfiguration>
    {
        public AggregatedApplicationsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Offer;
            return string.Format(@"SELECT * from(
                                    SELECT o.offerKey AS BusinessKey, {2} as BusinessKeyType
                                   FROM
                                      [2am].[dbo].[offerrole] ro
                                   JOIN
                                     [2am].[dbo].[offerroletype] rot on rot.OfferRoleTypeKey=ro.OfferRoleTypeKey
                                   JOIN
                                    [2am].[dbo].[offer] o on o.offerkey=ro.OfferKey
                                    WHERE
                                    ro.LegalEntityKey = {0}
                                    AND rot.OfferRoleTypeGroupKey=3 -- client roles only
                                    AND ro.GeneralStatusKey=1 -- active role only
                                    AND o.OfferEndDate IS NOT NULL -- only offers not yet finalised
                                     union
                                    select distinct er.GenericKey AS BusinessKey, {2} as BusinessKeyType 
                                    from [2am].dbo.ExternalRole er
                                        join [2am].dbo.ExternalRoleType ort on er.ExternalRoleTypeKey=ort.ExternalRoleTypeKey
                                        join [2am].dbo.Offer o on er.GenericKey=o.offerKey  
                                        where LegalEntityKey={0}
                                            and er.GenericKeyTypeKey = 2
                                            and ort.ExternalRoleTypeGroupKey=1
                                            and er.GeneralStatusKey=1
                                            and o.OfferTypeKey = 11) as g
                                    where (SELECT count(*)  
                                               FROM 
                                                  (
                                    SELECT o.offerKey AS BusinessKey, {2} as BusinessKeyType
                                   FROM
                                      [2am].[dbo].[offerrole] ro
                                   JOIN
                                     [2am].[dbo].[offerroletype] rot on rot.OfferRoleTypeKey=ro.OfferRoleTypeKey
                                   JOIN
                                    [2am].[dbo].[offer] o on o.offerkey=ro.OfferKey
                                    WHERE
                                    ro.LegalEntityKey = {0}
                                    AND rot.OfferRoleTypeGroupKey=3 -- client roles only
                                    AND ro.GeneralStatusKey=1 -- active role only
                                    AND o.OfferEndDate IS NOT NULL -- only offers not yet finalised
                                     union
                                    select distinct er.GenericKey AS BusinessKey, {2} as BusinessKeyType 
                                    from [2am].dbo.ExternalRole er
                                        join [2am].dbo.ExternalRoleType ort on er.ExternalRoleTypeKey=ort.ExternalRoleTypeKey
                                        join [2am].dbo.Offer o on er.GenericKey=o.offerKey  
                                        where LegalEntityKey={0}
                                            and er.GenericKeyTypeKey = 2
                                            and ort.ExternalRoleTypeGroupKey=1
                                            and er.GeneralStatusKey=1
                                            and o.OfferTypeKey = 11) AS numberOfApplications) <= {1}"
                                    , businessContext.BusinessKey.Key, 10, genericKeyType);
        }

    }
}