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
    public class when_inserting_for_invoice_rejection_pre_approval : WithFakes
    {
        private static IDbFactory dbFactory;
        private static InsertThirdPartyInvoiceRejectionCorrespondenceHandler handler;
        private static ThirdPartyInvoiceRejectedPreApprovalEvent @event;
        private static IServiceRequestMetadata metadata;

        private Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            handler = new InsertThirdPartyInvoiceRejectionCorrespondenceHandler(dbFactory);
            @event = new ThirdPartyInvoiceRejectedPreApprovalEvent(DateTime.Now, 1408282, "attorney@straussdaly.co.za", "InvoiceNumber", @"SAHL\ClintonS", "Rejection Comments", "SAHL_Reference",
                54, "Email_Subject_Line", Guid.NewGuid());
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
                        && b.CorrespondenceReason == "Third Party Invoice Rejection"
                        && b.CorrespondenceType == "Internal Query"
                        && b.UserName == @event.RejectedBy
                        && b.MemoText == @event.RejectionComments
                        )))
                .OnlyOnce();
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}