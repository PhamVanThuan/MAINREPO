namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateHearingDetailStatusToInactiveCommand : StandardDomainServiceCommand
    {
        public UpdateHearingDetailStatusToInactiveCommand(int debtCounsellingKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }

        public int DebtCounsellingKey { get; set; }

        public bool Result { get; set; }
    }
}