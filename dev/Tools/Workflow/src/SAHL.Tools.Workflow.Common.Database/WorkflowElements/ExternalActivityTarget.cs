using System.Collections.Generic;
using Headspring;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public class ExternalActivityTarget : Enumeration<ExternalActivityTarget>
    {
        private ExternalActivityTarget(int value, string displayName)
            : base(value, displayName)
        {
        }

        public static readonly ExternalActivityTarget
            ThisInstance = new ExternalActivityTarget(1, "This Instance");

        public static readonly ExternalActivityTarget
            ParentInstance = new ExternalActivityTarget(2, "Parent Instance");

        public static readonly ExternalActivityTarget
            AnyInstance = new ExternalActivityTarget(3, "Any Instance");

        public static readonly ExternalActivityTarget
            ChildInstance = new ExternalActivityTarget(4, "Child Instance");
    }
}