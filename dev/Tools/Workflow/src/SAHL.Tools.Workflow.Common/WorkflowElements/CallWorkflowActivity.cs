using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class CallWorkflowActivity : AbstractActivity
    {
        public CallWorkflowActivity(string name, Single locationX, Single locationY, CodeSection onStartActivity, CodeSection onCompleteActivity, CodeSection onGetStageTransition,Guid x2ID)
            : base(name, locationX, locationY, onStartActivity, onCompleteActivity, onGetStageTransition, x2ID)
        {
        }

        public Workflow WorkflowToCall { get; set; }

        public AbstractActivity ActivityToCall { get; set; }

        public AbstractActivity ReturnActivity { get; set; }
    }
}