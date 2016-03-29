namespace SAHL.X2Engine2.ViewModels
{
    public class Activity
    {
        public int ActivityID { get; set; }

        public int? StateId { get; set; }

        public string StateName { get; set; }

        public int ToStateId { get; set; }

        public string ActivityName { get; set; }

        public string ToStateName { get; set; }

        public int? RaiseExternalActivityId { get; set; }

        public int WorkflowId { get; set; }

        public int Priority { get; set; }

        public bool SplitWorkflow { get; set; }

        public Activity(int activityId, string activityName, int? stateId, string stateName, int toStateId, string toStateName, int workflowId,
            bool splitWorkflow, int? raiseExternalActivityId = null, int priority = 1)
        {
            this.ActivityID = activityId;
            this.StateName = stateName;
            this.StateId = stateId;
            this.ToStateId = toStateId;
            this.ActivityName = activityName;
            this.ToStateName = toStateName;
            this.WorkflowId = workflowId;
            this.RaiseExternalActivityId = raiseExternalActivityId;
            this.Priority = priority;
            this.SplitWorkflow = splitWorkflow;
        }
    }
}