using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class AbstractNamedElement : AbstractElement
    {
        public AbstractNamedElement(string name)
        {
            this.Name = name;
        }

        public string Name { get; protected set; }

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

        public override bool Equals(object obj)
        {
            AbstractNamedElement other = obj as AbstractNamedElement;

            if (other != null && other.Name == this.Name)
                return true;
            return base.Equals(obj);
        }
    }
}