namespace DomainService2.Workflow.DebtCounselling
{
    public class CancelDebtCounsellingCommand : StandardDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public int LatestReasonDefintionKey { get; set; }

        public CancelDebtCounsellingCommand(int debtCounsellingKey, int latestReasonDefintionKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.LatestReasonDefintionKey = latestReasonDefintionKey;
        }
    }
}