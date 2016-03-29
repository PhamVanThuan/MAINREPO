namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class UpdateRegMailDetailToInstructionNotSentCommand : StandardDomainServiceCommand
    {
        public UpdateRegMailDetailToInstructionNotSentCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}