namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendEmailToConsultantForQueryCommand : StandardDomainServiceCommand
    {
        public SendEmailToConsultantForQueryCommand(int genericKey, long instanceID, int reasonGroupTypeKey)
        {
            this.GenericKey = genericKey;
            this.InstanceID = instanceID;
            this.ReasonGroupTypeKey = reasonGroupTypeKey;
        }

        public int GenericKey { get; set; }

        public long InstanceID { get; set; }

        public int ReasonGroupTypeKey { get; set; }

        public bool Result { get; set; }
    }
}