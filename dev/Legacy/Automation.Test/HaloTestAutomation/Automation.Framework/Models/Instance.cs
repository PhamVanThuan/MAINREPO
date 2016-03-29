using System;
using System.Collections.Generic;

namespace Automation.Framework.Models
{
    public class Instance
    {
        public long ID { get; set; }

        public int WorkflowID { get; set; }

        public long ParentInstanceID { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string WorkflowProvider { get; set; }

        public string StateName { get; set; }

        public string CreatorADUserName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StateChangeDate { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime ActivityDate { get; set; }

        public string ActivityADUserName { get; set; }

        public int ActivityID { get; set; }

        public int Priority { get; set; }

        public long SourceInstanceID { get; set; }

        public int ReturnActivityID { get; set; }

        public List<Activity> ActivitySecurity { get; set; }

        public int StateID { get; set; }

        public List<Timers> ClonedInstanceTimers { get; set; }

    }
}