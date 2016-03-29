namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetCurrentConsultantAndAdminCommand : StandardDomainServiceCommand
    {
        public GetCurrentConsultantAndAdminCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public System.Data.DataTable Result
        {
            get;
            set;
        }
    }
}