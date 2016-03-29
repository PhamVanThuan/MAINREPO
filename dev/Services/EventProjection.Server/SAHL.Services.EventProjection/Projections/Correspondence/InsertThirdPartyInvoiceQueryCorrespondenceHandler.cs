using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.Correspondence.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.Correspondence
{
    public class InsertThirdPartyInvoiceQueryCorrespondenceHandler : ITableProjector<ThirdPartyInvoiceQueriedPostApprovalEvent, IDataModel>,
                                                                     ITableProjector<ThirdPartyInvoiceQueriedPreApprovalEvent, IDataModel>
    {
        private readonly IDbFactory dbFactory;

        public InsertThirdPartyInvoiceQueryCorrespondenceHandler(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void Handle(ThirdPartyInvoiceQueriedPreApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            InsertCorrespondence(@event);
        }

        public void Handle(ThirdPartyInvoiceQueriedPostApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            InsertCorrespondence(@event);
        }

        private void InsertCorrespondence(ThirdPartyInvoiceQueriedEvent @event)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new InsertCorrespondenceStatement(@event.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, "Third Party Invoice Query",
                    "Internal Query", "Memo", @event.QueryInitiatedBy, @event.Date, @event.QueryComments);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }
    }
}