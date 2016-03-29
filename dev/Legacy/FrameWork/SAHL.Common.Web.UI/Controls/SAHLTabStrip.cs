using System;
using System.Design;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Drawing.Design;
using System.Web.UI.HtmlControls;

namespace SAHL.Common.Web.UI.Controls
{

    /// <summary>
    /// Represents the method that will handle the event when a tab on a <see cref="SAHLTabStrip"/> 
    /// is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SAHLTabStripEventHandler(object sender, System.EventArgs e);

    #region SAHLTabStrip Class

    /// <summary>
    /// The SAHLTabStrip control.  This is a simple representation of a windows tab control, displaying 
    /// a row of tabs that each raise an event and become selected when they are clicked.  This is a 
    /// very basic implementation - there is no support at this stage for wrapping or scrolling.
    /// </summary>
    [
    DefaultProperty("Tabs"),
    ToolboxBitmap(typeof(SAHLRequiredFieldValidator), "Resources.SAHLTabStrip.bmp"),
    ToolboxData("<{0}:SAHLTabStrip runat=server></{0}:SAHLTabStrip>"),
    ParseChildren(true),
    Obsolete("No longer used - use the AJAX.NET Tab control instead.", true)
    ]
    public class SAHLTabStrip : CompositeControl, INamingContainer, IPostBackEventHandler
    {

        public new bool DesignMode = (HttpContext.Current == null);

        public SAHLTabStrip()
        {
            this.EnableViewState = false;
        }

        #region Private Members

        //private bool m_CanScrollLeft = false;
        //private bool m_CanScrollRight = false;
        //private Button m_LeftButton;
        //private Button m_RightButton;


        private HorizontalAlign m_ImageAlign = HorizontalAlign.Left;
        private int m_ImagePad = 5;

        private SAHLTabItems m_Tabs = new SAHLTabItems();

        #endregion

        #region Events

        /// <summary>
        /// The event that occurs when a tab on the tab strip is clicked.
        /// </summary>
        public event SAHLTabStripEventHandler TabClick;

        #endregion

        #region Methods

        protected override void AddParsedSubObject(object obj)
        {
            if (obj is SAHLTabItem)
            {
                m_Tabs.Add((SAHLTabItem)obj);
            }
            else
            {
                base.AddParsedSubObject(obj);
            }
        }

        //protected override void CreateChildControls()
        //{
        //    base.CreateChildControls();
        //    m_LeftButton = new Button();
        //    m_LeftButton.Text = "<";
        //    m_LeftButton.Width = Unit.Pixel(15);
        //    this.Controls.Add(m_LeftButton);
        //    m_RightButton = new Button();
        //    m_RightButton.Text = ">";
        //    m_RightButton.Width = Unit.Pixel(15);
        //    this.Controls.Add(m_RightButton);
        //}

        /// <summary>
        /// This method is used internally to retrieve Design-time specific CSS for the tab strip.  The 
        /// tab images are web resources in the CSS file which is itself a web resource, and Visual Studio seems 
        /// to ignore this.  This method was created to make the tabstrip resemble what you see at design time 
        /// by explicitly setting the background images.
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        private string GetTabUIDesignCss(bool active)
        {
            if (!DesignMode)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("background:url('");
            sb.Append(Page.ClientScript.GetWebResourceUrl(this.GetType(), "SAHL.Common.Web.UI.Controls.Resources.tabright.gif"));
            sb.Append("') no-repeat right top;");

            if (active) sb.Append("background-position:0% -42px;");
            return sb.ToString();
        }

        /// <summary>
        /// Overridden so the style sheet required for the tab trip can be inserted into the header at run time. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // do javascript and css output here
            if (Page.Header == null)
            {
                throw new InvalidOperationException("This control requires that the page have a <head runat=\"server\" /> control.");
            }
            else
            {
                // Add the Stylesheet
                Page.Header.Controls.Add(StyleSheetLink);
            }
        }

