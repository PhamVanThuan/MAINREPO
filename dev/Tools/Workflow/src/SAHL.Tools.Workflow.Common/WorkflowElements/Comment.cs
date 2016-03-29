using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class Comment : AbstractNamedPositionableElement
    {
        public Comment(string name, Single locationX, Single locationY, int genericKeyType)
            : base(name, locationX, locationY)
        {
        }
    }
}