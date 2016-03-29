using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CheckRuleResultCommandHandlerSpecs
{
    public class when_rule_returns_true : WithFakes
    {
        private static AutoMocker<CheckRuleResultCommandHandler> autoMocker;
        static CheckRuleResultCommand command = new CheckRuleResultCommand(true, "ruleName");
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
            {
                autoMocker = new NSubstituteAutoMocker<CheckRuleResultCommandHandler>();
            };

        Because of = () =>
        {
            messages = autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_return_an_empty_message_collection = () =>
            {
                messages.HasErrors.ShouldEqual(false);
                messages.HasWarnings.ShouldEqual(false);
            };
    }
}