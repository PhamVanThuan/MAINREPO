using SAHL.Services.Interfaces.PollingManager;
using System.Collections.Specialized;

namespace SAHL.Services.PollingManager.Settings
{
    public class LossControlPolledHandlerSettings : IPolledHandlerSettings
    {
        private NameValueCollection nameValueCollection;
        public LossControlPolledHandlerSettings(NameValueCollection  nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
        }

        public int TimerInterval
        {
            get
            {
                string settingsValue = nameValueCollection["TimerInterval"];
                int timerInterval;
                if (!int.TryParse(settingsValue, out timerInterval))
                {
                    timerInterval = 1000;
                }
                return timerInterval;
            }
        }

        public int ProcessingSetSize 
        {
            get
            {
                string settingsValue = nameValueCollection["ProcessingSetSize"];
                int processingSetSize;
                if (!int.TryParse(settingsValue, out processingSetSize))
                {
                    processingSetSize = 1;
                }
                return processingSetSize;
            }
        }
    }
}