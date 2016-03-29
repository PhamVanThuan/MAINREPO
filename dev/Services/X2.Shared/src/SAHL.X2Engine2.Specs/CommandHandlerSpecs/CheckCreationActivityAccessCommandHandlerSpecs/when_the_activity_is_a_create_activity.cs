using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CheckCreationActivityAccessCommandHandlerSpecs
{
    public class when_the_activity_is_a_create_activity : WithFakes
    {
        private static AutoMocker<CheckCreationActivityAccessCommandHandler> automocker = new NSubstituteAutoMocker<CheckCreationActivityAccessCommandHandler>();
        private static CheckCreationActivityAccessCommand command;
        private static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            Activity activity = new Activity(1, "Test", null, string.Empty, 0, string.Empty, 1, false);
            command = new CheckCreationActivityAccessCommand(activity);
        };

        Because of = () =>
        {
            messages = automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };

        It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };
    }
}