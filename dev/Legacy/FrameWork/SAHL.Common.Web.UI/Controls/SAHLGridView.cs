using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLGridView), "Resources.SAHLGridView.bmp")]
    [ToolboxData("<{0}:SAHLGridView runat=server width=770 height=200></{0}:SAHLGridView>")]
    public class SAHLGridView : GridView, ISAHLSecurityControl
    {
        public delegate void GridDoubleClickEventHandler(object sender, GridSelectEventArgs e);
        public event GridDoubleClickEventHandler GridDoubleClick;

        private int m_SelectedIndex = 0;
        private string m_HeaderCaption = "";
        private bool m_HeaderVisible = true;
        private string m_NullDataSetMessage = "";
        private string m_EmptyDataSetMessage = "";
        private bool m_ScrollX = false;
        private bool m_TotalsFooter = true;
        private int m_CellsWidth = 0;
        private Unit m_Width;
        private Unit m_Height;
        private bool m_FixedHeader;
        private string m_FixedHeaderStr;
        private bool m_FireEvents = false;
        private bool m_SelectFirstRow = true; // If there are records, the first record is selected by default.
        private bool m_SetSelectedIndex = false;
        private bool m_Invisible = false;
        private GridPostBackType m_PostBackType = GridPostBackType.None;
        private IFormatProvider culture = new CultureInfo(Constants.CultureGb, false);
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set { m_HeaderVisible = value; }
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string HeaderCaption
        {
            get { return m_HeaderCaption; }
            set { m_HeaderCaption = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NullDataSetMessage
        {
            get { return m_NullDataSetMessage; }
            set { m_NullDataSetMessage = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string EmptyDataSetMessage
        {
            get { return m_EmptyDataSetMessage; }
            set { m_EmptyDataSetMessage = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool ScrollX
        {
            get { return m_ScrollX; }
            set { m_ScrollX = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool TotalsFooter
        {
            get { return m_TotalsFooter; }
            set { m_TotalsFooter = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public Unit GridWidth
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public Unit GridHeight
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool FixedHeader
        {
            get { return m_FixedHeader; }
            set { m_FixedHeader = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool SelectFirstRow
        {
            get { return m_SelectFirstRow; }
            set { m_SelectFirstRow = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool Invisible
        {
            get { return m_Invisible; }
            set { m_Invisible = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(GridPostBackType.None)]
        [Localizable(true)]
        public GridPostBackType PostBackType
        {
            get { return m_PostBackType; }
            set { m_PostBackType = value; }
        }

        public override int SelectedIndex
        {
            get { return base.SelectedIndex; }
            set
            {
                base.SelectedIndex = value;
                m_SelectedIndex = value;
            }
        }

        public int SelectedIndexInternal
        {
            get { return m_SelectedIndex; }
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);

            int iC;
            GridBoundField gbField = new GridBoundField();

            if (m_FixedHeader)
                m_FixedHeaderStr = "true";
            else
                m_FixedHeaderStr = "false";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["id"] = this.UniqueID + "_" + e.Row.RowIndex;
                switch (PostBackType)
                {
                    case GridPostBackType.None:
                        break;
                    case GridPostBackType.NoneWithClientSelect:
                        e.Row.Attributes["onclick"] = "SAHLGridView_selectGridRow(this," + m_FixedHeaderStr + ",'" + this.UniqueID + "_SELECTED');";
                        break;
                    case GridPostBackType.SingleClick:
                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this, "Select$" + e.Row.RowIndex);
                        break;
                    case GridPostBackType.DoubleClick:
                        e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackEventReference(this, "SelectDBL$" + e.Row.RowIndex);
                        break;
                    case GridPostBackType.DoubleClickWithClientSelect:
                        e.Row.Attributes["onclick"] = "SAHLGridView_selectGridRow(this," + m_FixedHeaderStr + ",'" + this.UniqueID + "_SELECTED');";
                        e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackEventReference(this, "SelectDBL$" + e.Row.RowIndex);
                        break;
                    case GridPostBackType.SingleAndDoubleClick:
                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this, "Select$" + e.Row.RowIndex);
                        e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackEventReference(this, "SelectDBL$" + e.Row.RowIndex);
                        break;
                }

                if (PostBackType != GridPostBackType.None)
                {
                    if (m_SelectedIndex == e.Row.RowIndex)
                        e.Row.CssClass = "TableSelectRowA";
                    else
                    {
                        if ((e.Row.RowIndex % 2) == 0)
                            e.Row.CssClass = "TableRowA";
                        else
                            e.Row.CssClass = "TableRowA2";
                    }
                }
                else
                {
                    if ((e.Row.RowIndex % 2) == 0)
                        e.Row.CssClass = "TableNoSelectRowA";
                    else
                        e.Row.CssClass = "TableNoSelectRowA2";
                }

                // Right padd the last column cause the scrollbar covers the data.
                // Have to do it to the last visible column!

                //e.Row.Cells[this.Columns.Count - 1].Style.Add("padding-right", "20px");
                for (iC = this.Columns.Count-1; iC >= 0; iC--)
                {
                    if (Object.ReferenceEquals((this.Columns[iC]).GetType(), gbField.GetType()))
                    {
                        gbField = (GridBoundField)(this.Columns[iC]);

                        if (!gbField.ItemStyle.CssClass.Equals("GridHiddenColumn"))
                        {
                            e.Row.Cells[iC].Style.Add("padding-right", "20px");
                            break;
                        }
                    }
                }

                for (iC = 0; iC < this.Columns.Count; iC++)
                {
                    if ( Object.ReferenceEquals( (this.Columns[iC]).GetType(), gbField.GetType()) )
                    {
                        gbField = (GridBoundField)(this.Columns[iC]);

                        if (gbField.SumColumn)
                        {
                            if (!e.Row.Cells[iC].Text.Equals("&nbsp;"))
                                gbField.SumValue += double.Parse(e.Row.Cells[iC].Text);
                        }

                        if (gbField.FormatString.Length > 0)
                            e.Row.Cells[iC].Text = doCellFormat(e.Row.Cells[iC].Text, gbField);

                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                for (iC = 0; iC < this.Columns.Count; iC++)
                {
                    if (Object.ReferenceEquals((this.Columns[iC]).GetType(), gbField.GetType()))
                    {
                        gbField = (GridBoundField)(this.Columns[iC]);

                        if (gbField.ItemStyle.CssClass.Equals("GridHiddenColumn"))
                            e.Row.Cells[iC].CssClass = "GridHiddenColumn";
                        else
                            e.Row.Cells[iC].CssClass = "TableHeaderB";
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                int firstVisible = 0;
                for (iC = 0; iC < this.Columns.Count; iC++)
                {
                    if ( Object.ReferenceEquals( (this.Columns[iC]).GetType(), gbField.GetType()) )
                    {
                        gbField = (GridBoundField)(this.Columns[iC]);

                        if (gbField.ItemStyle.CssClass.Equals("GridHiddenColumn"))
                            e.Row.Cells[iC].CssClass = "GridHiddenColumn";
                        else
                        {
                            e.Row.Cells[iC].CssClass = "TableFooterA";
                            firstVisible++;
                        }

                        if (firstVisible == 1)
                        {
                            if (m_TotalsFooter)
                            {
                                e.Row.Cells[iC].Text = "Total";
                                e.Row.Cells[iC].HorizontalAlign = HorizontalAlign.Left;
                            }
                        }
                        else if (gbField.SumColumn && (firstVisible != 0 || firstVisible != 1))
                        {
                            e.Row.Cells[iC].Text = gbField.SumValue.ToString(gbField.FormatString); ;
                            e.Row.Cells[iC].HorizontalAlign = gbField.ItemStyle.HorizontalAlign;
                        }
                    }
                }

                // Right padd the last column cause the scrollbar covers the data.
                // Have to do it to the last visible column!

                //e.Row.Cells[this.Columns.Count - 1].Style.Add("padding-right", "20px");
                for (iC = this.Columns.Count - 1; iC >= 0; iC--)
                {
                    if (Object.ReferenceEquals((this.Columns[iC]).GetType(), gbField.GetType()))
                    {
                        gbField = (GridBoundField)(this.Columns[iC]);

                        if (!gbField.ItemStyle.CssClass.Equals("GridHiddenColumn"))
                        {
                            e.Row.Cells[iC].Style.Add("padding-right", "20px");
                            break;
                        }
                    }
                }
            }
        }

        public void DoPreBind()
        {
            myInit();
        }

        private string doCellFormat(string text, GridBoundField gbField)
        {
            if (text.Length == 0 || text.Equals("&nbsp;"))
                if (gbField.FormatType == GridFormatType.GridDate)
                    return "-";
                else
                    return text;

            switch (gbField.FormatType)
            {
                case GridFormatType.GridString:
                    return text;

                case GridFormatType.GridNumber:
                case GridFormatType.GridCurrency:
                case GridFormatType.GridRate:
                    return double.Parse(text).ToString(gbField.FormatString);

                case GridFormatType.GridDate:
                    return DateTime.Parse(text, culture).ToString(gbField.FormatString);
            }
            return text;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.DesignMode)
                myInit();

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLGridView_PreRender);
        }

        private void SAHLGridView_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        private void myInit()
        {
            if (m_SelectFirstRow)
                this.SelectedIndex = 0;
            else
            {
                this.SelectedIndex = -1;
                m_SelectedIndex = -1;
            }

            if (Page.Request.Form["__EVENTTARGET"] != null)
            {
                if (Page.Request.Form["__EVENTTARGET"].Equals(this.UniqueID))
                {
                    string[] arg = Page.Request.Form["__EVENTARGUMENT"].Split('$');
                    if (arg[0].Equals("Select"))
                    {
                        m_SelectedIndex = int.Parse(arg[1]);
                    }
                    else if (arg[0].Equals("SelectDBL"))
                    {
                        m_SelectedIndex = int.Parse(arg[1]);
                        m_FireEvents = true;
                    }
                }
                else
                {
                    checkClientSelect();
                }
            }
            else
            {
                checkClientSelect();
            }
        }

        private void checkClientSelect()
        {
            // Check to see if a different row selection was made clientside.
            if (Page.Request.Form[this.UniqueID + "_SELECTED"] != null)
            {
                string clientRow = Page.Request.Form[this.UniqueID + "_SELECTED"];
                if (this.SelectedIndex != int.Parse(clientRow))
                {
                    m_SelectedIndex = int.Parse(clientRow);
                    m_SetSelectedIndex = true;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (m_FireEvents)
            {
                this.SelectedIndex = m_SelectedIndex;

                if (GridDoubleClick != null)
                {
                    GridSelectEventArgs args = new GridSelectEventArgs(m_SelectedIndex);
                    GridDoubleClick(this, args);
                }
            }

            if (m_SetSelectedIndex) // User has made a client side selection.
                this.SelectedIndex = m_SelectedIndex;
            //else
            //{
            //    if (m_SelectedIndex == 0)
            //    {
            //        if (m_SelectFirstRow)
            //            this.SelectedIndex = 0;
            //        else
            //        {
            //            m_SelectedIndex = -1;
            //            this.SelectedIndex = -1;
            //        }
            //    }
            //}
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterCommonScript();
        }


        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLGridView);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLGridViewScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLGridView.js");
                cs.RegisterClientScriptInclude(type, "SAHLGridViewScript", url);
            }

        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            //if (!this.AutoGenerateColumns)
            if ( this.m_FixedHeader )
                if (this.HeaderRow != null)
                    this.HeaderRow.Visible = false;

            // The internal panel must cover 100% of the parent area
            // irrespective of the physical width. We change this here
            // so that the original Panel code doesn't reflect the
            // external width.
            //Width = m_Width;
            Unit oldWidth = Width;
            Width = Unit.Percentage(100);

            // Capture the default output of the Grid
            StringWriter writer = new StringWriter();
            HtmlTextWriter buffer = new HtmlTextWriter(writer);
            base.RenderContents(buffer);
            string gridOutput = writer.ToString();

            // Restore the wanted width because this affects the outer table
            Width = oldWidth;

            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            cell.Text = HeaderCaption;
            cell.Visible = HeaderVisible;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.CssClass = "TableHeaderA";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            if (m_FixedHeader)
            {
                if (this.Rows.Count > 0)
                {
                    row = new TableRow();
                    cell = new TableCell();
                    cell.Controls.Add(makeColumnHeaders());
                    cell.CssClass = "TableHeaderBBack";
                    row.Cells.Add(cell);
                    table.Rows.Add(row);
                }
            }

            row = new TableRow();
            cell = new TableCell();
            Panel panel = new Panel();
            panel.Height = m_Height;
            panel.Style.Add("overflow-y", "scroll");
            if ( ScrollX )
                panel.Style.Add("overflow-x", "scroll");
            else
                panel.Style.Add("overflow-x", "hidden");
            Literal literal = new Literal();
            if (this.DataSource == null && NullDataSetMessage.Length > 0)
                literal.Text = "<center><br><b>" + NullDataSetMessage + "</b></center>";
            else if ((this.DataSource != null) && (this.Rows.Count == 0) && (EmptyDataSetMessage.Length > 0))
                literal.Text = "<center><br><b>" + EmptyDataSetMessage + "</b></center>";
            else
                literal.Text = gridOutput;
            panel.Controls.Add(literal);
            panel.Width = m_Width;
            cell.Controls.Add(panel);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.CssClass = "TableFrame";
            table.Style["border"] = "0";
            if (m_CellsWidth == 0)
                table.Width = GridWidth;
            else
                table.Width = Unit.Pixel(m_CellsWidth);

            Panel allPanel = new Panel();
            //allPanel.Style["background-color"] = "#F3F3F6";
            //allPanel.Style["border"] = "1px solid #E5E5E5";
            //allPanel.Style["padding"] = "0px";
            allPanel.CssClass = "TableDiv";
            allPanel.Style["width"] = GridWidth.ToString();
            if (m_Invisible)
                allPanel.Style["display"] = "none";
            allPanel.Controls.Add(table);

            generateGridScript();

            HiddenField hidden = new HiddenField();
            hidden.ID = this.UniqueID + "_SELECTED";
            hidden.Value = m_SelectedIndex.ToString();
            allPanel.Controls.Add(hidden);
            allPanel.RenderControl(output);
        }

        private void generateGridScript()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Rows.Count > 0 && PostBackType != GridPostBackType.None)
            {
                sb.AppendLine("    var o" + this.ID + "Row = document.getElementById('" + this.UniqueID + "_" + this.SelectedIndex + "');");
                sb.AppendLine("    if ( o" + this.ID + "Row != null ){");
                sb.AppendLine("        o" + this.ID + "Row.scrollIntoView(false);");
                sb.AppendLine("        }");

                if (!Page.ClientScript.IsStartupScriptRegistered("GridFocusScript"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "GridFocusScript", sb.ToString(), true);
            }
        }

        private Table makeColumnHeaders()
        {
            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            if (this.Rows.Count > 0)
            {
                foreach (DataControlFieldCell dcfCell in this.Rows[0].Cells)
                {
                    if (!dcfCell.CssClass.Equals("GridHiddenColumn")) // Don't create for hidden columns.
                    {
                        cell = new TableCell();
                        cell.Text = dcfCell.ContainingField.HeaderText;
                        if (dcfCell.Width.Type == UnitType.Pixel)
                            cell.Width = Unit.Pixel((int)dcfCell.ContainingField.ItemStyle.Width.Value - 4);
                        else if (dcfCell.Width.Type == UnitType.Percentage)
                            cell.Width = Unit.Pixel(getPixels(dcfCell.ContainingField.ItemStyle.Width.Value) - 4);
                        else
                            cell.Width = dcfCell.ContainingField.ItemStyle.Width;
                        m_CellsWidth += (int)cell.Width.Value;
                        cell.CssClass = "TableHeaderB";
                        row.Cells.Add(cell);
                    }
                }
            }

            table.Rows.Add(row);
            table.CellPadding = 0;
            table.CellSpacing = 0;

            return table;
        }

        private int getPixels(double percentage)
        {
            return (int)((percentage / 100.0) * m_Width.Value);
        }

        public void AddGridBoundColumn(string dataField, string headerText, Unit width, HorizontalAlign horzAlign, bool display)
        {
            if (display)
                AddGridBoundColumn(dataField, "", GridFormatType.GridString, headerText, false, width, horzAlign, "", "");
            else
                AddGridBoundColumn(dataField, "", GridFormatType.GridString, headerText, false, width, horzAlign, "GridHiddenColumn", "GridHiddenColumn");
        }

        public void AddGridBoundColumn(string dataField, string formatString, GridFormatType formatType, string headerText, bool sumColumn, Unit width, HorizontalAlign horzAlign, bool display)
        {
            if (display)
                AddGridBoundColumn(dataField, formatString, formatType, headerText, sumColumn, width, horzAlign, "", "");
            else
                AddGridBoundColumn(dataField, formatString, formatType, headerText, sumColumn, width, horzAlign, "GridHiddenColumn", "GridHiddenColumn");
        }

        public void AddGridBoundColumn(string dataField, string formatString, GridFormatType formatType, string headerText, bool sumColumn, Unit width, HorizontalAlign horzAlign, string itemCss, string headerCss)
        {
            GridBoundField boundField = new GridBoundField();
            boundField.DataField = dataField;
            if (formatString.Length > 0)
            {
                boundField.FormatString = formatString;
                boundField.FormatType = formatType;
            }
            boundField.HeaderText = headerText;
            boundField.SumColumn = sumColumn;
            boundField.ItemStyle.Width = width;
            boundField.ItemStyle.HorizontalAlign = horzAlign;
            if (itemCss.Length > 0)
                boundField.ItemStyle.CssClass = itemCss;
            if (headerCss.Length > 0)
                boundField.HeaderStyle.CssClass = headerCss;
            this.Columns.Add(boundField);
        }

        #region AddCheckBoxColumn - Added by Craig Fraser 07/09/2006
        public void AddCheckBoxColumn(string columnName, string headerText, bool selectAllOption, Unit width, HorizontalAlign horzAlign, bool display)
        {
            if (display)
                AddCheckBoxColumn(columnName, headerText, selectAllOption, width, horzAlign, "", "");
            else
                AddCheckBoxColumn(columnName, headerText, selectAllOption, width, horzAlign, "GridHiddenColumn", "GridHiddenColumn");
        }
       
        private void AddCheckBoxColumn(string columnName, string headerText, bool selectAllOption ,Unit width, HorizontalAlign horzAlign, string itemCss, string headerCss)
        {
            TemplateField customField = new TemplateField();

            // Create the dynamic templates and assign them to the appropriate template property.

            // Setup the Item Template
            customField.ItemTemplate = new GridViewCheckBoxField(DataControlRowType.DataRow, columnName, selectAllOption);
            customField.ItemStyle.Width = width;
            customField.ItemStyle.HorizontalAlign = horzAlign;
            if (itemCss.Length > 0)
                customField.ItemStyle.CssClass = itemCss;

            // Setup the HeaderTemplate
            customField.HeaderTemplate = new GridViewCheckBoxField(DataControlRowType.Header, headerText, selectAllOption);
            customField.HeaderStyle.Width = width;
            customField.HeaderStyle.HorizontalAlign = horzAlign;
            if (headerCss.Length > 0)
                customField.HeaderStyle.CssClass = headerCss;
            else
                customField.HeaderStyle.CssClass = "TableHeaderB";

            // Add the Checkbox column to the Columns collection of the GridView control.
            this.Columns.Add(customField);
        }

        #endregion

        public void AddHyperLinkFieldColumn(string headerText, string showText, Unit width, HorizontalAlign horzAlign, string[] dataNavigateUrlFields, string dataNavigateUrlFormatString, string target, string itemCss, string headerCss)
        {
            HyperLinkField hlf = new HyperLinkField();

            hlf.DataNavigateUrlFields = dataNavigateUrlFields;
            hlf.DataNavigateUrlFormatString = dataNavigateUrlFormatString;
            hlf.HeaderText = headerText;
            hlf.Target = target;
            hlf.Text = showText;
            hlf.ItemStyle.Width = width;
            hlf.ItemStyle.HorizontalAlign = horzAlign;

            if (itemCss.Length > 0)
                hlf.ItemStyle.CssClass = itemCss;
            if (headerCss.Length > 0)
                hlf.HeaderStyle.CssClass = headerCss;
            else
                hlf.HeaderStyle.CssClass = "TableHeaderB";

            this.Columns.Add(hlf);

        }


        public void AddButtonColumn(string headerText, string showText, Unit width, HorizontalAlign horzAlign, string commandName, string itemCss, string headerCss)
        {
            AddButtonColumn(headerText, showText, null, ButtonType.Button, width, horzAlign, commandName, itemCss, headerCss);
        }
        public void AddButtonColumn(string headerText, string showText, string imageUrl, System.Web.UI.WebControls.ButtonType buttonType, Unit width, HorizontalAlign horzAlign, string commandName, string itemCss, string headerCss)
        {
            ButtonField bf = new ButtonField();

            bf.ButtonType = buttonType;
            bf.HeaderText = headerText;
            bf.CommandName = commandName;
            if (!String.IsNullOrEmpty(showText))
                bf.Text = showText;
            if (!String.IsNullOrEmpty(imageUrl))
                bf.ImageUrl = imageUrl;
            bf.ItemStyle.Width = width;
            bf.ItemStyle.HorizontalAlign = horzAlign;

            bf.ControlStyle.CssClass = "buttonSpacer";

            if (itemCss.Length > 0)
                bf.ItemStyle.CssClass = itemCss;
            if (headerCss.Length > 0)
                bf.HeaderStyle.CssClass = headerCss;
            else
                bf.HeaderStyle.CssClass = "TableHeaderB";

            this.Columns.Add(bf);

        }

        #region ISAHLSecurityControl Members

        /// <summary>
        /// Tag that identifies the security block in the control.  This should be unique 
        /// per object (view/presenter).
        /// </summary>
        [Category("Authentication")]
        [Description("The configuration security tag to apply to the control.")]
        public string SecurityTag
        {
            get
            {
                return _securityTag;
            }
            set
            {
                _securityTag = value;
            }
        }

        /// <summary>
        /// Gets/sets what happens to the control when authentication fails.  This 
        /// defaults to <see cref="SAHLSecurityDisplayType.Hide"/>
        /// </summary>
        [Category("Authentication")]
        [DefaultValue(SAHLSecurityDisplayType.Hide)]
        [Description("Specifies what happens to the control when authentication fails.")]
        public SAHLSecurityDisplayType SecurityDisplayType
        {
            get
            {
                return _securityDisplayType;
            }
            set
            {
                _securityDisplayType = value;
            }
        }

        /// <summary>
        /// Gets/sets what happens to the control when authentication fails.  This 
        /// defaults to <see cref="SAHLSecurityHandler.Automatic"/>
        /// </summary>
        [Category("Authentication")]
        [DefaultValue(SAHLSecurityHandler.Automatic)]
        [Description("Specifies whether a custom implementation of security exists or if security is automatic.")]
        public SAHLSecurityHandler SecurityHandler
        {
            get
            {
                return _securityHandler;
            }
            set
            {
                _securityHandler = value;
            }
        }

        /// <summary>
        /// Occurs when the control tries to authenticate i.e. ensure that all security 
        /// restrictions have been passed.
        /// </summary>
        public event SAHLSecurityControlEventHandler Authenticate;

        #endregion

    }

    public class GridBoundField : BoundField
    {
        private string m_FormatString = "";
        private GridFormatType m_FormatType = GridFormatType.GridString;
        private bool m_SumColumn = false;
        private double m_SumValue = 0.0;

        public string FormatString
        {
            get { return m_FormatString; }
            set { m_FormatString = value; }
        }

        public GridFormatType FormatType
        {
            get { return (GridFormatType)m_FormatType; }
            set { m_FormatType = value; }
        }

        public bool SumColumn
        {
            get { return m_SumColumn; }
            set { m_SumColumn = value; }
        }

        public double SumValue
        {
            get { return m_SumValue; }
            set { m_SumValue = value; }
        }
    }

    public class GridSelectEventArgs : EventArgs
    {
        public GridSelectEventArgs(int rowNum)
        {
            this.m_RowNum = rowNum;
        }

        public int m_RowNum;
    }

    public enum GridPostBackType
    {
        None = 0,
        NoneWithClientSelect = 1,
        SingleClick = 2,
        DoubleClick = 3,
        DoubleClickWithClientSelect = 4,
        SingleAndDoubleClick = 5,

    }

    public enum GridFormatType
    {
        GridString = 0,
        GridCurrency = 1,
        GridDate = 2,
        GridNumber = 3,
        GridRate = 4,
        GridDateTime = 5,
        GridRate3Decimal = 6
    }

    #region Create a template class to represent a CheckBox template column. Added by Craig Fraser 07/09/2006
    public class GridViewCheckBoxField : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private bool selectAllOption;

        public GridViewCheckBoxField(DataControlRowType type, string colname, bool selectalloption)
        {
            templateType = type;
            columnName = colname;
            selectAllOption = selectalloption;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.Header:
                    // Create the controls to put in the header section and set their properties.

                    Literal lc = new Literal();
                    lc.Text = columnName + "</br>";

                    CheckBox chkBoxHeader = new CheckBox();
                    chkBoxHeader.ID = columnName;
                    chkBoxHeader.ToolTip = "Click here to select/deselect all rows";
                    chkBoxHeader.Attributes.Add("onclick", "SAHLGridView_SelectAllCheckboxes(this)");

                    if (selectAllOption == false || columnName.Length > 0)
                        container.Controls.Add(lc);

                    if (selectAllOption)
                        container.Controls.Add(chkBoxHeader);

                    break;
                case DataControlRowType.DataRow:
                    // Create the controls to put in a data row
                    // section and set their properties.
                    CheckBox chkBoxRow = new CheckBox();
                    chkBoxRow.ID = columnName;

                    // Add the controls to the Controls collection
                    // of the container.
                    container.Controls.Add(chkBoxRow);
                    break;

                // Insert cases to create the content for the other 
                // row types, if desired.

                default:
                    // Insert code to handle unexpected values.
                    break;
            }
        }
    }
    #endregion


}
