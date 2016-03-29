using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.Registration;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateRegistrationsLoanRegisteredLegacyEventQueryHandler : IServiceQueryHandler<CreateRegistrationsLoanRegisteredLegacyEventQuery>
    {
        private ILegacyEventDataService legacyEventDataService;

        public CreateRegistrationsLoanRegisteredLegacyEventQueryHandler(ILegacyEventDataService legacyEventDataService)
        {
            this.legacyEventDataService = legacyEventDataService;
        }

        public ISystemMessageCollection HandleQuery(CreateRegistrationsLoanRegisteredLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var model = legacyEventDataService.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey);

            query.Result = new ServiceQueryResult<RegistrationsLoanRegisteredLegacyEvent>(
                new List<RegistrationsLoanRegisteredLegacyEvent>() {
                    new RegistrationsLoanRegisteredLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, 
                        query.ADUserName, query.GenericKey, model.OfferInformationKey, model.AssignedADUser, model.CommissionableADUser)
                });

            return messages;
        }
    }
}
