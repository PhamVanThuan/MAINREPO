namespace SAHL.X2Engine2.ViewModels
{
    public class WorkflowActivity
    {
        public WorkflowActivity()
        {
        }

        public int ID { get; set; }

        public string CurrentWorkflowName { get; set; }

        public string DestinationWorkflowName { get; set; }

        public int ActivityIDInDestinationMap { get; set; }

        public int? ReturnActivityId { get; set; }

        public string ActivityName { get; set; }

        public WorkflowActivity(int id, string currentWorkflowName, string destinationWorkflowName, int activityIdInDestinationMap, int? returnActivityId, string activityName)
        {
            this.ID = id;
            this.CurrentWorkflowName = currentWorkflowName;
            this.DestinationWorkflowName = destinationWorkflowName;
            this.ActivityIDInDestinationMap = activityIdInDestinationMap;
            this.ReturnActivityId = returnActivityId;
            this.ActivityName = activityName;
        }
    }
}