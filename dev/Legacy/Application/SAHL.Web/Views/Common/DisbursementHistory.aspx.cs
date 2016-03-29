using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DisbursementHistory : SAHLCommonBaseView, IDisbursementHistory
    {
        /// <summary>
        /// 
        /// </summary>
        public int SelectedStatus
        {
            get
            {
                if (Request.Form[FilterDropDown.UniqueID] != null && Request.Form[FilterDropDown.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[FilterDropDown.UniqueID]);
                else
                    return -1;
            }
            set
            {
                FilterDropDown.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalDisbursementsValue
        {
            set
            {
                TotalDisbursements.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyStatuses"></param>
        public void BindDisbursementStatuses(IList<IDisbursementStatus> historyStatuses)
        {
            FilterDropDown.DataSource = historyStatuses;
            FilterDropDown.DataValueField = "Key";
            FilterDropDown.DataTextField = "Description";
            FilterDropDown.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursements"></param>
        public void BindGrid(IReadOnlyEventList<IDisbursement> disbursements)
        {
            DisbursementGrid.Columns.Clear();
            DisbursementGrid.AddGridBoundColumn("ActionDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Date", false, Unit.Percentage(15), HorizontalAlign.Left, true);
            DisbursementGrid.AddGridBoundColumn("", "Bank", Unit.Percentage(50), HorizontalAlign.Left, true);
            DisbursementGrid.AddGridBoundColumn("", "Disbursement Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            DisbursementGrid.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(15), HorizontalAlign.Right, true);
            DisbursementGrid.DataSource = disbursements;
            DisbursementGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DisbursementGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IDisbursement disbursement = e.Row.DataItem as IDisbursement;

                string bankDesc = disbursement.GetBankDisplayName(BankAccountNameFormat.Full);

                e.Row.Cells[1].Text = !String.IsNullOrEmpty(bankDesc) ? bankDesc : "";

                e.Row.Cells[2].Text = disbursement.DisbursementTransactionType != null ? disbursement.DisbursementTransactionType.Description : "";
            }
        }
    }
}
