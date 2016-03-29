using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckDebtCounsellingRulesCommand : RuleSetDomainServiceCommand
    {
        public int DebtCounsellingKey { get; set; }

        public CheckDebtCounsellingRulesCommand(int debtCounsellingKey, string workflowRuleSetName, bool ignoreWarnings)
            : base(ignoreWarnings, workflowRuleSetName)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }
    }
}