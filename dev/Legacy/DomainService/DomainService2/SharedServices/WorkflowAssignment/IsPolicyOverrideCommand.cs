namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsPolicyOverrideCommand : StandardDomainServiceCommand
    {
        public IsPolicyOverrideCommand(long instanceID, long sourceInstanceID, int genericKey)
        {
            this.InstanceID = instanceID;
            this.SourceInstanceID = sourceInstanceID;
            this.GenericKey = genericKey;
        }

        public int GenericKey { get; set; }

        public long SourceInstanceID { get; set; }

        public long InstanceID { get; set; }

        public bool Result { get; set; }
    }
}