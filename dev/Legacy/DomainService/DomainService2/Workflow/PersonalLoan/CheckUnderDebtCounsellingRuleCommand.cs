using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckUnderDebtCounsellingRuleCommand : RuleDomainServiceCommand
    {
        public CheckUnderDebtCounsellingRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, "LegalEntityUnderDebtCounselling")
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}