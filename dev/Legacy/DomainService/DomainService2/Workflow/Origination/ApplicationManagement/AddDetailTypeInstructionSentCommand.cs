namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class AddDetailTypeInstructionSentCommand : StandardDomainServiceCommand
    {
        public AddDetailTypeInstructionSentCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}