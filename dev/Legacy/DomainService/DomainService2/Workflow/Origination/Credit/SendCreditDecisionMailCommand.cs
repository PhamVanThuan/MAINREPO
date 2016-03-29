using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.Credit
{
    public class SendCreditDecisionMailCommand : StandardDomainServiceCommand
    {
        public long InstanceID { get; set; }
        public string Action { get; set; }
        public int OfferKey { get; set; }

        public SendCreditDecisionMailCommand(long instanceID,string action, int offerKey)
        {
            this.InstanceID = instanceID;
            this.Action = action;
            this.OfferKey = offerKey;
        }
    }
}
