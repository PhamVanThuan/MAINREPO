using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Authentication;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using System;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Views_HOCHistory : SAHLCommonBaseView, IHOCHistoryView
    {
        /// <summary>
        /// RowDataBound event of HOCHistory Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HOCHistoryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IHOCHistory hocHistory = e.Row.DataItem as IHOCHistory;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = hocHistory.HOCInsurer.Description;
                cells[1].Text = hocHistory.CommencementDate.HasValue ? Convert.ToDateTime(hocHistory.CommencementDate).ToString(SAHL.Common.Constants.DateFormat) : string.Empty;
                cells[2].Text = hocHistory.CancellationDate.HasValue ? Convert.ToDateTime(hocHistory.CancellationDate).ToString(SAHL.Common.Constants.DateFormat) : string.Empty;
                cells[3].Text = Convert.ToDateTime(hocHistory.ChangeDate).ToString(SAHL.Common.Constants.DateFormat);
                cells[4].Text = hocHistory.UserID;
            }

        }
        /// <summary>
        /// RowDataBound event of HOCHistoryDetail Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HOCHistoryDetailGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection Cells = e.Row.Cells;

            IHOCHistoryDetail hocHistoryDetail = e.Row.DataItem as IHOCHistoryDetail;

            if (e.Row.DataItem != null)
            {
                Cells[0].Text = Convert.ToDateTime(hocHistoryDetail.EffectiveDate).ToString(SAHL.Common.Constants.DateFormat);
                if (hocHistoryDetail.UpdateType == Convert.ToChar("I"))
                    e.Row.Cells[1].Text = "Insert";
                if (hocHistoryDetail.UpdateType == Convert.ToChar("V"))
                    e.Row.Cells[1].Text = "Valuation";
                if (hocHistoryDetail.UpdateType == Convert.ToChar("U"))
                    e.Row.Cells[1].Text = "Update";
                if (hocHistoryDetail.UpdateType == Convert.ToChar("R"))
                    e.Row.Cells[1].Text = "Re-Index";
                Cells[2].Text = hocHistoryDetail.HOCThatchAmount.ToString();
                Cells[3].Text = hocHistoryDetail.HOCConventionalAmount.ToString();
                Cells[4].Text = hocHistoryDetail.HOCProrataPremium.ToString();
                Cells[5].Text = hocHistoryDetail.HOCMonthlyPremium.ToString();
                Cells[6].Text = hocHistoryDetail.PrintDate.ToString();
                Cells[7].Text = hocHistoryDetail.SendFileDate.ToString();
                Cells[8].Text = hocHistoryDetail.ChangeDate.ToString();
                Cells[9].Text = hocHistoryDetail.UserID;
            }
        }
        /// <summary>
        /// Selected Index change on HOC History grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HOCHistoryGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HOCHistoryGrid.SelectedIndex >= 0)
            {
                OnHOCHistoryGridsSelectedIndexChanged(sender, new KeyChangedEventArgs(HOCHistoryGrid.SelectedIndex));
            }
        }

        /// <summary>
        /// Event handler for change of Memo Grid Selected Index Change
        /// </summary>
        public event KeyChangedEventHandler OnHOCHistoryGridsSelectedIndexChanged;

        /// <summary>
        /// Bind HOC History Grid
        /// </summary>
        /// <param name="hocHistory"></param>
        public void BindHOCHistoryGrid( IEventList<IHOCHistory> hocHistory)
        {
            HOCHistoryGrid.Columns.Clear();
            HOCHistoryGrid.AddGridBoundColumn("", "HOC Insurer", Unit.Percentage(30), HorizontalAlign.Left, true);
            HOCHistoryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Commencement Date", false, Unit.Percentage(17), HorizontalAlign.Right, true);
            HOCHistoryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Cancellation Date", false, Unit.Percentage(17), HorizontalAlign.Right, true);
            HOCHistoryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Change Date", false, Unit.Percentage(17), HorizontalAlign.Right, true);
            HOCHistoryGrid.AddGridBoundColumn("", "User ID", Unit.Percentage(19), HorizontalAlign.Left, true);
            HOCHistoryGrid.DataSource = hocHistory;
            HOCHistoryGrid.DataBind();
        }
        /// <summary>
        /// Bind HOCHistory detail Grid
        /// </summary>
        /// <param name="hocHistoryDetail"></param>
        public void BindHOCDetailGrid(IEventList<IHOCHistoryDetail> hocHistoryDetail)
        {
            HOCHistoryDetailGrid.Columns.Clear();
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", "Update Type", Unit.Percentage(10), HorizontalAlign.Left, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Thatch Amount", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Conventional Amount", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Prorata Premium", false, Unit.Percentage(8), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Monthly Premium", false, Unit.Percentage(8), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Print Date", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Upload Date", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Change Date", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HOCHistoryDetailGrid.AddGridBoundColumn("", "User ID", Unit.Percentage(10), HorizontalAlign.Left, true);
            HOCHistoryDetailGrid.DataSource = hocHistoryDetail;
            HOCHistoryDetailGrid.DataBind();
        }
        /// <summary>
        /// Set PostBack Type on Grid
        /// </summary>
        public void SetPostBackType()
        {
            HOCHistoryGrid.PostBackType = GridPostBackType.SingleClick;
        }

    }
}

