using SAHL.Core.Events;
using System;

namespace SAHL.Core.X2.Events
{
    public class WorkflowTransitionEvent : Event, IWorkflowEvent
    {
        public long InstanceId { get; protected set; }

        public string ProcessName { get; protected set; }

        public string WorkflowName { get; protected set; }

        public DateTime ActivityDate { get { return base.Date; } }

        public int GenericKey { get; protected set; }

        public int? GenericKeyTypeKey { get; protected set; }

        public string WorkflowActivity { get; protected set; }

        public int? FromStateId { get; protected set; }

        public int? ToStateId { get; protected set; }

        public string FromStateName { get; protected set; }

        public string ToStateName { get; protected set; }

        public string AdUserName { get; protected set; }

        public WorkflowTransitionEvent(
              long instanceId
            , string adUserName
            , string processName
            , string workflowName
            , int genericKey
            , int? genericKeyTypeKey
            , string workflowActivity
            , DateTime activityDate
            , int? fromStateId
            , int? toStateId
            , string fromStateName
            , string toStateName
            )
            : base(activityDate)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.InstanceId = instanceId;
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
            this.WorkflowActivity = workflowActivity;
            this.FromStateId = fromStateId;
            this.ToStateId = toStateId;
            this.FromStateName = fromStateName;
            this.ToStateName = toStateName;
            this.AdUserName = adUserName;
        }
    }
}