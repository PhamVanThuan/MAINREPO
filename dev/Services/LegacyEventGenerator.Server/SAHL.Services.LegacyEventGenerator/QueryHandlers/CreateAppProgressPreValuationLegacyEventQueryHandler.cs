using System.Collections.Generic;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateAppProgressPreValuationLegacyEventQueryHandler: IServiceQueryHandler<CreateAppProgressPreValuationLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateAppProgressPreValuationLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateAppProgressPreValuationLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<AppProgressPreValuationLegacyEvent>(
                new List<AppProgressPreValuationLegacyEvent>() {
                    new AppProgressPreValuationLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, query.ADUserName, 
                        query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser)
                });

            return messages;
        }
    }
}
