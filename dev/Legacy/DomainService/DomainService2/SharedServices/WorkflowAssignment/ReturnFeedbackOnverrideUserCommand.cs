namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReturnFeedbackOnverrideUserCommand : StandardDomainServiceCommand
    {
        public ReturnFeedbackOnverrideUserCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public string UserResult { get; set; }
    }
}