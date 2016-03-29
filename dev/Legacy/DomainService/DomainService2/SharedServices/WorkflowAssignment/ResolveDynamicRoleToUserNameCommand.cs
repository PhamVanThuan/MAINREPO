namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ResolveDynamicRoleToUserNameCommand : StandardDomainServiceCommand
    {
        public ResolveDynamicRoleToUserNameCommand(string dynamicRole, long instanceID)
        {
            this.DynamicRole = dynamicRole;
            this.InstanceID = instanceID;
        }

        public string DynamicRole { get; set; }

        public long InstanceID { get; set; }

        public string ADUserNameResult { get; set; }
    }
}