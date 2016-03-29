namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CreateEWorkPipelineCaseCommand : StandardDomainServiceCommand
    {
        public CreateEWorkPipelineCaseCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public string EFolderID { get; set; }

        public bool Result { get; set; }
    }
}