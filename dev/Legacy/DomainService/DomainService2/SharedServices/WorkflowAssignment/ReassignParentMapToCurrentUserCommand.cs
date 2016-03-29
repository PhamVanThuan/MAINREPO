namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignParentMapToCurrentUserCommand : StandardDomainServiceCommand
    {
        public ReassignParentMapToCurrentUserCommand(long instanceID, long sourceInstanceID, int applicationKey, string state, SAHL.Common.Globals.Process process)
        {
            this.InstanceID = instanceID;
            this.SourceInstanceID = sourceInstanceID;
            this.ApplicationKey = applicationKey;
            this.State = state;
            this.Process = process;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public long SourceInstanceID
        {
            get;
            protected set;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string State
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.Process Process
        {
            get;
            protected set;
        }
    }
}