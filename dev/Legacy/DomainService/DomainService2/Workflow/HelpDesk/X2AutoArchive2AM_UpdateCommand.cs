namespace DomainService2.Workflow.HelpDesk
{
    public class X2AutoArchive2AM_UpdateCommand : StandardDomainServiceCommand
    {
        public X2AutoArchive2AM_UpdateCommand(int helpDeskQueryKey)
        {
            this.HelpDeskQueryKey = helpDeskQueryKey;
        }

        public int HelpDeskQueryKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}