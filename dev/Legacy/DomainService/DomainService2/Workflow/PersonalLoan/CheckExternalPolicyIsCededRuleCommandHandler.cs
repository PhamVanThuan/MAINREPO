using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckExternalPolicyIsCededRuleCommandHandler : RuleDomainServiceCommandHandler<CheckExternalPolicyIsCededRuleCommand>
    {
        public CheckExternalPolicyIsCededRuleCommandHandler(ICommandHandler commandHandler)
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
