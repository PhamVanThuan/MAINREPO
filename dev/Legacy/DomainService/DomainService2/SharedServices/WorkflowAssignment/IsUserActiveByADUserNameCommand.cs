namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsUserActiveByADUserNameCommand : StandardDomainServiceCommand
    {
        public IsUserActiveByADUserNameCommand(string adUserName)
        {
            this.ADUserName = adUserName;
        }

        public string ADUserName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}