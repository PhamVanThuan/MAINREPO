using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    public class ElementPosition
    {
        public ElementPosition(AbstractElement element, Single locationX, Single locationY)
        {
            this.Element = element;
            this.LocationX = locationX;
            this.LocationY = locationY;
        }

        public AbstractElement Element { get; protected set; }

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