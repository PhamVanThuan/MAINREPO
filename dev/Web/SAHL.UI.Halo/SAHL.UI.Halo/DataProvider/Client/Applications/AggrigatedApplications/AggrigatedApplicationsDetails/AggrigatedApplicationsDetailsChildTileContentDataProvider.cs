using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.AggregatedApplications.AggregatedApplicationsDetails
{
    public class AggregatedApplicationsDetailsChildTileContentDataProvider : HaloTileBaseContentDataProvider<AggregatedApplicationsDetailsModel>
    {
        public AggregatedApplicationsDetailsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select o.offerKey as [ApplicationNumber], 
                                    ot.Description as [OfferType], 
                                    p.Description as Product, 
                                    o.ReservedAccountKey as [AccountNumber], 
                                    os.Description as [Status],
                                    OfferStartDate as [StartDate], 
                                    OfferEndDate as [EndDate] 
                                    from [2am].dbo.Offer o
                                    OUTER APPLY (SELECT max(offerInformationKey) as maxoi, offerKey 
                                    from [2am].dbo.OfferInformation oi where oi.offerKey = o.OfferKey
                                    group by offerKey) AS offerInfo
                                    join OfferInformation oi on offerInfo.maxoi = oi.OfferInformationKey
                                    join [2am].dbo.OfferType ot on o.offerTypeKey=ot.OfferTypeKey
                                    join [2am].dbo.OfferStatus os on o.OfferStatusKey = os.OfferStatusKey
                                    join [2am].dbo.Product p on oi.ProductKey=p.ProductKey
                                    where o.offerkey =  {0}
                                        union
                                    select o.offerKey as [ApplicationNumber], 
                                    ot.Description as [OfferType], 
                                    p.Description as Product, 
                                    o.ReservedAccountKey as [AccountNumber], 
                                    os.Description as [Status],
                                    OfferStartDate as [StartDate], 
                                    OfferEndDate as [EndDate] 
                                   
                                from [2am].dbo.ExternalRole er
                                    join [2am].dbo.ExternalRoleType ort on er.ExternalRoleTypeKey=ort.ExternalRoleTypeKey
                                    join [2am].dbo.Offer o on er.GenericKey=o.offerKey  
                                    OUTER APPLY (SELECT max(offerInformationKey) as maxoi, offerKey 
                                    from [2am].dbo.OfferInformation oi where oi.offerKey = o.OfferKey
                                    group by offerKey) AS offerInfo
                                    join OfferInformation oi on offerInfo.maxoi = oi.OfferInformationKey
                                    join [2am].dbo.OfferType ot on o.offerTypeKey=ot.OfferTypeKey
                                    join [2am].dbo.OfferStatus os on o.OfferStatusKey = os.OfferStatusKey
                                    join [2am].dbo.Product p on oi.ProductKey=p.ProductKey
                                    where o.offerkey =  {0}
                                    and er.GenericKeyTypeKey = 2
                                        and ort.ExternalRoleTypeGroupKey=1
                                        and er.GeneralStatusKey=1
                                        and o.OfferTypeKey = 11", businessContext.BusinessKey.Key);
        }
    }
}