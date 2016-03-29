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
    public class CreateAppProgressInValuationLegacyEventQueryHandler : IServiceQueryHandler<CreateAppProgressInValuationLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateAppProgressInValuationLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateAppProgressInValuationLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<AppProgressInValuationLegacyEvent>(
                new List<AppProgressInValuationLegacyEvent>() {
                    new AppProgressInValuationLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, query.ADUserName, 
                        query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser,
                        AppProgressEnum.Valuations)
                });

            return messages;
        }
    }
}