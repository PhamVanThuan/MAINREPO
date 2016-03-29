using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications;
using SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications.AggregatedApplicationsDetails;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.AggregatedApplications.AggregatedApplicationsDetails
{
    public class AggregatedApplicationsDetailsChildDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<AggregatedApplicationsDetailsChildTileConfiguration>
    {
        public AggregatedApplicationsDetailsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Offer;
            return string.Format(@"select o.offerKey AS BusinessKey, DATEPART ( year , o.OfferStartDate)  AS Context, {2} as BusinessKeyType
                                from
                                    [2am].[dbo].[offerrole] ro
                                join
                                    [2am].[dbo].[offerroletype] rot on rot.OfferRoleTypeKey=ro.OfferRoleTypeKey
                                join
                                    [2am].[dbo].[offer] o on o.offerkey=ro.OfferKey
                                where
                                    ro.LegalEntityKey = {0}
                                    and ro.GeneralStatusKey = 1 -- active role only
                                    and DATEPART ( year , o.OfferStartDate) = {1}
                                    union
                                    select distinct er.GenericKey AS BusinessKey, DATEPART ( year , o.OfferStartDate)  AS Context, {2} as BusinessKeyType 
                                    from [2am].dbo.ExternalRole er
                                        join [2am].dbo.ExternalRoleType ort on er.ExternalRoleTypeKey=ort.ExternalRoleTypeKey
                                        join [2am].dbo.Offer o on er.GenericKey=o.offerKey  
                                        where LegalEntityKey={0}
                                            and er.GenericKeyTypeKey = 2
                                            and ort.ExternalRoleTypeGroupKey=1
                                            and er.GeneralStatusKey=1
                                            and o.OfferTypeKey = 11
                                            and DATEPART (year , o.OfferStartDate) = {1}
                                        ", businessContext.BusinessKey.Key, businessContext.Context, genericKeyType);
        }
    }
}