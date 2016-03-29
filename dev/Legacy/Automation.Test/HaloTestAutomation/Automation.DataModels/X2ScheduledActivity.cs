using System;

namespace Automation.DataModels
{
    public class X2ScheduledActivity : IDataModel
    {
        public Int64 InstanceID { get; set; }

        public DateTime Time { get; set; }

        public X2Activity Activity { get; set; }

        public int ActivityID { get; set; }

        public int Priority { get; set; }

        public string WorkFlowProviderName { get; set; }

        public int ScheduledActivityID { get; set; }
    }
}