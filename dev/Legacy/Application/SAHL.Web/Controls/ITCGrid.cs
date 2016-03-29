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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Common ITC display grid
    /// </summary>
    public class ITCGrid : SAHLGridView
    {
        /// <summary>
        /// Defines all columns used in the <see cref="ITCGrid"/>.
        /// </summary>
        public enum GridColumns
        {
            Key = 0,
            AccountKey,
            LegalEntityKey,
            DisplayName,
            IDNumber,
            ResponseStatus,
            ChangeDate,
            UserID,
            View,
            DoEnquiry,
            ViewHistory,
            ArchiveCount,
            RedoITC,
            PerformITC,
            IsHistory
        }

        /// <summary>
        /// 
        /// </summary>
        public ITCGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "ITC Results";
            EmptyDataSetMessage = "No legal entity natural persons found.";
            NullDataSetMessage = EmptyDataSetMessage;
            EmptyDataText = EmptyDataSetMessage;
            RowStyle.CssClass = "TableRowA";
            GridWidth = Unit.Percentage(100);
            Width = Unit.Percentage(95);
            ShowFooter = false;

            if (!DesignMode)
            {
                string[] dataNavigateUrlFields = { "Key", "IsHistory" };
                // add the columns to the grid
                this.AddGridBoundColumn("Key", "ITCKey", Unit.Empty, HorizontalAlign.Left, false);
                this.AddGridBoundColumn("AccountKey", "Account Key", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("LegalEntityKey", "Legal Entity Key", Unit.Empty, HorizontalAlign.Left, false);
                this.AddGridBoundColumn("DisplayName", "Name", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("IDNumber", "IDNumber", Unit.Pixel(110), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("ResponseStatus", "Status", Unit.Pixel(100), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("ChangeDate", "Enquiry Date", Unit.Pixel(100), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("UserID", "User", Unit.Pixel(50), HorizontalAlign.Right, true);
                this.AddHyperLinkFieldColumn("View", "View", Unit.Pixel(50), HorizontalAlign.Center, dataNavigateUrlFields, "ITCReport.aspx?ITCKey={0}&History={1}", "_blank", "", ""); 
                this.AddCheckBoxColumn("DoEnquiry", "Do Enquiry", true, Unit.Pixel(70), HorizontalAlign.Center, true);
                this.AddButtonColumn("History", "View", Unit.Pixel(50), HorizontalAlign.Center, "ViewHistory", "", "");
                this.AddGridBoundColumn("ArchiveCount", "ArchiveCount", Unit.Empty, HorizontalAlign.Left, false);
                this.AddGridBoundColumn("RedoITC", "RedoITC", Unit.Empty, HorizontalAlign.Left, false);
                this.AddGridBoundColumn("PerformITC", "PerformITC", Unit.Empty, HorizontalAlign.Left, false);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the grid
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[(int)GridColumns.Key].Text == "0")
                {
                    e.Row.Cells[(int)GridColumns.ResponseStatus].Text = "No ITC done";
                    e.Row.Cells[(int)GridColumns.ChangeDate].Text = "";
                    e.Row.Cells[(int)GridColumns.UserID].Text = "";
                    e.Row.Cells[(int)GridColumns.View].Text = "";
                }

                if (e.Row.Cells[(int)GridColumns.ArchiveCount].Text == "0")
                {
                    e.Row.Cells[(int)GridColumns.ViewHistory].Text = "None";
                }

                if (String.Compare(e.Row.Cells[(int)GridColumns.RedoITC].Text, "false", true) == 0)
                {
                    e.Row.Cells[(int)GridColumns.DoEnquiry].Text = "";
                }

                // Nazir J  - 2008.07.30
                // If client OfferDeclarationAnswer is "NO" to OfferDeclarationQuestion then dont allow ITC Check
                if (String.Compare(e.Row.Cells[(int)GridColumns.PerformITC].Text, "false", true) == 0)
                {
                    e.Row.Cells[(int)GridColumns.DoEnquiry].Enabled = false;
                    e.Row.Cells[(int)GridColumns.ResponseStatus].Text = "ITC Not Allowed";
                }
            }
        }

        /// <summary>
        /// Binds a collection of <see cref="SAHL.Common.BusinessModel.Interfaces.IITC"/> entities to the grid.
        /// </summary>
        /// <param name="itc"></param>
        public void BindITCList(List<BindableITC> itc)
        {
            DataSource = itc;
            DataBind();
        }

        /// <summary>
        /// Gets/sets whether the History link column is visible on the grid.
        /// </summary>
        public bool ViewHistoryColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.ViewHistory].Visible;
            }
            set
            {
                Columns[(int)GridColumns.ViewHistory].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Do Enquiry column is visible on the grid.
        /// </summary>
        public bool DoEnquiryColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.DoEnquiry].Visible;
            }
            set
            {
                Columns[(int)GridColumns.DoEnquiry].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Account Key column is visible on the grid.
        /// </summary>
        public bool AccountColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.AccountKey].Visible;
            }
            set
            {
                Columns[(int)GridColumns.AccountKey].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the ResponseStatus column is visible on the grid.
        /// </summary>
        public bool StatusColumnVisible
        {
            get
            {
                return Columns[(int)GridColumns.ResponseStatus].Visible;
            }
            set
            {
                Columns[(int)GridColumns.ResponseStatus].Visible = value;
            }
        }
    }
}
