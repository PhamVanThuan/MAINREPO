using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsChildTileContentDataProvider : HaloTileBaseContentDataProvider<AggregatedApplicationsModel>
    {
        public AggregatedApplicationsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
        public override string GetSqlStatement(BusinessContext businessContext)
        {

            return string.Format(@"select {1} AS Year, count(*) AS NumberOfApplications from (
                    select distinct ofr.offerKey 
                    from [2am].dbo.OfferRole ofr
                    join [2am].dbo.OfferRoleType ort on ofr.OfferRoleTypeKey=ort.OfferRoleTypeKey
                        and ort.OfferRoleTypeGroupKey=3
                    join [2am].dbo.Offer o on ofr.offerKey=o.offerKey
                        and o.offerTypeKey in (2,3,4,6,7,8, 11)
                    where LegalEntityKey={0} 
                        and ofr.GeneralStatusKey=1
                        and DATEPART (year , o.OfferStartDate) = {1} 
                    union
                    select distinct er.GenericKey from [2am].dbo.ExternalRole er
                    join [2am].dbo.ExternalRoleType ort on er.ExternalRoleTypeKey=ort.ExternalRoleTypeKey
                    join [2am].dbo.Offer o on er.GenericKey=o.offerKey  
                    where LegalEntityKey={0}
                    and er.GenericKeyTypeKey = 2
                        and ort.ExternalRoleTypeGroupKey=1
                        and er.GeneralStatusKey=1
                        and o.OfferTypeKey = 11
                        and DATEPART (year , o.OfferStartDate) = {1}) AS Applications
                    ", businessContext.BusinessKey.Key, businessContext.Context);
        }
    }
}