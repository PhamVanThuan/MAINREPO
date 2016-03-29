using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckExternalPolicyIsCededRuleCommand : RuleDomainServiceCommand
    {
        public CheckExternalPolicyIsCededRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.CheckExternalPolicyIsCeded)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}
