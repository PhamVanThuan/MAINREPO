namespace DomainService2.Workflow.DebtCounselling
{
    public class SendNotificationsCancellatonRegistered_EXTCommand : StandardDomainServiceCommand
    {
        public SendNotificationsCancellatonRegistered_EXTCommand(int debtCounsellingKey, bool recoveriesProposalReceivedStageTransitionExists)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.RecoveriesProposalReceivedStageTransitionExists = recoveriesProposalReceivedStageTransitionExists;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public bool RecoveriesProposalReceivedStageTransitionExists
        {
            get;
            set;
        }
    }
}