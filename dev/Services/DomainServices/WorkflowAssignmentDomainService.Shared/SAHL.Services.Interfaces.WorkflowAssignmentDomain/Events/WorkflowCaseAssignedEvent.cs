using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events
{
    public class WorkflowCaseAssignedEvent : Event
    {
        public long InstanceId { get; private set; }
        public int UserOrganisationStructureKey { get; private set; }
        public int CapabilityKey { get; private set; }
        public int GenericKeyTypeKey { get; private set; }
        public int GenericKey { get; private set; }

        public WorkflowCaseAssignedEvent(DateTime date, int genericKeyTypeKey, int genericKey, long instanceId, int userOrganisationStructureKey, int capabilityKey) : base(date)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.InstanceId = instanceId;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.CapabilityKey = capabilityKey;
        }
    }
}
