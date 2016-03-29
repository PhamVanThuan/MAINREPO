using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public abstract class AbstractNamedState : AbstractState
    {
        public AbstractNamedState(string name, Single locationX, Single locationY, CodeSection onEnterState, CodeSection onExitState,Guid x2ID)
            : base(locationX, locationY)
        {
            this.X2ID = x2ID;
            this.Name = name;
            this.OnEnterStateCode = onEnterState;
            this.OnExitStateCode = onExitState;

            this.AddCodeSection(this.OnEnterStateCode);
            this.AddCodeSection(this.OnExitStateCode);
        }

        public Guid X2ID { get; set; }

        public string Name { get; protected set; }

        public Workflow Workflow { get; set; }

        public string SafeName
        {
            get
            {
                string OriginalName = Name.Replace("'", "");
                OriginalName = OriginalName.Replace("-", "_");
                OriginalName = OriginalName.Replace("&", "_");
                OriginalName = OriginalName.Replace("?", "_");
                OriginalName = OriginalName.Replace("%", "_");
                OriginalName = OriginalName.Replace("<", "_");
                OriginalName = OriginalName.Replace(">", "_");
                if (OriginalName.Contains("/"))
                    OriginalName = OriginalName.Replace("/", "_");
                OriginalName = OriginalName.Replace("+", "_");
                OriginalName = OriginalName.Replace(".", "_");
                OriginalName = OriginalName.Replace(",", "_");
                return OriginalName.Replace(" ", "_");
            }
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty.", "name");
            }
            this.Name = name;
        }

        public CodeSection OnEnterStateCode { get; protected set; }

        public CodeSection OnExitStateCode { get; protected set; }
    }
}