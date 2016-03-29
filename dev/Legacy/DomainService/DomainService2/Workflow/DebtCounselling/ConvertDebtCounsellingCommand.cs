namespace DomainService2.Workflow.DebtCounselling
{
    public class ConvertDebtCounsellingCommand : StandardDomainServiceCommand
    {
        public int AccountKey { get; set; }

        public string UserID { get; set; }

        public ConvertDebtCounsellingCommand(int accountKey, string userID)
        {
            this.AccountKey = accountKey;
            this.UserID = userID;
        }

        public bool Result { get; set; }
    }
}