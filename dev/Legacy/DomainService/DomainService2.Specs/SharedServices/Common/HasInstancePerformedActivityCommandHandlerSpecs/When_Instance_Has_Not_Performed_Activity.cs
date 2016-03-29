using System;
using DomainService2.SharedServices;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.HasInstancePerformedActivityCommandHandlerSpecs
{
    [Subject(typeof(HasInstancePerformedActivityCommandHandler))]
    public class When_Instance_Has_Not_Performed_Activity : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static HasInstancePerformedActivityCommand command;
        protected static HasInstancePerformedActivityCommandHandler handler;
        protected static IX2WorkflowService x2WorkflowService;

        Establish context = () =>
        {
            x2WorkflowService = An<IX2WorkflowService>();
            x2WorkflowService.WhenToldTo(x => x.HasInstancePerformedActivity(Param<Int64>.IsAnything, Param<String>.IsAnything)).Return(false);

            command = new HasInstancePerformedActivityCommand(1, "string");
            handler = new HasInstancePerformedActivityCommandHandler(x2WorkflowService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}