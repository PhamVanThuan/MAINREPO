using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.Correspondence.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.Correspondence
{
    public class InsertThirdPartyInvoiceRejectionCorrespondenceHandler : ITableProjector<ThirdPartyInvoiceRejectedPostApprovalEvent, IDataModel>
                                                                       , ITableProjector<ThirdPartyInvoiceRejectedPreApprovalEvent, IDataModel>
    {
        private readonly IDbFactory dbFactory;

        public InsertThirdPartyInvoiceRejectionCorrespondenceHandler(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void Handle(ThirdPartyInvoiceRejectedPreApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            InsertCorrespondence(@event);
        }

        public void Handle(ThirdPartyInvoiceRejectedPostApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            InsertCorrespondence(@event);
        }

        private void InsertCorrespondence(ThirdPartyInvoiceRejectedEvent @event)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new InsertCorrespondenceStatement(@event.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, "Third Party Invoice Rejection",
                    "Internal Query", "Memo", @event.RejectedBy, @event.Date, @event.RejectionComments);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }
    }
}