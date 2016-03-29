using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.Correspondence;
using SAHL.Services.EventProjection.Projections.Correspondence.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Specs.Projections.WorkflowSpecs
{
    public class when_inserting_for_invoice_query_pre_approval : WithFakes
    {
        private static IDbFactory dbFactory;
        private static InsertThirdPartyInvoiceQueryCorrespondenceHandler handler;
        private static ThirdPartyInvoiceQueriedPreApprovalEvent @event;
        private static IServiceRequestMetadata metadata;

        private Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            handler = new InsertThirdPartyInvoiceQueryCorrespondenceHandler(dbFactory);
            @event = new ThirdPartyInvoiceQueriedPreApprovalEvent(DateTime.Now, 1222, @"SAHL\ClintonS", "QueryComments");
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            handler.Handle(@event, metadata);
        };

        private It should_have_updated_the_projection = () =>
        {
            dbFactory
                .NewDb()
                .InAppContext()
                .WasToldTo(a => a.ExecuteNonQuery(Param<InsertCorrespondenceStatement>
                    .Matches(b => b.GenericKeyTypeKey == (int)GenericKeyType.ThirdPartyInvoice
                        && b.GenericKey == @event.ThirdPartyInvoiceKey
                        && b.CorrespondenceMedium == "Memo"
                        && b.CorrespondenceReason == "Third Party Invoice Query"
                        && b.CorrespondenceType == "Internal Query"
                        && b.UserName == @event.QueryInitiatedBy
                        && b.MemoText == @event.QueryComments
                        )))
                .OnlyOnce();
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}