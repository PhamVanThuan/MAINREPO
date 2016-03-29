using System.Collections.Generic;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.Registration;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateRegistrationInstructAttorneyLegacyEventQueryHandler : IServiceQueryHandler<CreateRegistrationsInstructAttorneyLegacyEventQuery>
    {

        private ILegacyEventDataService legacyEventDataService;

        public CreateRegistrationInstructAttorneyLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateRegistrationsInstructAttorneyLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<RegistrationsInstructAttorneyLegacyEvent>(
                new List<RegistrationsInstructAttorneyLegacyEvent>() {
                    new RegistrationsInstructAttorneyLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, 
                        query.ADUserName, query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser)
                });

            return messages;
        }
    }
}