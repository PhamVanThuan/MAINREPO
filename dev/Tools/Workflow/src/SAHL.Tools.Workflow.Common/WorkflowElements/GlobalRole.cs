using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class GlobalRole : AbstractRole
    {
        public GlobalRole(string name, bool isDynamic)
            : base(name, isDynamic, null)
        {
            this.RoleType = RoleTypeEnum.Global;
        }

        public GlobalRole(string name, bool isDynamic, CodeSection onGetRole)
            : base(name, isDynamic, onGetRole)
        {
        }
    }
}