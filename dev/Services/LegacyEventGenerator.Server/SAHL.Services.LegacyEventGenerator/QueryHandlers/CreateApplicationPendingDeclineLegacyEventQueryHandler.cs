using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateApplicationPendingDeclineLegacyEventQueryHandler : IServiceQueryHandler<CreateApplicationPendingDeclineLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;
        
        public CreateApplicationPendingDeclineLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateApplicationPendingDeclineLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<ApplicationDeclinePendedLegacyEvent>(
                new List<ApplicationDeclinePendedLegacyEvent>() {
                    new ApplicationDeclinePendedLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, query.ADUserName, 
                        query.GenericKey, model.OfferInformationKey, null, null)
                });

            return messages;
        }
    }
}