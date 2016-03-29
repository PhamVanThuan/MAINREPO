namespace DomainService2.Workflow.DebtCounselling
{
    public class SendLitigationReminderInternalEmailCommand : StandardDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public SendLitigationReminderInternalEmailCommand(int debtCounsellingKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}