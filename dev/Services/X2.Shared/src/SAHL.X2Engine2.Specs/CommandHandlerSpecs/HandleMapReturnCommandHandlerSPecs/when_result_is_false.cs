using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.HandleMapReturnCommandHandlerSpecs
{
    public class when_result_is_false : WithFakes
    {
        static AutoMocker<HandleMapReturnCommandHandler> automocker = new NSubstituteAutoMocker<HandleMapReturnCommandHandler>();
        static HandleMapReturnCommand command;
        static Exception exception;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            messages = new SystemMessageCollection();
            messages.AddMessage(new SystemMessage("sample message", SystemMessageSeverityEnum.Error));
            command = new HandleMapReturnCommand(-1, false, messages, "activity", WorkflowMapCodeSectionEnum.OnExit);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => automocker.ClassUnderTest.HandleCommand(command, metadata));
        };

        It should_throw_a_map_returned_false_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(MapReturnedFalseException));
        };

        It exception_should_contain_the_messages_from_the_map = () =>
        {
            var mapReturnedFalseException = exception as MapReturnedFalseException;
            foreach (var message in messages.AllMessages)
            {
                mapReturnedFalseException.Messages.AllMessages.Contains(message).ShouldBeTrue();
            }
        };
    }
}
