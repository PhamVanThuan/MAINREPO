using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.DebtCounselling
{
    public class SendNotificationCommand :StandardDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }
        public SendNotificationCommand(int DebtCounsellingKey)
        {
            this.DebtCounsellingKey = DebtCounsellingKey;
        }
    }
}
