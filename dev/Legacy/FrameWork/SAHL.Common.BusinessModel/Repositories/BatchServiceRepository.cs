using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IBatchServiceRepository))]
    public class BatchServiceRepository : AbstractRepositoryBase, IBatchServiceRepository
    {
        public IBatchService GetEmptyBatchService()
        {
            return base.CreateEmpty<IBatchService, BatchService_DAO>();
        }

        public void SaveBatchService(IBatchService batchService)
        {
            base.Save<IBatchService, BatchService_DAO>(batchService);
        }

        public IBatchService GetBatchService(int batchServiceKey)
        {
            return base.GetByKey<IBatchService, BatchService_DAO>(batchServiceKey);
        }
    }
}
