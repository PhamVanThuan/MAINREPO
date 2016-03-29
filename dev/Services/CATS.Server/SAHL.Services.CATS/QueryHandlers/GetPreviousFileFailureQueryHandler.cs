using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;

namespace SAHL.Services.CATS.QueryHandlers
{
    public class GetPreviousFileFailureQueryHandler : IServiceQueryHandler<GetPreviousFileFailureQuery>
    {
        private ICATSDataManager catsDataManager;
        private ICATSManager catsHelper;

        public GetPreviousFileFailureQueryHandler(ICATSDataManager catsDataManager, ICATSManager catsHelper)
        {
            this.catsDataManager = catsDataManager;
            this.catsHelper = catsHelper;
        }

        public ISystemMessageCollection HandleQuery(GetPreviousFileFailureQuery query)
        {
            var lastProcessedBath = catsDataManager.GetLastProcessedBatch(query.CATSPaymentBatchType);
            var failedFileList = new List<GetPreviousFileFailureQueryResult>();

            if (lastProcessedBath != null)
            {
                if (catsHelper.HasFileFailedProcessing(lastProcessedBath.CATSFileName))
                {
                    failedFileList.Add(new GetPreviousFileFailureQueryResult
                    {
                        CATSFileName = lastProcessedBath.CATSFileName,
                        CATSPaymentBatchStatusKey = lastProcessedBath.CATSPaymentBatchStatusKey,
                        CATSPaymentBathKey = lastProcessedBath.CATSPaymentBatchKey,
                        BatchCreationDate = lastProcessedBath.CreatedDate,
                        BatchProcessDate = lastProcessedBath.ProcessedDate
                    });
                }
            }
            query.Result = new ServiceQueryResult<GetPreviousFileFailureQueryResult>(failedFileList);

            return SystemMessageCollection.Empty();
        }
    }
}