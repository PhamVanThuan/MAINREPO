using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckDisbursementCutOffTimeRuleCommand : RuleDomainServiceCommand
    {
        public CheckDisbursementCutOffTimeRuleCommand(bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.PersonalLoanDisbursementCutOffTimeCheck)
        {

        }
    }
}
