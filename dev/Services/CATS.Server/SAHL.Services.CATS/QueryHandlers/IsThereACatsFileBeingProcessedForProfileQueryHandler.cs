using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;

namespace SAHL.Services.CATS.QueryHandlers
{
    public class IsThereACatsFileBeingProcessedForProfileQueryHandler : IServiceQueryHandler<IsThereACatsFileBeingProcessedForProfileQuery>
    {
        private ICATSManager catsHelper;
        private ICATSDataManager catsDataManager;

        public IsThereACatsFileBeingProcessedForProfileQueryHandler(ICATSManager catsHelper, ICATSDataManager catsDataManager)
        {
            this.catsHelper = catsHelper;
            this.catsDataManager = catsDataManager;
        }

        public ISystemMessageCollection HandleQuery(IsThereACatsFileBeingProcessedForProfileQuery query)
        {
            var batchType = catsDataManager.GetBatchTypeByKey(query.CatsBatchTypeKey);
            var IsThereACatsFileBeingProcessedForProfile = catsHelper.IsThereACatsFileBeingProcessedForProfile(batchType.CATSProfile);
            query.Result = new ServiceQueryResult<bool>(new List<bool> { IsThereACatsFileBeingProcessedForProfile });
            return SystemMessageCollection.Empty();
        }
    }
}