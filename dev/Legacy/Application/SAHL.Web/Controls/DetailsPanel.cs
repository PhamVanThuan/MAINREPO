using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Basic panel to which rows can be added with a title and corresponding text.
    /// </summary>
    public class DetailsPanel : Panel, INamingContainer
    {

        private bool _readOnly = true;
        private int _labelWidth = 160;
        private int _spacerHeight = 10;

        #region Properties

        /// <summary>
        /// Determines whether we are in design mode (standard DesignMode not reliable).
        /// </summary>
        protected static new bool DesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

        /// <summary>
        /// Gets/sets the width of the labels on the panel.
        /// </summary>
        public int LabelWidth
        {
            get
            {
                return _labelWidth;
            }
            set
            {
                _labelWidth = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the values on the control can be edited or not.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the amount of padding space is added at the top of the control before any rows 
        /// are added.  This defaults to 10px.
        /// </summary>
        public int SpacerHeight
        {
            get
            {
                return _spacerHeight;
            }
            set
            {
                _spacerHeight = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new readonly row to the panel.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        protected void AddRow(Control parent, string title, string text)
        {
            // create the div for displaying the text
            if (String.IsNullOrEmpty(text)) text = "-";
            HtmlGenericControl spanText = new HtmlGenericControl("span");
            spanText.InnerText = text;

            AddRow(parent, title, spanText);
        }

        /// <summary>
        /// Adds a new row to the panel with <c>valueControl</c> used for input or to display text.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="valueControl"></param>
        protected void AddRow(Control parent, string title, Control valueControl)
        {
            // create the row div
            HtmlGenericControl divRow = new HtmlGenericControl("div");
            divRow.Attributes.Add("class", "row");

            // create the div for displaying the title
            HtmlGenericControl divTitle = new HtmlGenericControl("div");
            divTitle.Attributes.Add("class", "cellInput");
            divTitle.Style.Add("width", LabelWidth.ToString() + "px");
            divTitle.Style.Add("font-weight", "bold");
            divTitle.InnerText = title;
            divRow.Controls.Add(divTitle);

            // add the control used for displaying the text
            HtmlGenericControl divText = new HtmlGenericControl("div");
            divText.Attributes.Add("class", "cellInput");
            divText.Controls.Add(valueControl);
            divRow.Controls.Add(divText);

            parent.Controls.Add(divRow);
        }

        protected void AddRow(Control parent, string title, Control valueControl, string hint)
        {
            // create the row div
            HtmlGenericControl divRow = new HtmlGenericControl("div");
            divRow.Attributes.Add("class", "row");

            // create the div for displaying the title
            HtmlGenericControl divTitle = new HtmlGenericControl("div");
            divTitle.Attributes.Add("class", "cellInput");
            divTitle.Style.Add("width", LabelWidth.ToString() + "px");
            divTitle.Style.Add("font-weight", "bold");
            divTitle.InnerText = title;
            divRow.Controls.Add(divTitle);

            // add the control used for displaying the text
            HtmlGenericControl divText = new HtmlGenericControl("div");
            divText.Attributes.Add("class", "cellInput");
            divText.Controls.Add(valueControl);
            divRow.Controls.Add(divText);

            // add a hint for the user control
            HtmlGenericControl divHint = new HtmlGenericControl("div");
            Image imgHint = new Image();
            imgHint.ImageUrl = "../../Images/help.gif";
            imgHint.ToolTip = hint;
            divHint.Attributes.Add("class", "cellInput");
            divHint.Controls.Add(imgHint);
            divRow.Controls.Add(divHint);

            parent.Controls.Add(divRow);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!DesignMode && !String.IsNullOrEmpty(GroupingText) && SpacerHeight > 0)
            {
                // this control is so we have some space between the title and the other controls
                HtmlGenericControl spacer = new HtmlGenericControl("div");
                spacer.Style.Add(HtmlTextWriterStyle.Height, String.Format("{0}px", SpacerHeight));
                this.Controls.AddAt(0, spacer);
            }

        }

        #endregion
    }
}
