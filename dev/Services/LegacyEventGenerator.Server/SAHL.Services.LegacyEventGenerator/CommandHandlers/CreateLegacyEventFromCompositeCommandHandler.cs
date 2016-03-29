using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Commands;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models;

namespace SAHL.Services.LegacyEventGenerator.CommandHandlers
{
    public class CreateLegacyEventFromCompositeCommandHandler : IServiceCommandHandler<CreateLegacyEventFromCompositeCommand>
    {
        private ILegacyEventDataService legacyEventDataService;
        private ILegacyEventQueryMappingService legacyEventMappingService;
        private IServiceQueryRouter serviceQueryRouter;
        private IEventRaiser eventRaiser;

        public CreateLegacyEventFromCompositeCommandHandler(ILegacyEventDataService legacyEventDataService,
            ILegacyEventQueryMappingService legacyEventMappingService,
            IServiceQueryRouter serviceQueryRouter,
            IEventRaiser eventRaiser)
        {
            this.legacyEventDataService = legacyEventDataService;
            this.legacyEventMappingService = legacyEventMappingService;
            this.serviceQueryRouter = serviceQueryRouter;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(CreateLegacyEventFromCompositeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var stageTransitionCompositeData = legacyEventDataService.GetModelByStageTransitionCompositeKey(command.StageTransitionCompositeKey);
            if (stageTransitionCompositeData == null)
            {
                messages.AddMessage(new SystemMessage("Could not get composite data", SystemMessageSeverityEnum.Warning));
                return messages;
            }

            dynamic legacyEventQuery = legacyEventMappingService.GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(stageTransitionCompositeData.StageDefinitionStageDefinitionGroupKey);
            if (legacyEventQuery == null)
            {
                messages.AddMessage(new SystemMessage("Could not get legacy event query", SystemMessageSeverityEnum.Warning));
                return messages;
            }

            legacyEventQuery.Initialise(command.StageTransitionCompositeKey,
                                        stageTransitionCompositeData.GenericKey,
                                        stageTransitionCompositeData.GenericKeyTypeKey,
                                        stageTransitionCompositeData.TransitionDate,
                                        stageTransitionCompositeData.ADUserKey,
                                        stageTransitionCompositeData.ADUserName);

            messages = serviceQueryRouter.HandleQuery((object)legacyEventQuery);
            if (legacyEventQuery.Result == null || legacyEventQuery.Result.Results == null || legacyEventQuery.Result.Results.Count == 0 || messages.HasErrors)
            {
                messages.AddMessage(new SystemMessage("Could not get legacy event query result", SystemMessageSeverityEnum.Warning));
                return messages;
            }

            Event result = legacyEventQuery.Result.Results[0];

            eventRaiser.RaiseEvent(legacyEventQuery.Date, result, legacyEventQuery.GenericKey, legacyEventQuery.GenericKeyTypeKey, metadata);

            return messages;
        }
    }
}