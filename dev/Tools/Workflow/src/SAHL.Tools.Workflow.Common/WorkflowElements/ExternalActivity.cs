using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class ExternalActivity : AbstractActivity
    {
        public ExternalActivity(string name, Single locationX, Single locationY, CodeSection onStartActivity, CodeSection onCompleteActivity, CodeSection onGetStageTransition,Guid x2ID)
            : base(name, locationX, locationY, onStartActivity, onCompleteActivity, onGetStageTransition, x2ID)
        {
        }

        public ExternalActivityDefinition InvokedBy { get; set; }

        public string ExternalActivityTarget { get; set; }
    }
}