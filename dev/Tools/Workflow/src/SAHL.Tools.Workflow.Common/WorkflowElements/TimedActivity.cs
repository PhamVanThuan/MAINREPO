using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class TimedActivity : AbstractActivity
    {
        public TimedActivity(string name, Single locationX, Single locationY, CodeSection onStartActivity, CodeSection onCompleteActivity, CodeSection onGetStageTransition, CodeSection onGetActivityTime,Guid x2ID)
            : base(name, locationX, locationY, onStartActivity, onCompleteActivity, onGetStageTransition, x2ID)
        {
            this.GetActivityTimeCode = onGetActivityTime;
            base.AddCodeSection(this.GetActivityTimeCode);
        }

        public CodeSection GetActivityTimeCode { get; protected set; }
    }
}