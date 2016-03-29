namespace DomainService2.Workflow.Cap2
{
    public class IsCreditCheckRequiredCommand : StandardDomainServiceCommand
    {
        public IsCreditCheckRequiredCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}