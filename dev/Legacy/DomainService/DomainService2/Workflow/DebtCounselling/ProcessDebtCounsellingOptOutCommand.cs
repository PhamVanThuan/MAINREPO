namespace DomainService2.Workflow.DebtCounselling
{
    public class ProcessDebtCounsellingOptOutCommand : StandardDomainServiceCommand
    {
        public ProcessDebtCounsellingOptOutCommand(int accountKey, string userID)
        {
            this.AccountKey = accountKey;
            this.UserID = userID;
        }

        public int AccountKey { get; set; }

        public string UserID { get; set; }

        public bool Result { get; set; }
    }
}