using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public class BatchServiceConfiguration : IBatchServiceConfiguration
    {
        public BatchServiceConfiguration()
        {
            this.NumberOfTimesToRetryToProcessTheMessage = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfTimesToRetryToProcessTheMessage"]);
            this.NumberOfAttemptsToRetryToProcessTheMessage = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfAttemptsToRetryToProcessTheMessage"]);
            this.TimeOutIntervalToReloadFailedMessages = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutIntervalToReloadFailedMessages"]);
        }

        public int NumberOfTimesToRetryToProcessTheMessage
        {
            get;
            protected set;
        }

        public int NumberOfAttemptsToRetryToProcessTheMessage
        {
            get;
            protected set;
        }

        public int TimeOutIntervalToReloadFailedMessages
        {
            get;
            protected set;
        }
    }
}
