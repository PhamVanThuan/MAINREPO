using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateIsComcorpApplicationLegacyEventQueryHandler : IServiceQueryHandler<CreateIsComcorpApplicationLegacyEventQuery>
    {
        public ISystemMessageCollection HandleQuery(CreateIsComcorpApplicationLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            query.Result = new ServiceQueryResult<IsComcorpApplicationLegacyEvent>(
                new List<IsComcorpApplicationLegacyEvent>() {
                    new IsComcorpApplicationLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, query.ADUserName, 
                        query.GenericKey)
                });

            return messages;
        }
    }
}