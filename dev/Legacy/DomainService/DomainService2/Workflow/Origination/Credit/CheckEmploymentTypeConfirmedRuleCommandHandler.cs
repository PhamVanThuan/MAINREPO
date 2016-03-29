using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.Credit
{
    public class CheckEmploymentTypeConfirmedRuleCommandHandler : RuleDomainServiceCommandHandler<CheckEmploymentTypeConfirmedRuleCommand>
    {
        public CheckEmploymentTypeConfirmedRuleCommandHandler(ICommandHandler commandHandler)
            : base(commandHandler)
        {

        }
        public override void SetupRule()
        {
            command.RuleParameters = new object[] { command.InstanceId };
            base.SetupRule();
        }
    }
}
