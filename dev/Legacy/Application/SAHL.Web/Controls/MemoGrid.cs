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
    /// Grid used to display Memos.
    /// </summary>
    /// 
    public class MemoGrid : SAHLGridView
    {
        private IMemo _memo;

        /// <summary>
        /// Defines all columns used in the <see cref="MemoGrid"/>.
        /// </summary>
        private enum GridColumns
        {
            Key = 0,
            Description,
            ADUser,
            InsertedDate,
            GenericKey,
            GenericKeyType,
            ReminderDate,
            ExpiryDate,
            GeneralStatus
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MemoGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "";
            EmptyDataSetMessage = "No data found";
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
                this.AddGridBoundColumn("", "Description", Unit.Percentage(90), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "User", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("", "Inserted", Unit.Percentage(0), HorizontalAlign.Center, false);
                this.AddGridBoundColumn("", "GenericKey", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("", "GenericKeyTypeKey", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("", "ReminderDate", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("", "ExpiryDate", Unit.Percentage(0), HorizontalAlign.Left, false);
                this.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            }
        }

        /// <summary>
        /// Binds a collection of <see cref="IMemo"/> entities to the grid.
        /// </summary>
        /// <param name="memos">IEventList&lt;IMemo&gt;</param>
        /// 
        public void BindData(IEventList<IMemo> memos)
        {
            DataSource = memos;
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
                IMemo memo = e.Row.DataItem as IMemo;

                e.Row.Cells[(int)GridColumns.Key].Text = memo.Key.ToString();
                e.Row.Cells[(int)GridColumns.Description].Text = memo.Description;

                e.Row.Cells[(int)GridColumns.Description].ToolTip = "Inserted by : " + memo.ADUser.ADUserName + " on " + memo.InsertedDate.ToString(SAHL.Common.Constants.DateTimeFormat);

                e.Row.Cells[(int)GridColumns.GenericKey].Text = memo.GenericKey.ToString();
                e.Row.Cells[(int)GridColumns.GenericKeyType].Text = memo.GenericKeyType.Description;
                e.Row.Cells[(int)GridColumns.InsertedDate].Text = memo.InsertedDate.ToString(SAHL.Common.Constants.DateTimeFormat);
                e.Row.Cells[(int)GridColumns.ADUser].Text = memo.ADUser.ADUserName;
                e.Row.Cells[(int)GridColumns.ReminderDate].Text = memo.ReminderDate.HasValue ? memo.ReminderDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                e.Row.Cells[(int)GridColumns.ExpiryDate].Text = memo.ExpiryDate.HasValue ? memo.ExpiryDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                e.Row.Cells[(int)GridColumns.GeneralStatus].Text = memo.GeneralStatus.Description;
            }
        }

        /// <summary>
        /// Gets a reference to the currently selected <see cref="IMemo"/>.  If no row is selected, a null is returned.
        /// </summary>
        public IMemo SelectedMemo
        {
            get
            {
                if (base.SelectedIndex > -1)
                {
                    _memo = (IMemo)Rows[SelectedIndex].DataItem;
                }
                else
                    _memo = null;

                return _memo;
            }
        }
	
    }
}
