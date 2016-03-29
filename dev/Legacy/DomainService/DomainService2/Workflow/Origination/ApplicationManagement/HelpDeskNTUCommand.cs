using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class HelpDeskNTUCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }
        public bool Result { get; set; }
        public HelpDeskNTUCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}
