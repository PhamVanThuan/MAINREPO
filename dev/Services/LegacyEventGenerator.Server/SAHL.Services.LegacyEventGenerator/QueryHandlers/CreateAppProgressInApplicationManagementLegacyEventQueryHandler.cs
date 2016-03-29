using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateAppProgressInApplicationManagementLegacyEventQueryHandler : IServiceQueryHandler<CreateAppProgressInApplicationManagementLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateAppProgressInApplicationManagementLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateAppProgressInApplicationManagementLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<AppProgressInApplicationManagementLegacyEvent>(
                new List<AppProgressInApplicationManagementLegacyEvent>() {
                    new AppProgressInApplicationManagementLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey,
                        query.ADUserName, query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser,
                        AppProgressEnum.ManageApplication)
                });

            return messages;
        }
    }
}