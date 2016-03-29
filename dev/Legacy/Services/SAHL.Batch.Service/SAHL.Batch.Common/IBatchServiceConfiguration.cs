using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public interface IBatchServiceConfiguration
    {
        int NumberOfTimesToRetryToProcessTheMessage { get; }

        int NumberOfAttemptsToRetryToProcessTheMessage { get; }

        int TimeOutIntervalToReloadFailedMessages { get; }
    }
}
