using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Credit;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.QueryHandlers
{
    public class CreateQACompletedLegacyEventQueryHandler : IServiceQueryHandler<CreateQACompletedLegacyEventQuery>
    {
        
        public ISystemMessageCollection HandleQuery(CreateQACompletedLegacyEventQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            query.Result = new ServiceQueryResult<QACompletedLegacyEvent>(
                new List<QACompletedLegacyEvent>() {
                    new QACompletedLegacyEvent(CombGuid.Instance.Generate(),query.Date, query.ADUserKey, query.ADUserName, query.GenericKey)
                });

            return messages;
        }
    }
}