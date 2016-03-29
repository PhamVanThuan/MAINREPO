namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsUserActiveByADUserKeyCommand : StandardDomainServiceCommand
    {
        public IsUserActiveByADUserKeyCommand(int adUserKey)
        {
            this.ADUserKey = adUserKey;
        }

        public int ADUserKey
        {
            get;
            set;
        }

        public bool Result { get; set; }
    }
}