namespace DomainService2.SharedServices.Common
{
    public class CheckUserInMandateCommand : StandardDomainServiceCommand
    {
        public CheckUserInMandateCommand(int applicationKey, string aDuserName, string orgStructureName)
        {
            this.ApplicationKey = applicationKey;
            this.ADuserName = aDuserName;
            this.OrgStructureName = orgStructureName;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string ADuserName
        {
            get;
            protected set;
        }

        public string OrgStructureName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}