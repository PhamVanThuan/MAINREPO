namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class AddAccountMemoMessageOnReceiptOfApplicationCommand : StandardDomainServiceCommand
    {
        public AddAccountMemoMessageOnReceiptOfApplicationCommand(int applicationKey, string adUser, string assignedTo)
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = adUser;
            this.AssignedTo = assignedTo;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string ADUser
        {
            get;
            protected set;
        }

        public string AssignedTo
        {
            get;
            protected set;
        }
    }
}