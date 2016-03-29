using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Commands;
using SAHL.Services.LegacyEventGenerator.CommandHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models;
using SAHL.Services.LegacyEventGenerator.Specs.Stubs;
using System;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.Specs.CommandHandlerSpecs.CreateLegacyEventFromCompositeCommandHandlerSpecs
{
    public class when_told_to_create_legacy_event_from_composite : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateLegacyEventFromCompositeCommand command;
        private static CreateLegacyEventFromCompositeCommandHandler handler;
        private static ILegacyEventDataService legacyEventDataService;
        private static ILegacyEventQueryMappingService legacyEventMappingService;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IEventRaiser eventRaiser;
        private static StageTransitionCompositeEventDataModel stageTransitionCompositeEventDataModel;
        private static FakeLegacyEventQuery fakeLegacyEventQuery;
        private static ServiceRequestMetadata metadata;
        private static DateTime eventDate;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();
            legacyEventMappingService = An<ILegacyEventQueryMappingService>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            eventRaiser = An<IEventRaiser>();

            messages = new SystemMessageCollection();
            command = new CreateLegacyEventFromCompositeCommand(1234);
            handler = new CreateLegacyEventFromCompositeCommandHandler(legacyEventDataService, legacyEventMappingService, serviceQueryRouter, eventRaiser);
            metadata = new ServiceRequestMetadata();
            eventDate = DateTime.Now;

            stageTransitionCompositeEventDataModel = new StageTransitionCompositeEventDataModel()
            {
                GenericKey = 1234567,
                GenericKeyTypeKey = 2,
                TransitionDate = eventDate,
                ADUserKey = 123,
                ADUserName = "bobs",
                StageDefinitionStageDefinitionGroupKey = 0
            };
            legacyEventDataService
                .WhenToldTo(x => x.GetModelByStageTransitionCompositeKey(command.StageTransitionCompositeKey))
                .Return(stageTransitionCompositeEventDataModel);

            fakeLegacyEventQuery = new FakeLegacyEventQuery();

            fakeLegacyEventQuery.Result = new ServiceQueryResult<FakeLegacyEvent>(
                new List<FakeLegacyEvent>() {
                    new FakeLegacyEvent(Guid.NewGuid(),eventDate, 1234567, "sahl\bobs", 1)
                });
            legacyEventMappingService.WhenToldTo(x => x.GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(Param.IsAny<int>())).Return(fakeLegacyEventQuery);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_raise_the_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(eventDate, Param.IsAny<IEvent>(), 1234567, 2, metadata));
        };
    }
}