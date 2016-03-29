using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkFlow
    {
        public WorkFlow()
        {
            Activities = new List<Activity>();
            Forms = new List<Form>();
            SecurityGroups = new List<SecurityGroup>();
            States = new List<State>();
            ExternalActivities = new List<ExternalActivity>();
            CallWorkFlowActivities = new List<WorkFlowActivity>();
        }

        public virtual int Id { get; set; }

        public virtual Process Process { get; set; }

        public virtual WorkFlow WorkFlowAncestor { get; set; }

        public virtual int WorkFlowIconId { get; set; }

        public virtual IList<Activity> Activities { get; set; }

        public virtual IList<WorkFlowActivity> CallWorkFlowActivities { get; set; }

        public virtual IList<ExternalActivity> ExternalActivities { get; set; }

        public virtual IList<Form> Forms { get; set; }

        public virtual IList<SecurityGroup> SecurityGroups { get; set; }

        public virtual IList<State> States { get; set; }

        public virtual string Name { get; set; }

        public virtual System.DateTime CreateDate { get; set; }

        public virtual string StorageTable { get; set; }

        public virtual string StorageKey { get; set; }

        public virtual string DefaultSubject { get; set; }

        public virtual System.Nullable<int> GenericKeyTypeKey { get; set; }
    }
}