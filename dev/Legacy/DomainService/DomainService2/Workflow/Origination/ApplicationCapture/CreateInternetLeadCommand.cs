using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CreateInternetLeadCommand : StandardDomainServiceCommand
    {
        public bool Result { get; set; }
        public int ApplicationKey { get; set; }
        public string LeadData { get; set; }

        public CreateInternetLeadCommand(int applicationKey, string leadData)
        {
            this.ApplicationKey = applicationKey;
            this.LeadData = leadData;
        }
    }
}
