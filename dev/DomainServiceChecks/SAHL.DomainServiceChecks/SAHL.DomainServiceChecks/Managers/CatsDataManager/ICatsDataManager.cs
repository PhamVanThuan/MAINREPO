using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.Managers.CatsDataManager
{
    public interface ICatsDataManager
    {
        bool DoesCATSPaymentBatchExist(int thirdPaymentBatchKey);
    }
}