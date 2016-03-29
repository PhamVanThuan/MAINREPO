namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateActiveAcceptedProposalCommand : StandardDomainServiceCommand
    {
        public UpdateActiveAcceptedProposalCommand(int debtCounsellingKey, bool accepted)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.Accepted = accepted;
        }

        public int DebtCounsellingKey { get; set; }

        public bool Accepted { get; set; }

        public bool Result { get; set; }
    }
}