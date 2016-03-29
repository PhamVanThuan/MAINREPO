namespace DomainService2.Workflow.DebtCounselling
{
    public class RollbackTransactionCommand : StandardDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public RollbackTransactionCommand(int debtCounsellingKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }

        public bool Result { get; set; }
    }
}