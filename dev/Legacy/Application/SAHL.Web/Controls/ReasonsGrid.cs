using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Grid used to display Reasons.
    /// </summary>
    /// 
    public class ReasonsGrid : SAHLGridView
    {
        private IReason _reason;

        /// <summary>
        /// Defines all columns used in the <see cref="ReasonsGrid"/>.
        /// </summary>
        private enum GridColumns
        {
            Key = 0,
            Reason,
            Comment
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReasonsGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "";
            EmptyDataSetMessage = "No reasons found";
            NullDataSetMessage = EmptyDataSetMessage;
            EmptyDataText = EmptyDataSetMessage;
            PostBackType = GridPostBackType.None;
            RowStyle.CssClass = "TableRowA";
            GridWidth = Unit.Percentage(100);
            Width = Unit.Percentage(100);
            GridHeight = Unit.Pixel(100);

            // add the columns to the div
            if (!DesignMode)
            {
                this.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("", "Reason", Unit.Percentage(30), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Comment", Unit.Percentage(70), HorizontalAlign.Left, true);
            }
        }

        /// <summary>
        /// Binds a collection of <see cref="IReason"/> entities to the grid.
        /// </summary>
        /// <param name="reasons">IReadOnlyEventList&lt;IReason&gt;</param>
        /// 
        public void BindData(IReadOnlyEventList<IReason> reasons)
        {
            DataSource = reasons;
            DataBind();
        }

        /// <summary>
        /// Handles the RowDataBound event of the grid
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);

            if (DesignMode)
                return;

            if (e.Row.DataItem != null)
            {
                IReason reason = e.Row.DataItem as IReason;

                e.Row.Cells[(int)GridColumns.Key].Text = reason.GenericKey.ToString();
                e.Row.Cells[(int)GridColumns.Reason].Text = reason.ReasonDefinition.ReasonDescription.Description;
                e.Row.Cells[(int)GridColumns.Comment].Text = reason.Comment;
            }
        }

        /// <summary>
        /// Gets a reference to the currently selected <see cref="IReason"/>.  If no row is selected, a null is returned.
        /// </summary>
        public IReason SelectedReason
        {
            get
            {
                if (base.SelectedIndex > -1)
                {
                    _reason = (IReason)Rows[SelectedIndex].DataItem;
                }
                else
                    _reason = null;

                return _reason;
            }
        }
    }
}
