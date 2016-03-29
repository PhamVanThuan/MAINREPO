using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.QueryHandlers
{
    public class GetNewThirdPartyPaymentBatchReferenceQueryHandler : IServiceQueryHandler<GetNewThirdPartyPaymentBatchReferenceQuery>
    {
        public ICATSDataManager catsDataManager { get; protected set; }

        public GetNewThirdPartyPaymentBatchReferenceQueryHandler(ICATSDataManager catsDataManager)
        {
            this.catsDataManager = catsDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetNewThirdPartyPaymentBatchReferenceQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            var result = new List<GetNewThirdPartyPaymentBatchReferenceQueryResult>() {  
                new GetNewThirdPartyPaymentBatchReferenceQueryResult { BatchKey = catsDataManager.GetNewThirdPartyPaymentBatchReference(query.BatchType) } 
            };
            query.Result = new ServiceQueryResult<GetNewThirdPartyPaymentBatchReferenceQueryResult>(result);

            if (result.First().BatchKey == 0)
            {
                messages.AddMessage(new SystemMessage("Failed to retrieve the batch key", SystemMessageSeverityEnum.Error));
            }
            return messages;
        }
    }
}
