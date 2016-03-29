using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateApplicationNTUFinalizedLegacyEventQueryHandler : IServiceQueryHandler<CreateApplicationNTUFinalizedLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateApplicationNTUFinalizedLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateApplicationNTUFinalizedLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<ApplicationNTUFinalizedLegacyEvent>(
                new List<ApplicationNTUFinalizedLegacyEvent>() {
                    new ApplicationNTUFinalizedLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, query.ADUserName, query.GenericKey, 
                        model.OfferInformationKey, null, null)
                });

            return messages;
        }
    }
}