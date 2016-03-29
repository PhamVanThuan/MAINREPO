namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class InsertCommissionableConsultantCommand : StandardDomainServiceCommand
    {
        public InsertCommissionableConsultantCommand(long instanceID, string adUserName, int genericKey, string stateName)
        {
            this.InstanceID = instanceID;
            this.AdUserName = adUserName;
            this.GenericKey = genericKey;
            this.StateName = stateName;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public string AdUserName
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public string StateName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}