using System;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class ArchiveFLRelatedCasesCommand : StandardDomainServiceCommand
    {
        public ArchiveFLRelatedCasesCommand(int applicationKey, string ADUser, Int64 instanceID)
            : base()
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = ADUser;
            this.InstanceID = instanceID;
        }

        public Int64 InstanceID { get; protected set; }

        public int ApplicationKey { get; protected set; }

        public string ADUser { get; protected set; }
    }
}