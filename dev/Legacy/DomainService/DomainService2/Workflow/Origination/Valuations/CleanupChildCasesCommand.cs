using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class CleanupChildCasesCommand : StandardDomainServiceCommand
    {
        public CleanupChildCasesCommand(long parentInstanceID)
        {
            this.ParentInstanceID = parentInstanceID;
        }

        public long ParentInstanceID { get; set; }
    }
}
