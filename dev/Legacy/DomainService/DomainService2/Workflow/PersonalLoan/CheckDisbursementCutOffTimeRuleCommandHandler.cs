using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckDisbursementCutOffTimeRuleCommandHandler : RuleDomainServiceCommandHandler<CheckDisbursementCutOffTimeRuleCommand>
    {
        public CheckDisbursementCutOffTimeRuleCommandHandler(ICommandHandler commandHandler):base(commandHandler)
        {

        }

    }
}
