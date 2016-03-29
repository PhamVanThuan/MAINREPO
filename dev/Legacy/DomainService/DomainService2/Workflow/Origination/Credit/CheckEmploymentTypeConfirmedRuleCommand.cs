using DomainService2.SharedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.Credit
{
    public class CheckEmploymentTypeConfirmedRuleCommand : RuleDomainServiceCommand
    {
        public CheckEmploymentTypeConfirmedRuleCommand(long instanceId, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.CheckEmploymentTypeConfirmed)
        {
            this.InstanceId = instanceId;
        }

        public long InstanceId { get; protected set; }

    }
}
