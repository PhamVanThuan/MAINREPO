using System;

namespace Automation.DataModels
{
    public sealed class X2WorkflowHistory
    {
        public Int64 InstanceID { get; set; }

        public int StateID { get; set; }

        public int ActivityID { get; set; }

        public int CreatorADUserName { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? StateChangeDate { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public DateTime? ActivityDate { get; set; }

        public string ADUserName { get; set; }

        public int Priority { get; set; }
    }
}