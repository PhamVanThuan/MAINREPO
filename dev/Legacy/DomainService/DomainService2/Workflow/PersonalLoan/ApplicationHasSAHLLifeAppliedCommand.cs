namespace DomainService2.Workflow.PersonalLoan
{
    public class ApplicationHasSAHLLifeAppliedCommand : StandardDomainServiceCommand
    {
        public ApplicationHasSAHLLifeAppliedCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public bool Result { get; set; }
    }
}