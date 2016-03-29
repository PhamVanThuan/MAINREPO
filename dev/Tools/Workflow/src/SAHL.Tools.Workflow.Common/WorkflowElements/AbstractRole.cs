using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class AbstractRole : AbstractNamedElement
    {
        public AbstractRole(string name, bool isDynamic, CodeSection onGetRole)
            : base(name)
        {
            this.IsDynamic = isDynamic;
            this.OnRoleCode = onGetRole;

            base.AddCodeSection(this.OnRoleCode);
        }

        public bool IsDynamic { get; protected set; }

        public virtual string Description { get; set; }

        public RoleTypeEnum RoleType { get; protected set; }

        public CodeSection OnRoleCode { get; protected set; }
    }
}