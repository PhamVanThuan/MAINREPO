namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CanRollbackReadvanceCorrectionTransactionCommand : StandardDomainServiceCommand
    {
        public CanRollbackReadvanceCorrectionTransactionCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public bool Result { get; set; }
    }
}