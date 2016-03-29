namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class DeactivateAllWorkflowRoleAssigmentsForInstanceCommand : StandardDomainServiceCommand
    {
        public DeactivateAllWorkflowRoleAssigmentsForInstanceCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID
        {
            get;
            protected set;
        }
    }
}