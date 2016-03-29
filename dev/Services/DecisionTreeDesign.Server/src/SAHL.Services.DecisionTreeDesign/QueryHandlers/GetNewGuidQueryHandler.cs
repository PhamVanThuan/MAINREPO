using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryHandlers
{
    public class GetNewGuidQueryHandler : IServiceQueryHandler<GetNewGuidQuery>
    {
        private ICombGuid combGuidGenerator;

        public GetNewGuidQueryHandler(ICombGuid combGuidGenerator)
        {
            this.combGuidGenerator = combGuidGenerator;
        }

        public ISystemMessageCollection HandleQuery(GetNewGuidQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var result = new GetNewGuidQueryResult();
            result.Id = this.combGuidGenerator.Generate();
            query.Result = new ServiceQueryResult<GetNewGuidQueryResult>(new GetNewGuidQueryResult[] { result });

            return messages;
        }
    }
}