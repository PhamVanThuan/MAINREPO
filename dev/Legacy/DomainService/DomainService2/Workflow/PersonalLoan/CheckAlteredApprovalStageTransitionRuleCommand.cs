using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckAlteredApprovalStageTransitionRuleCommand: RuleDomainServiceCommand
    {
        public CheckAlteredApprovalStageTransitionRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.CheckAlteredApprovalStageTransition)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}
