using System;
using System.Drawing;
using Northwoods.Go;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class CustomLink : GoLink
    {
        public CustomLink()
        {
            this.Style = GoStrokeStyle.Bezier;
            this.Deletable = false;
            this.Selectable = true;
            this.Editable = false;
            this.Reshapable = false;
            this.Curviness = 0;
            this.Movable = false;
        }

        protected override bool RescalePoints(int startIndex, PointF newFromPoint, int endIndex, PointF newToPoint)
        {
            base.CalculateStroke();
            return base.RescalePoints(startIndex, newFromPoint, endIndex, newToPoint);
        }

        public override void CalculateStroke()
        {
            if (this.FromNode != null && this.FromNode.GoObject is BaseActivity)
            {
                foreach (CustomLink l in FromNode.Links)
                {
                    if (l != this)
                    {
                        if (l.FromNode == this.ToNode && l.ToNode == this.FromNode && l.ToNode.GetType() != typeof(CallWorkFlowActivity) && l.ToNode.GetType() != typeof(ReturnWorkflowActivity))
                        {
                            this.Curviness = 50;
                        }
                        else
                        {
                            this.Curviness = 0;
                        }
                    }
                }
            }
            else if (this.ToNode != null && this.ToNode.GoObject is BaseActivity)
            {
                foreach (CustomLink l in ToNode.Links)
                {
                    if (l != this)
                    {
                        if (l.ToNode == this.FromNode && l.FromNode == this.ToNode && this.ToNode.GetType() != typeof(CallWorkFlowActivity) && l.FromNode.GetType() != typeof(ReturnWorkflowActivity))
                        {
                            this.Curviness = 50;
                        }
                        else
                        {
                            this.Curviness = 0;
                        }
                    }
                }
            }
            base.CalculateStroke();
        }
    }
}