using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.X2.Events;
using SAHL.Services.EventProjection.Projections.Workflow;
using SAHL.Services.EventProjection.Projections.Workflow.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.WorkflowSpecs
{
    public class when_updating_current_state_for_instance : WithFakes
    {
        private static IDbFactory dbFactory;
        private static UpdateCurrentStateForInstanceHandler handler;
        private static WorkflowTransitionEvent @event;
        private static IServiceRequestMetadata metadata;

        private Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            handler = new UpdateCurrentStateForInstanceHandler(dbFactory);
            @event = new WorkflowTransitionEvent(123456L, @"SAHL\ClintonS", "Third Party Invoices", "Third Party Invoices", 34, 54, "Approve for Payment", DateTime.Now,
                1, 2, "Invoice Captured", "Awaiting Payment");
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
                .WasToldTo(a => a.ExecuteNonQuery(Param<UpdateCurrentStateForInstanceStatement>
                    .Matches(b => b.InstanceId == @event.InstanceId
                        && b.WorkflowName == @event.WorkflowName
                        && b.WorkflowState == @event.ToStateName
                        && b.GenericKeyTypeKey == @event.GenericKeyTypeKey
                        && b.GenericKey == @event.GenericKey)))
                .OnlyOnce();
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}