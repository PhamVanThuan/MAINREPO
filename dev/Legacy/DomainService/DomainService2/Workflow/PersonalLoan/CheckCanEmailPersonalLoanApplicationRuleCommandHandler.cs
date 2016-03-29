using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCanEmailPersonalLoanApplicationRuleCommandHandler : RuleDomainServiceCommandHandler<CheckCanEmailPersonalLoanApplicationRuleCommand>
    {
        public CheckCanEmailPersonalLoanApplicationRuleCommandHandler(ICommandHandler commandHandler)
            : base(commandHandler)
        {

        }
        public override void SetupRule()
        {
            command.RuleParameters = new object[] { command.ApplicationKey };
            base.SetupRule();
        }
    }
}
