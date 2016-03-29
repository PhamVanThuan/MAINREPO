using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCanEmailPersonalLoanApplicationRuleCommand : RuleDomainServiceCommand
    {
        public CheckCanEmailPersonalLoanApplicationRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.CheckCanEmailPersonalLoanApplication)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}
