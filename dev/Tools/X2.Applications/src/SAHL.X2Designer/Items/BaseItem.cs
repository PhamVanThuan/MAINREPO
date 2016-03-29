using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Northwoods.Go;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class BaseItem : GoIconicNode, IWorkflowItem
    {
        protected string m_IconName;
        protected List<string> m_ValidationErrors;
        protected WorkflowItemBaseType m_ItemBaseType;
        protected WorkflowItemType m_ItemType;
        protected string[] m_AvailableCodeSections;
        protected object m_Props;

        /// <summary>
        /// Override the base Initialize to use the protected member variable for the image name
        /// and do not allow reshaping of the node.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="iconname"></param>
        /// <param name="name"></param>
        public override void Initialize(System.Resources.ResourceManager res, string iconname, string name)
        {
            // change below to always load the correct icon
            base.Initialize(null, m_IconName, name);
            this.Reshapable = false;
            this.Selectable = true;
            this.Label.FontSize = 8;
            this.Label.BackgroundColor = Color.Cornsilk;
            this.Label.TransparentBackground = false;
            foreach (MultiPortNodePort p in this.Ports)
            {
                p.Visible = false;
                p.FromSpot = NoSpot;
                p.ToSpot = NoSpot;
            }
        }

        // dont allow initialisation by this means

        #region Overrides

        public override void Initialize(System.Windows.Forms.ImageList imglist, int imgindex, string name)
        {
            throw new NotSupportedException();
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            base.ChangeValue(e, undo);
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        #endregion Overrides

        int IComparable.CompareTo(object x)
        {
            String mName = x.ToString();
            return string.Compare(this.Name, mName);
        }

        #region IWorkflowItem Members

        public virtual string Name
        {
            get { return base.Text; }
            set
            {
                MainForm.App.GetCurrentView().Document.StartTransaction();
                base.Text = value;
                MainForm.App.GetCurrentView().Document.FinishTransaction("rename");
            }
        }

        public WorkflowItemBaseType WorkflowItemBaseType
        {
            get { return m_ItemBaseType; }
        }

        public WorkflowItemType WorkflowItemType
        {
            get { return m_ItemType; }
        }

        public List<string> ValidationErrors
        {
            get { return null; }
        }

        public void Validate()
        {
            // validate the item in here
        }

        public string[] AvailableCodeSections
        {
            get
            {
                return GetAvailableCodeSectionsInternal(); ;
            }
        }

        protected virtual string[] GetAvailableCodeSectionsInternal()
        {
            return m_AvailableCodeSections;
        }

        public virtual string GetCodeSectionData(string CodeSectionName)
        {
            return "";
        }

        public virtual void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
        }

        public virtual void UpdateCodeSectionData(string OldValue, string NewValue)
        {
        }

        public virtual void Copy(BaseItem newItem)
        {
        }

        public virtual string RefreshCodeSectionData(string CodeSectionName)
        {
            return "";
        }

        public virtual object Properties
        {
            get
            {
                return m_Props;
            }
        }

        #endregion IWorkflowItem Members

        /// <summary>
        /// Don't create any ports initially--these need to be added and positioned explicitly.
        /// </summary>
        /// <returns>a <see cref="MultiPortNodePort"/> by default</returns>
        protected override GoPort CreatePort()
        {
            if (this.Initializing) return null;
            MultiPortNodePort p = new MultiPortNodePort();
            p.Visible = false;
            p.Printable = false;
            p.FromSpot = NoSpot;
            p.ToSpot = NoSpot;
            return p;
        }

        /// <summary>
        /// Create and Add a port to this node at a particular position
        /// relative to the icon.
        /// </summary>
        /// <param name="id">
        /// the <see cref="GoPort.UserFlags"/> property is set to this value
        /// in order to help distinguish between different ports
        /// </param>
        /// <param name="iconoffset">relative to the top-left corner of the icon</param>
        /// <param name="linkspot">
        /// initial values for the port's <see cref="GoPort.FromSpot"/>
        /// and <see cref="GoPort.ToSpot"/> properties
        /// </param>
        /// <returns>the result of the call to <see cref="CreatePort"/></returns>
        public GoPort AddPort(int id, SizeF iconoffset, int linkspot)
        {
            GoPort p = CreatePort();
            if (p != null)
            {
                p.UserFlags = id;
                PointF pnt = this.SelectionObject.Position;
                p.Center = new PointF(pnt.X + iconoffset.Width, pnt.Y + iconoffset.Height);
                p.FromSpot = linkspot;
                p.ToSpot = linkspot;
                p.Selectable = false;
                Add(p);
            }
            return p;
        }

        /// <summary>
        /// Update the positions of all the other ports before performing the
        /// standard call to <see cref="GoIconicNode.LayoutChildren"/>
        /// </summary>
        /// <param name="child"></param>
        /// <param name="old"></param>
        protected override void OnChildBoundsChanged(GoObject child, RectangleF old)
        {
            //if (child != null && child == this.Icon && old.Width > 0 && old.Height > 0)
            //{
            //    RectangleF thisRect = this.Icon.Bounds;
            //    float scaleFactorX = thisRect.Width / old.Width;
            //    float scaleFactorY = thisRect.Height / old.Height;

            //    foreach (GoObject obj in this)
            //    {
            //        if (obj == this.Icon) continue;
            //        RectangleF childRect = obj.Bounds;
            //        float newRectx = thisRect.X + ((childRect.X - old.X) * scaleFactorX);
            //        float newRecty = thisRect.Y + ((childRect.Y - old.Y) * scaleFactorY);
            //        float newRectwidth = childRect.Width;
            //        float newRectheight = childRect.Height;
            //        if (obj.AutoRescales)
            //        {
            //            newRectwidth *= scaleFactorX;
            //            newRectheight *= scaleFactorY;
            //        }
            //        obj.Bounds = new RectangleF(newRectx, newRecty, newRectwidth, newRectheight);
            //    }
            //}
            base.OnChildBoundsChanged(child, old);  // this will call LayoutChildren
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MultiPortNodePort : GoPort
    {
        public MultiPortNodePort()
        {
            this.Style = GoPortStyle.Ellipse;
            this.Brush = null;
            this.Size = new SizeF(8, 8);
            this.FromSpot = NoSpot;
            this.ToSpot = NoSpot;
            this.AutoRescales = false;
        }

        /// <summary>
        /// This is a convenience property for treating the parent node as a <see cref="MultiPortNode"/>.
        /// </summary>
        public BaseItem MultiPortNode
        {
            get { return this.Parent as BaseItem; }
        }

        /// <summary>
        /// This is a convenience property when the port's appearance is actually a <see cref="GoImage"/>.
        /// </summary>
        /// <remarks>
        /// Setting this to a non-null value will change the <see cref="GoPort.PortStyle"/> appropriately.
        /// </remarks>
        public GoImage PortImage
        {
            get { return this.PortObject as GoImage; }
            set
            {
                this.PortObject = value;
                if (value == null)
                    this.Style = GoPortStyle.Ellipse;
                else
                    this.Style = GoPortStyle.Object;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of links the user may draw to this port.
        /// </summary>
        public int MaxLinks
        {
            get { return myMaxLinks; }
            set
            {
                int old = myMaxLinks;
                if (old != value && value >= 0)
                {
                    myMaxLinks = value;
                    Changed(ChangedMaxLinks, old, null, NullRect, value, null, NullRect);
                }
            }
        }

        /// <summary>
        /// This override also checks to see if <see cref="IGoPort.LinksCount"/> is less
        /// than the permissible <see cref="MaxLinks"/> value.
        /// </summary>
        /// <returns></returns>
        public override bool CanLinkFrom()
        {
            return base.CanLinkFrom() &&
                   this.LinksCount < this.MaxLinks;
        }

        /// <summary>
        /// This override also checks to see if <see cref="IGoPort.LinksCount"/> is less
        /// than the permissible <see cref="MaxLinks"/> value.
        /// </summary>
        /// <returns></returns>
        public override bool CanLinkTo()
        {
            return base.CanLinkTo() &&
                   this.LinksCount < this.MaxLinks;
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedMaxLinks:
                    this.MaxLinks = e.GetInt(undo);
                    return;
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        public const int ChangedMaxLinks = 2121;

        // State
        private int myMaxLinks = 999999;
    }

    /// <summary>
    /// Base Properties
    /// </summary>
    ///
    [Serializable]
    public class BaseProperties
    {
        protected BaseItem m_Owner;

        public BaseProperties(BaseItem Owner)
        {
            m_Owner = Owner;
        }

        [Browsable(false)]
        public BaseItem Owner
        {
            get
            {
                return m_Owner;
            }
        }
    }
}