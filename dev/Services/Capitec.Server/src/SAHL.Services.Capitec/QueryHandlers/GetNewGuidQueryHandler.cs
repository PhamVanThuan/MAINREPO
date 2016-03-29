using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryHandlers
{
    public class GetNewGuidQueryHandler : IServiceQueryHandler<GetNewGuidQuery>
    {
        public ISystemMessageCollection HandleQuery(GetNewGuidQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            query.Result = new GetNewGuidQueryResult() { NewGuid = CombGuid.Instance.Generate() };

            return messages;
        }
    }
}