namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class StageActivity
    {
        public StageActivity() { }

        public virtual int Id { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual System.Nullable<int> StageDefinitionKey { get; set; }

        public virtual System.Nullable<int> StageDefinitionStageDefinitionGroupKey { get; set; }
    }
}