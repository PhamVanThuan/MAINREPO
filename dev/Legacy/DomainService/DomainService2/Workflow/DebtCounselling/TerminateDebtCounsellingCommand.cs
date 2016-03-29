namespace DomainService2.Workflow.DebtCounselling
{
    public class TerminateDebtCounsellingCommand : StandardDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public string UserID { get; set; }

        public TerminateDebtCounsellingCommand(int debtCounsellingKey, string userID)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.UserID = userID;
        }

        public bool Result { get; set; }
    }
}