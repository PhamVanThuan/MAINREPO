using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.Datasets;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common;
using System.Xml;

using Microsoft.ApplicationBlocks.UIProcess;

[assembly: TagPrefix("SAHL.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxBitmap(typeof(CoreBusinessObjectHeader), "Resources.CoreBusinessObjectNavigator.bmp")]
    [ToolboxData("<{0}:CoreBusinessObjectHeader runat=server></{0}:CoreBusinessObjectHeader>")]
    public class CoreBusinessObjectHeader : WebControl
    {
        private CBO m_Cbo;
        private CoreBusinessObjectNavigator m_CboNavigator;
        private string m_HeaderDescription;

        private SAHLTabStrip m_TabStrip;
        private TableCell m_CellTabStrip;
        private int m_TabStripIndent = 0;

        /// <summary>
        /// 
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public CBO DataSource
        {
            get { return m_Cbo; }
            set { m_Cbo = value; }
        }

        /// <summary>
        /// Gets/sets the navigator - this is required for the building of the tab strip, as this is only 
        /// displayed if a node of type "Workflow Case" is currently selected.
        /// </summary>
        [Browsable(false)]
        public CoreBusinessObjectNavigator CboNavigator
        {
            get
            {
                return m_CboNavigator;
            }
            set
            {
                m_CboNavigator = value;
            }
        }

        // TODO: FIX after NEW UIP
        ///// <summary>
        ///// Gets a reference to the current controller.  This is used by the tabstrip for displaying the 
        ///// correct tab.
        ///// </summary>
        //[Browsable(false)]
        //private ControllerBase CurrentController 
        //{
        //    get 
        //    {
        //        return ((SAHLCommonBaseView)this.Page).Controller;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string HeaderDescription
        {
            get { return m_HeaderDescription; }
            set { m_HeaderDescription = value; }
        }

        public int TabStripIndent
        {
            get
            {
                return m_TabStripIndent;
            }
            set
            {
                m_TabStripIndent = value;
            }
        }

        /// <summary>
        /// Adds a tab item to the tab strip displayed on the header.  The tab item will not be selected.
        /// </summary>
        /// <param name="item">The tab item to add.</param>
        public void AddTabItem(SAHLTabItem item)
        {
            AddTabItem(item, false);
        }

        /// <summary>
        /// Adds a tab item to the tab strip displayed on the header.
        /// </summary>
        /// <param name="item">The tab item to add.</param>
        /// <param name="selected">If set to true, this tab becomes the selected tab.</param>
        public void AddTabItem(SAHLTabItem item, bool selected)
        {
            m_TabStrip.Tabs.Add(item);
            if (selected) m_TabStrip.SelectedTab = item;
        }

        private void generateIcons(ref TableCell cell)
        {
            if (DataSource != null)
            {
                if (
                    (DataSource.CboProperties.Rows.Count == 1) &&
                    (DataSource.CboProperties[0].SelectedItem.Length > 0)
                    )
                {
                    CGenericCboNode iconNode = CGenericCboNode.Construct(DataSource.CboProperties[0].SelectedItem) as CGenericCboNode;

                    if (iconNode != null)
                    {
                        string szFilter = "UserSelectsKey=" + iconNode.UserSelectsKey.ToString();
                        DataRow[] drHM = DataSource.HeaderMenu.Select(szFilter, "");

                        foreach (CBO.HeaderMenuRow hmRow in drHM)
                        {
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.ImageUrl = "~/Images/" + hmRow.Icon;
                            img.ToolTip = hmRow.Description;
                            cell.Controls.Add(img);
                        }
                        if (iconNode.includeParentIcons)
                            LoadParentIcons(iconNode, ref cell);
                    }
                }
            }
        }


        /// <summary>
        /// Load the icons for the parent node
        /// </summary>
        /// <param name="menuNode"></param>
        /// <param name="cell"></param>
        private void LoadParentIcons(CGenericCboNode menuNode, ref TableCell cell)
        {
            string szFilter = "UserSelectsKey=" + menuNode.ParentKey.ToString();
            DataRow[] dr = DataSource.UserSelects.Select(szFilter, "");

            foreach (CBO.UserSelectsRow userRow in dr)
            {
                szFilter = "UserSelectsKey=" + userRow.UserSelectsKey.ToString();
                DataRow[] drHM = DataSource.HeaderMenu.Select(szFilter, "");

                foreach (CBO.HeaderMenuRow hmRow in drHM)
                {
                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                    img.ImageUrl = "~/Images/" + hmRow.Icon;
                    img.ToolTip = hmRow.Description;
                    cell.Controls.Add(img);
                }

                if (!userRow.IsParentKeyNull())
                    LoadParentIcons(new CGenericCboNode(null, userRow), ref cell);
            }

        }

        /// <summary>
        /// Overridden to include a call to create the table for the header.  This must be done here so 
        /// that postback information is available when a tab is clicked.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateTableControls();
        }

        /// <summary>
        /// Overridden to determine display of tabstrip.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // only show the tabstrip if there are tabs to display
            if (m_TabStrip.TabCount > 0)
            {
                // add indenting to the tabstrip if it has been set
                if (TabStripIndent > 0)
                {
                    m_CellTabStrip.Style.Add(HtmlTextWriterStyle.PaddingLeft, TabStripIndent.ToString() + "px");
                }
            }
            else
            {
                m_CellTabStrip.Controls.Remove(m_TabStrip);
            }
        }

        /// <summary>
        /// Creates the controls displayed on the header.
        /// </summary>
        protected void CreateTableControls()
        {
            //TODO mm
            /*
              Comment this block out to stop the display of the timing information on the screeen  
            */
            //if (DataSource != null)
            //{
            //    if (DataSource.CboProperties[0].InformationDisplay.Length > 0)
            //    {
            //        cell.CssClass = "CboHeaderText";
            //        cell.Text = DataSource.CboProperties[0].InformationDisplay.Trim();
            //        cell.BackColor = Color.Azure;
            //        row.Cells.Add(cell);
            //        table.Rows.Add(row);

            //        row = new TableRow();
            //        cell = new TableCell();
            //    }
            //}

            // To here

            // create the main table
            Table table = new Table();
            table.CellSpacing = 0;
            table.CellPadding = 0;
            table.HorizontalAlign = HorizontalAlign.Left;
            table.Width = Unit.Percentage(100);
            table.Height = Unit.Percentage(100);
            this.Controls.Add(table);

            // the top row and cell of the table
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Height = Unit.Percentage(1);
            cell.CssClass = "CboHeaderText";
            cell.Text = m_HeaderDescription;
            row.Cells.Add(cell);
            table.Rows.Add(row);

            // add the second row (the main area displayed)
            row = new TableRow();
            cell = new TableCell();
            cell.Height = Unit.Percentage(99);
            cell.CssClass = "CboHeaderTable";
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //

            // generate a new internal table that contains the icons and the tabstrip
            Table tableInner = new Table();
            tableInner.CellSpacing = 0;
            tableInner.CellPadding = 0;
            tableInner.HorizontalAlign = HorizontalAlign.Left;
            tableInner.Width = Unit.Percentage(100);
            tableInner.Height = Unit.Percentage(100);
            cell.Controls.Add(tableInner);

            // icons row
            row = new TableRow();
            tableInner.Rows.Add(row);
            cell = new TableCell();
            generateIcons(ref cell);
            row.Cells.Add(cell);

            // tabstrip row
            row = new TableRow();
            tableInner.Rows.Add(row);
            m_CellTabStrip = new TableCell();
            m_CellTabStrip.VerticalAlign = VerticalAlign.Bottom;
            row.Cells.Add(m_CellTabStrip);

             //
            // create the tabstrip - this will get added to m_CellTabStrip during OnPreRender if there 
            // are tabs to display
            m_TabStrip = new SAHLTabStrip();
            m_TabStrip.ID = "CBOHeaderTabStrip";
            m_TabStrip.TabClick += new SAHLTabStripEventHandler(m_TabStrip_TabClick);
            m_CellTabStrip.Controls.Add(m_TabStrip);


        }

        /// <summary>
        /// Event handler for the tab strip.  All this does is navigate to the view associated with the selected tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_TabStrip_TabClick(object sender, EventArgs e)
        {
            SAHLTabItem tabItem = ((SAHLTabItem)sender);


            foreach (CBO.WorkflowContextMenuRow dataRow in this.m_Cbo.WorkflowContextMenu)
            {
                if (dataRow.MenuType == "F" && dataRow.Description == tabItem.Text)
                {
                    CboNavigator.SelectedWflCtxItem = dataRow;
                    break;
                }
            }

// TODO : FIX AFTER NEW 
            
//            ((SAHLCommonViewBase)this.Page).Controller.Navigator.Navigate(tabItem.NavigationValue);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                sb.Append("<tr><td class='CboHeaderLeft'>Icons</td>");
                sb.Append("    <td class='CboHeaderRight'>Logo</td></tr>");
                sb.Append("</table>");
                output.Write(sb.ToString());
            }
            else
            {
                EnsureChildControls();
                base.RenderContents(output);
            }
        }
    }
}
