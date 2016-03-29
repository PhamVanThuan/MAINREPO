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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Web.Views.Cap.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Cap
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CAPAcceptedHistory : SAHLCommonBaseView, ICAPAcceptedHistory
    {
        #region Private Variables

		IDictionary<int, IFinancialAdjustment> _financialAdjustmentDict = new Dictionary<int, IFinancialAdjustment>();

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool CancelCapRowVisible
        {
            set
            {
                CancelCapRow.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ButtonRowVisible
        {
            set
            {
                ButtonRow.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedCancellationReason
        {
            get
            {
                if (Request.Form[ddlCancellationReason.UniqueID] != null && Request.Form[ddlCancellationReason.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlCancellationReason.UniqueID]);
                else
                    return -1;
            }
        }


        public IDictionary<int, IFinancialAdjustment> FinancialAdjustmentDict
        {
            set { _financialAdjustmentDict = value; }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        #endregion


        #region Protected Members

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HistoryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ICapApplicationDetail ro = e.Row.DataItem as ICapApplicationDetail;
                if (ro.CapTypeConfigurationDetail != null)
                {
                    IMortgageLoanAccount mortgageLoanAcc = ro.CapApplication.Account as IMortgageLoanAccount;
					IFinancialAdjustment financialAdjustment = _financialAdjustmentDict[ro.Key];
                    
                    e.Row.Cells[1].Text = ro.CapTypeConfigurationDetail.CapType.Description;
                    e.Row.Cells[2].Text = ro.CapApplication.CapTypeConfiguration.CapEffectiveDate.ToString();
                    e.Row.Cells[3].Text = ro.CapApplication.CapTypeConfiguration.CapClosureDate.ToString();
                    e.Row.Cells[8].Text = ro.CapApplication.Broker.FullName;
                    e.Row.Cells[4].Text = ro.CapTypeConfigurationDetail.Rate.ToString();
                    e.Row.Cells[7].Text = ro.CapApplication.CurrentBalance.ToString();  
                    e.Row.Cells[10].Text = financialAdjustment.FinancialAdjustmentStatus.Description;

                    if (ro.CapApplication.CAPPaymentOption != null)
                        e.Row.Cells[11].Text = ro.CapApplication.CAPPaymentOption.Description;
                    else
                        e.Row.Cells[11].Text = "-";

                    if (mortgageLoanAcc != null)
                    {
                        double linkrate = mortgageLoanAcc.SecuredMortgageLoan.RateConfiguration.Margin.Value;
						linkrate += mortgageLoanAcc.SecuredMortgageLoan.RateAdjustment;
                        e.Row.Cells[5].Text = linkrate.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        #endregion


        #region ICAPAcceptedHistory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capofferDetailList"></param>
        public void BindGrid(IList<ICapApplicationDetail> capofferDetailList)
        {
            HistoryGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            HistoryGrid.AddGridBoundColumn("", "CAP Offer", Unit.Percentage(10), HorizontalAlign.Left, true);
            HistoryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            HistoryGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Closure Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            HistoryGrid.AddGridBoundColumn("EffectiveRate", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "CAP Base Rate", false, Unit.Percentage(7), HorizontalAlign.Left, true);
            HistoryGrid.AddGridBoundColumn("EffectiveRate", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(7), HorizontalAlign.Left, true);
            HistoryGrid.AddGridBoundColumn("Fee", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Once off CAP Premium", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HistoryGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Capped Balance incl. Fee", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            HistoryGrid.AddGridBoundColumn("", "CAP Consultant", Unit.Percentage(13), HorizontalAlign.Left, true);
            HistoryGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            HistoryGrid.AddGridBoundColumn("", "Status", Unit.Percentage(8), HorizontalAlign.Left, true);
            HistoryGrid.AddGridBoundColumn("", "Payment", Unit.Percentage(10), HorizontalAlign.Left, true);
            HistoryGrid.DataSource = capofferDetailList;
            HistoryGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capReasons"></param>
        public void BindReasons(IList<ICancellationReason> capReasons)
        {
            ddlCancellationReason.DataSource = capReasons;
            ddlCancellationReason.DataTextField = "Description";
            ddlCancellationReason.DataValueField = "Key";
            ddlCancellationReason.DataBind();
        }

        #endregion
    }
}
