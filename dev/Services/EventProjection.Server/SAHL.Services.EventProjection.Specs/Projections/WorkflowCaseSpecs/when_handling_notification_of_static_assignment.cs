using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.WorkflowCaseSpecs
{
    public class when_handling_notification_of_static_assignment : WithFakes
    {
        private static IDbFactory dbFactory;
        private static InstanceStaticWorkflowAssignmentNotifiedHandler handler;
        private static InstanceStaticWorkflowAssignmentNotifiedEvent @event;
        private static IServiceRequestMetadata metadata;

        private Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            handler = new InstanceStaticWorkflowAssignmentNotifiedHandler(dbFactory);
            @event = new InstanceStaticWorkflowAssignmentNotifiedEvent(DateTime.Now, 12132123L, (int)GenericKeyType.ThirdPartyInvoice, "Invoice Payment Processor", 12345);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            handler.Handle(@event, metadata);
        };

        private It should_remove_the_record_for_the_instance = () =>
        {
            dbFactory
                .NewDb()
                .InAppContext()
                .WasToldTo(a => a.ExecuteNonQuery(Param<RemoveCurrentlyAssignedUserForInstanceStatement>
                    .Matches(b => b.InstanceId == @event.InstanceId)))
                .OnlyOnce();
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}