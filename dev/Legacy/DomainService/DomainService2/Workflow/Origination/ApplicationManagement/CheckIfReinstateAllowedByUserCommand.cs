namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckIfReinstateAllowedByUserCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public string PreviousState { get; set; }

        public string ADUserName { get; set; }

        public CheckIfReinstateAllowedByUserCommand(int applicationKey, string previousState, bool IgnoreWarnings, string adUserName)
        {
            this.ApplicationKey = applicationKey;
            this.PreviousState = previousState;
            this.IgnoreWarnings = IgnoreWarnings;
            this.ADUserName = adUserName;
        }
    }
}