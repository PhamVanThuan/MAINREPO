using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class Instance
    {
        public Instance()
        {
        }

        public virtual long Id { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual Instance ParentInstance { get; set; }

        public virtual State State { get; set; }

        public virtual string Name { get; set; }

        public virtual string Subject { get; set; }

        public virtual string WorkFlowProvider { get; set; }

        public virtual string CreatorADUserName { get; set; }

        public virtual System.DateTime CreationDate { get; set; }

        public virtual System.Nullable<System.DateTime> StateChangeDate { get; set; }

        public virtual System.Nullable<System.DateTime> DeadlineDate { get; set; }

        public virtual System.Nullable<System.DateTime> ActivityDate { get; set; }

        public virtual string ActivityADUserName { get; set; }

        public virtual System.Nullable<int> ActivityID { get; set; }

        public virtual System.Nullable<int> Priority { get; set; }

        public virtual System.Nullable<long> SourceInstanceID { get; set; }

        public virtual System.Nullable<int> ReturnActivityID { get; set; }
    }
}