        /// <summary>
        /// Raises the <see cref="TabClick"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTabClick(object sender, EventArgs e)
        {
            if (TabClick != null)
            {
                TabClick(sender, e);
            }
        }

        /// <summary>
        /// Overridden for custom control.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            // at design time, the style sheet from the web resource is not inserted
            if (DesignMode)
            {
                StyleSheetLink.RenderControl(writer);
            }

            // DIV - the containing div, add an id based on the control name and add the cssclass
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID + "_tabcontainer");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "tabcontainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // render the left navigation
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID + "_ts_leftnav");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "tsleftnav");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div); // Div
            //m_LeftButton.RenderControl(writer);
            //writer.RenderEndTag(); // - Left Nav Div

            // render the right navigation
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID + "_ts_rightvnav");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "tsrightnav");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div); // Div
            //m_RightButton.RenderControl(writer);
            //writer.RenderEndTag(); // - Right Nav Div


            // DIV - the tab container
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "tabstrip");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // UL
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);// Ul

            // now write out each of the tabs
            for (int i = 0; i < TabCount; i++)
            {
                bool p_Active = (SelectedTabIndex == i);

                // LI
                if (p_Active)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "active");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                // A - also add some additional Css if we're in design time - for some reason Visual Studio 
                // seems to ignore WebResource calls within the Css web resource, so this is a bit of a hack 
                // to make the control look similar to what it does at run time
                if (DesignMode)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, GetTabUIDesignCss(p_Active));
                }
                // NOTE: the event has to be done with onclick, otherwise the onbeforeunload event handler 
                // on the SAHL pages overwrites the framework hidden values and you'll get JavaScript errors
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                if (!p_Active)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                        Page.ClientScript.GetPostBackEventReference(this, i.ToString())
                        );
                }
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                if (DesignMode)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, GetTabUIDesignCss(p_Active));
                }

                // SPAN
                writer.RenderBeginTag(HtmlTextWriterTag.Span);

                // this is the tab body, with the image drawn on either side depending on the alignment set
                // if no image is set for the tab, this will be ignored
                if (ImageAlign != HorizontalAlign.Right)
                    RenderImage(writer, m_Tabs[i]);
                writer.Write(m_Tabs[i].Text);
                if (ImageAlign == HorizontalAlign.Right)
                    RenderImage(writer, m_Tabs[i]);
                // end SPAN
                writer.RenderEndTag();

                // end A
                writer.RenderEndTag();

                // end LI
                writer.RenderEndTag();

            }
            // end UL
            writer.RenderEndTag();

            // end DIV (container)
            writer.RenderEndTag();

            // end DIV (tabstrip)
            writer.RenderEndTag();


        }

        /// <summary>
        /// Used internally to render an image on each tab.  This will be called at different 
        /// times depending on the horizontal alignment of the image.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tabItem"></param>
        private void RenderImage(HtmlTextWriter writer, SAHLTabItem tabItem)
        {
            if (tabItem.Image.Length == 0)
            {
                return;
            }
            string p_Style = "padding:0px;";
            if (ImageAlign == HorizontalAlign.Right)
            {
                p_Style = p_Style + "margin-left:" + ImagePad.ToString() + "px;";
            }
            else
            {
                p_Style = p_Style + "margin-right:" + ImagePad.ToString() + "px;";
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, String.Empty);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, tabItem.Image);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, p_Style);

            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }


        #endregion

        #region IPostBackEventHandler Implementation

        /// <summary>
        /// This control handles postback events by setting the currently selected tab, and raising a 
        /// <see cref="TabClick"/> event.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            SelectedTab = Tabs[Int32.Parse(eventArgument)];
            OnTabClick(SelectedTab, new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// How the image is aligned on the tab.  
        /// </summary>
        /// <remarks>
        /// Only left and right alignments will be used, with left alignment being the default.
        /// </remarks>
        public HorizontalAlign ImageAlign
        {
            get
            {
                return m_ImageAlign;
            }
            set
            {
                // we only care about this value if it is right aligned - otherwise ignore it 
                // so the image always appears on the left
                if (value == HorizontalAlign.Right)
                {
                    m_ImageAlign = value;
                }
                else
                {
                    m_ImageAlign = HorizontalAlign.Left;
                }
            }
        }

        /// <summary>
        /// Gets/sets the space (in pixels) between the image and the text.  This defaults to 5px.
        /// </summary>
        public int ImagePad
        {
            get
            {
                return m_ImagePad;
            }
            set
            {
                m_ImagePad = value;
            }
        }

        /// <summary>
        /// Gets the link object that is written to the page for the tabstrip stylesheet.
        /// </summary>
        private HtmlLink StyleSheetLink
        {
            get
            {
                HtmlLink p_Link = new HtmlLink();
                p_Link.Href = Page.ClientScript.GetWebResourceUrl(
                    this.GetType(),
                    "SAHL.Common.Web.UI.Controls.Resources.SAHLTabStrip.css");
                p_Link.Attributes.Add("type", "text/css");
                p_Link.Attributes.Add("rel", "stylesheet");
                return p_Link;

            }
        }

        /// <summary>
        /// Gets/sets the currently selected tab.  By default, there is no tab selected and this 
        /// returns null.
        /// </summary>
        [Browsable(false)]
        public SAHLTabItem SelectedTab
        {
            get
            {
                this.EnsureChildControls();
                // no tabs set - return null
                if (TabCount == 0 || SelectedTabIndex == -1)
                {
                    return null;
                }
                else
                {
                    return Tabs[SelectedTabIndex];
                }
            }
            set
            {
                this.EnsureChildControls();
                if (TabCount == 0) return;

                if (value == null)
                {
                    SelectedTabIndex = -1;
                }
                else
                {
                    SelectedTabIndex = Tabs.IndexOf(value);
                }
            }
        }

        /// <summary>
        /// Gets/sets the currently selected tab index.  By default, the selected index is -1 (no tab 
        /// selected).
        /// </summary>
        [PersistenceMode(PersistenceMode.Attribute)]
        public int SelectedTabIndex
        {
            get
            {
                if (Attributes["SelectedTabIndex"] != null)
                {
                    int i = Int32.Parse(Attributes["SelectedTabIndex"]);
                    if (i >= -1 && i < Tabs.Count) return i;
                }
                return -1;
            }
            set
            {
                Attributes["SelectedTabIndex"] = value.ToString();
            }
        }

        /// <summary>
        /// Gets the number of tabs added to the control.  
        /// </summary>
        /// <remarks>This property is not available at design time.</remarks>
        [Browsable(false)]
        public int TabCount
        {
            get
            {
                return m_Tabs.Count;
            }
        }

        /// <summary>
        /// Gets a reference to the Tabs list.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SAHLTabItems Tabs
        {
            get
            {
                return m_Tabs;
            }
        }

        #endregion

    }

    #endregion

    #region SAHLTabItems Class (Collection)

    /// <summary>
    /// Collection containing tab items that can appear on the <see cref="SAHLTabStrip"/>.
    /// </summary>
    public class SAHLTabItems : List<SAHLTabItem>
    {

    }

    #endregion

    #region SAHLTabItem class


    /// <summary>
    /// Tab item control for use with the <see cref="SAHLTabStrip"/> control.
    /// </summary>
    public class SAHLTabItem
    {
        private string m_Text = String.Empty;
        private string m_NavigationValue = String.Empty;
        private string m_Image = String.Empty;

        /// <summary>
        /// The text appearing on each tab.
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// This can be used to store a navigation value for a tab.  This is not really used by the control, 
        /// but is useful for keeping track of what should happen when the tab is clicked - e.g. navigate to the 
        /// page stored in this value.
        /// </summary>
        public string NavigationValue
        {
            get { return m_NavigationValue; }
            set { m_NavigationValue = value; }
        }

        /// <summary>
        /// The image used for the tab.  NOTE: Images used on tabs should all be the same size, otherwise 
        /// rendering issues occur!
        /// </summary>
        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor))]
        public string Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }

    }

    #endregion


}
