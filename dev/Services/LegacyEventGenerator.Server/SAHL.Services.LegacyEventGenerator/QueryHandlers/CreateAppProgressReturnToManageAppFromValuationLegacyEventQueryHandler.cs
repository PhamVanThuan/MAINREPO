using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateAppProgressReturnToManageAppFromValuationLegacyEventQueryHandler : IServiceQueryHandler<CreateAppProgressReturnToManageAppFromValuationLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateAppProgressReturnToManageAppFromValuationLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateAppProgressReturnToManageAppFromValuationLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);
            
            query.Result = new ServiceQueryResult<AppProgressReturnToManageAppFromValuationLegacyEvent>(
                new List<AppProgressReturnToManageAppFromValuationLegacyEvent>() {
                    new AppProgressReturnToManageAppFromValuationLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, 
                        query.ADUserName, query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser, 
                        AppProgressEnum.Valuations)
                });

            return messages;
        }
    }
}