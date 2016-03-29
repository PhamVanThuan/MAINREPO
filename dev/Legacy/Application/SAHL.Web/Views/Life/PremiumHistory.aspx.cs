using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Life
{
    /// <summary>
    ///
    /// </summary>
    public partial class PremiumHistory : SAHLCommonBaseView, IPremiumHistory
    {
        #region IPremiumHistory Members

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Controls visibility of Cancel Button
        /// </summary>
        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Life Workflow Header control
        /// </summary>
        public bool ShowLifeWorkFlowHeader
        {
            set
            {
                WorkFlowHeader1.Visible = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lstLifePremiumHistory"></param>
        public void BindPremiumHistoryGrid(IList<ILifePremiumHistory> lstLifePremiumHistory)
        {
            gridPremiumHistory2.SettingsPager.PageSize = 20;
            AddGridColumn(gridPremiumHistory2, "LifePremiumHistoryKey", "LifePremiumHistoryKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            AddGridColumn(gridPremiumHistory2, "ChangeDate", "Date", 18, GridFormatType.GridDateTime, "dd/MM/yyyy HH:mm:ss", HorizontalAlign.Left, true, true);
            AddGridColumn(gridPremiumHistory2, "", "User", 10, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            AddGridColumn(gridPremiumHistory2, "DeathPremium", "Death Prem", 10, GridFormatType.GridCurrency, null, HorizontalAlign.Right, true, true);
            AddGridColumn(gridPremiumHistory2, "IPBPremium", "IPB Prem", 10, GridFormatType.GridCurrency, null, HorizontalAlign.Right, true, true);
            AddGridColumn(gridPremiumHistory2, "SumAssured", "Sum Assured", 10, GridFormatType.GridCurrency, null, HorizontalAlign.Right, true, true);
            AddGridColumn(gridPremiumHistory2, "YearlyPremium", "Yearly Prem", 10, GridFormatType.GridCurrency, null, HorizontalAlign.Right, true, true);
            AddGridColumn(gridPremiumHistory2, "MonthlyPremium", "Monthly Prem", 10, GridFormatType.GridCurrency, null, HorizontalAlign.Right, true, true);
            AddGridColumn(gridPremiumHistory2, "PolicyFactor", "Policy Factor", 12, GridFormatType.GridString, null, HorizontalAlign.Center, true, true);
            AddGridColumn(gridPremiumHistory2, "DiscountFactor", "Discount Factor", 14, GridFormatType.GridString, null, HorizontalAlign.Center, true, true);
            gridPremiumHistory2.Settings.ShowGroupPanel = false;

            gridPremiumHistory2.DataSource = lstLifePremiumHistory;
            gridPremiumHistory2.DataBind();
        }

        #endregion IPremiumHistory Members

        private static void AddGridColumn(SAHL.Common.Web.UI.Controls.DXGridView gridview, string fieldName, string caption, int width, GridFormatType formatType, string format, HorizontalAlign align, bool visible, bool readOnly)
        {
            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = fieldName;
            col.Caption = caption;
            col.Width = Unit.Percentage(width);
            col.Format = formatType;
            if (!String.IsNullOrEmpty(format))
                col.FormatString = format;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            col.ReadOnly = readOnly;
            gridview.Columns.Add(col);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridPremiumHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                // Get the LifePremiumHistory Row
                ILifePremiumHistory ph = e.Row.DataItem as ILifePremiumHistory;

                //Date
                e.Row.Cells[1].Text = ph.ChangeDate.ToString(SAHL.Common.Constants.DateFormat + " HH:mm:ss");

                e.Row.Cells[2].Text = ph.UserName == null ? "" : ph.UserName;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }
    }
}