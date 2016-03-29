using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class FixedRole : WorkflowRole
    {
        public FixedRole(string name, string description, bool isDynamic)
            : base(name, isDynamic, null)
        {
            this.RoleType = RoleTypeEnum.Fixed;
            this.Description = description;
        }
    }
}