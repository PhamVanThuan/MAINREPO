using System;

namespace DomainService2.Workflow.Origination.Credit
{
    public class DoesNotMeetCreditSignatureRequirementsCommand : StandardDomainServiceCommand
    {
        public DoesNotMeetCreditSignatureRequirementsCommand(int applicationKey, long instanceID)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
        }

        public int ApplicationKey { get; protected set; }

        public Int64 InstanceID { get; protected set; }

        public bool Result { get; set; }
    }
}