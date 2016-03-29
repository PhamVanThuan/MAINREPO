using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IBatchServiceRepository
    {
        IBatchService GetEmptyBatchService();

        void SaveBatchService(IBatchService batchService);

        IBatchService GetBatchService(int batchServiceKey);
    }
}
