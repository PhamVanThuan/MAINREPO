using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCededAmountCoversApplicationAmountRuleCommand : RuleDomainServiceCommand
    {
        public CheckCededAmountCoversApplicationAmountRuleCommand(int applicationKey, double sumInsured, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.CheckCededAmountCoversApplicationAmount)
        {
            this.ApplicationKey = applicationKey;
            this.SumInsured = sumInsured;
        }

        public int ApplicationKey { get; set; }
        public double SumInsured { get; set; }
    }
}
