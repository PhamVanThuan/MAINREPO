using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Workflow.Origination.Credit
{
    public class CheckApplicationIsNewBusinessRuleCommand : RuleDomainServiceCommand
    {
        public CheckApplicationIsNewBusinessRuleCommand(int applicationKey, bool ignoreWarnings)
            :base(ignoreWarnings,SAHL.Common.Rules.ApplicationIsNewBusiness)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}
