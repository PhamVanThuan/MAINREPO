using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.HandleMapReturnCommandHandlerSpecs
{
    public class when_result_is_true_and_there_are_no_messages : WithFakes
    {
        static AutoMocker<HandleMapReturnCommandHandler> automocker = new NSubstituteAutoMocker<HandleMapReturnCommandHandler>();
        static HandleMapReturnCommand command;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            command = new HandleMapReturnCommand(-1, true, new SystemMessageCollection(), "activity", WorkflowMapCodeSectionEnum.OnEnter);
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_have_a_result_of_false = () =>
        {
            command.Result.ShouldEqual(true);
        };
    }
}