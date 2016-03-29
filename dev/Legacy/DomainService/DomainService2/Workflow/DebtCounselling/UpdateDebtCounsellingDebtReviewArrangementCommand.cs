namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateDebtCounsellingDebtReviewArrangementCommand : StandardDomainServiceCommand
    {
        public int AccountKey { get; set; }

        public string UserID { get; set; }

        public UpdateDebtCounsellingDebtReviewArrangementCommand(int accountKey, string userID)
        {
            this.AccountKey = accountKey;
            this.UserID = userID;
        }

        public bool Result { get; set; }
    }
}