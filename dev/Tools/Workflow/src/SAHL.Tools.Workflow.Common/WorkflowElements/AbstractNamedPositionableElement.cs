using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class AbstractNamedPositionableElement : AbstractNamedElement
    {
        public AbstractNamedPositionableElement(string name, Single locationX, Single locationY)
            : base(name)
        {
            this.LocationX = locationX;
            this.LocationY = locationY;
        }

        public Single LocationX { get; protected set; }

        public void UpdateLocationX(Single locationX)
        {
            this.LocationX = locationX;
        }

        public Single LocationY { get; protected set; }

        public void UpdateLocationY(Single locationY)
        {
            this.LocationY = locationY;
        }
    }
}