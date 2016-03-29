using System.Collections.Generic;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.BusinessApplicationProcess;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateNewBusinessApplicationProgressDisbursedLegacyEventQueryHandler : IServiceQueryHandler<CreateNewBusinessApplicationProgressDisbursedEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateNewBusinessApplicationProgressDisbursedLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateNewBusinessApplicationProgressDisbursedEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<NewBusinessApplicationProgressDisbursedLegacyEvent>(
                new List<NewBusinessApplicationProgressDisbursedLegacyEvent>() {
                    new NewBusinessApplicationProgressDisbursedLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, 
                        query.ADUserName, query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser)
                });

            return messages;
        }
    }
}