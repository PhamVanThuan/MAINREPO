namespace DomainService2.Workflow.DebtCounselling
{
    public class GetEworkDataForLossControlCaseCommand : StandardDomainServiceCommand
    {
        public int AccountKey { get; set; }

        public string eStageName { get; set; } // output parameter

        public string eFolderID { get; set; } // output parameter

        public string eADUserName { get; set; } // output parameter

        public GetEworkDataForLossControlCaseCommand(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}