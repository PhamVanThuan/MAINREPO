﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Commands;
using SAHL.Services.LegacyEventGenerator.CommandHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.CommandHandlerSpecs.CreateLegacyEventFromCompositeCommandHandlerSpecs
{
    public class when_told_to_create_legacy_event_from_composite_with_invalid_STCKey : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateLegacyEventFromCompositeCommand command;
        private static CreateLegacyEventFromCompositeCommandHandler handler;

        private static ILegacyEventDataService legacyEventDataService;
        private static ILegacyEventQueryMappingService legacyEventMappingService;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IEventRaiser eventRaiser;
        private static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();
            legacyEventMappingService = An<ILegacyEventQueryMappingService>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            eventRaiser = An<IEventRaiser>();

            messages = new SystemMessageCollection();
            command = new CreateLegacyEventFromCompositeCommand(-1);
            handler = new CreateLegacyEventFromCompositeCommandHandler(legacyEventDataService, legacyEventMappingService, serviceQueryRouter, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_any_error_messages = () =>
        {
            messages.HasWarnings.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.AllMessages.Contains<ISystemMessage>(new SystemMessage("Could not get composite data", SystemMessageSeverityEnum.Warning));
        };
    }
}
