namespace DomainService2.SharedServices.Common
{
    public class UpdateAssignedUserInIDMCommand : StandardDomainServiceCommand
    {
        public UpdateAssignedUserInIDMCommand(int applicationKey, System.Boolean isFurtherLoan, long instanceID, string mapName)
        {
            this.ApplicationKey = applicationKey;
            this.IsFurtherLoan = isFurtherLoan;
            this.InstanceID = instanceID;
            this.MapName = mapName;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public System.Boolean IsFurtherLoan
        {
            get;
            protected set;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public string MapName
        {
            get;
            protected set;
        }
    }
}