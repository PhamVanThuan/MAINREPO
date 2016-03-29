namespace DomainService2.Workflow.DebtCounselling
{
    public class CancelDebtCounselling_EXTCommand : StandardDomainServiceCommand
    {
        public CancelDebtCounselling_EXTCommand(long instanceID, string activityName, int debtCounsellingKey)
        {
            this.InstanceID = instanceID;
            this.ActivityName = activityName;
            this.DebtCounsellingKey = debtCounsellingKey;
        }

        public long InstanceID
        {
            get;
            set;
        }

        public string ActivityName
        {
            get;
            set;
        }

        public int DebtCounsellingKey
        {
            get;
            set;
        }

        public bool Result { get; set; }
    }
}