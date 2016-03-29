namespace DomainService2.Workflow.DebtCounselling
{
    public class DebtCounsellingOptOutRequiredCommand : StandardDomainServiceCommand
    {
        public DebtCounsellingOptOutRequiredCommand(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public int AccountKey { get; set; }

        public bool Result { get; set; }
    }
}