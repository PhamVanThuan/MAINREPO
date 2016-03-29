using System;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Services.EventProjection.Specs.Projections.WorkflowCaseSpecs
{
    public class when_updating_the_last_assignment_for_a_user_in_a_capability : WithFakes
    {
        Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            userName = "banana";
            dbFactory
                .NewDb()
                .InAppContext()
                .WhenToldTo(a => a.SelectOne(Arg.Any<GetUserNameForUserOrganisationStructureKeyStatement>()))
                .Return(userName);
            handler = new UpdateLastAssignedUserByCapabilityForInstanceHandler(dbFactory);
            @event = new WorkflowCaseAssignedEvent(DateTime.Now, 1, 2, 3L, 4, 5);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            handler.Handle(@event, metadata);
        };

        private It should_have_retrieved_the_username_for_the_userOrganisationStructureKey = () =>
        {
            dbFactory
                .NewDb()
                .InAppContext()
                .WasToldTo(a => a.SelectOne(Param<GetUserNameForUserOrganisationStructureKeyStatement>
                    .Matches(b => b.UserOrganisationStructureKey == @event.UserOrganisationStructureKey)))
                .OnlyOnce();
        };

        private It should_have_updated_the_projection = () =>
        {
            dbFactory
                .NewDb()
                .InAppContext()
                .WasToldTo(a => a.ExecuteNonQuery(Param<UpdateLastAssignedUserByCapabilityForInstanceStatement>
                    .Matches(b => b.InstanceId == @event.InstanceId
                        && b.CapabilityKey == @event.CapabilityKey
                        && b.UserOrganisationStructureKey == @event.UserOrganisationStructureKey
                        && b.GenericKeyTypeKey == @event.GenericKeyTypeKey
                        && b.GenericKey == @event.GenericKey
                        && b.UserName == userName)))
                .OnlyOnce();
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };

        private static IDbFactory dbFactory;
        private static UpdateLastAssignedUserByCapabilityForInstanceHandler handler;
        private static WorkflowCaseAssignedEvent @event;
        private static IServiceRequestMetadata metadata;
        private static string userName;
    }
}
