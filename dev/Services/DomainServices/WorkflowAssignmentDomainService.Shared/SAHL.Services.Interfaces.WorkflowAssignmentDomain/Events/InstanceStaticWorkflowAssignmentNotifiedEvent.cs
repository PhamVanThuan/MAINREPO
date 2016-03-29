using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events
{
    public class InstanceStaticWorkflowAssignmentNotifiedEvent : Event
    {
        public long InstanceId { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public int GenericKey { get; protected set; }

        public string StaticGroupName { get; protected set; }

        public InstanceStaticWorkflowAssignmentNotifiedEvent(DateTime date, long instanceId, int genericKeyTypeKey, string staticGroupName, int genericKey) : base(date)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.StaticGroupName = staticGroupName;
            this.InstanceId = instanceId;
        }
    }
}