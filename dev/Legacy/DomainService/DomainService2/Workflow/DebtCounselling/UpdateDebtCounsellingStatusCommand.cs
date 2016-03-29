namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateDebtCounsellingStatusCommand : StandardDomainServiceCommand
    {
        public UpdateDebtCounsellingStatusCommand(int debtCounsellingKey, int debtCounsellingStatusKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.DebtCounsellingStatusKey = debtCounsellingStatusKey;
        }

        public int DebtCounsellingKey { get; set; }

        public int DebtCounsellingStatusKey { get; set; }

        public bool Result { get; set; }
    }
}