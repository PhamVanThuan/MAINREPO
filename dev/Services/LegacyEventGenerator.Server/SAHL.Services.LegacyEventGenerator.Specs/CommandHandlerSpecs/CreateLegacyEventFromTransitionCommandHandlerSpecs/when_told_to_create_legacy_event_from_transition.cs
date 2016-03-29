using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LegacyEventGenerator.Commands;
using SAHL.Services.LegacyEventGenerator.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LegacyEventGenerator.Specs.CommandHandlerSpecs.CreateLegacyEventFromTransitionCommandHandlerSpecs
{
    public class when_told_to_create_legacy_event_from_transition : WithCoreFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateLegacyEventFromTransitionCommand command;
        private static CreateLegacyEventFromTransitionCommandHandler handler;

        private static DateTime eventDate;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            command = new CreateLegacyEventFromTransitionCommand(1234);
            handler = new CreateLegacyEventFromTransitionCommandHandler();
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

    }
}
