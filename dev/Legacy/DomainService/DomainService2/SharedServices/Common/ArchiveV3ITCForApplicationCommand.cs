namespace DomainService2.SharedServices.Common
{
    public class ArchiveV3ITCForApplicationCommand : StandardDomainServiceCommand
    {
        public ArchiveV3ITCForApplicationCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}