using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationStatusReturnedCommand : StandardDomainServiceCommand
    {
        public SetValuationStatusReturnedCommand(int valuationKey)
        {
            this.ValuationKey = valuationKey;
        }

        public int ValuationKey { get; set; }
    }
}
