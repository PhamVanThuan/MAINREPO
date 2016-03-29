using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.GetSystemRequestToProcessCommandHandlerSpecs
{
    public class when_getting_system_requests_to_process_where_state_is_autoforward : WithFakes
    {
        private static AutoMocker<BuildSystemRequestToProcessCommandHandler> automocker;
        private static BuildSystemRequestToProcessCommand command;
        private static InstanceDataModel instance;
        private static StateDataModel state;
        private static IX2ContextualDataProvider contextualData;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            contextualData = An<IX2ContextualDataProvider>();
            automocker = new NSubstituteAutoMocker<BuildSystemRequestToProcessCommandHandler>();
            state = new StateDataModel(1, "AutoForwardState", 2, true, null, null, null, Guid.NewGuid());
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            command = new BuildSystemRequestToProcessCommand(instance, contextualData);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById((int)instance.StateID)).Return(state);
        };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_get_the_state_that_the_instance_is_at = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStateById((int)instance.StateID));
            };

        private It should_add_an_autoforward_activity = () =>
            {
                automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.QueueUpCommandToBeProcessed<PublishBundledRequestCommand>(Arg.Is<PublishBundledRequestCommand>(y => ((NotificationOfNewAutoForwardCommand)y.Commands[0]).InstanceId == command.Instance.ID)));
            };
    }
}