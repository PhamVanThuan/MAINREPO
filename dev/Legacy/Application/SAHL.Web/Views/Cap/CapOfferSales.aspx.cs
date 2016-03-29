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
    public partial class CapOfferSales : SAHLCommonBaseView, ICapOfferSales
    {
        #region Variables

        double _variableInstallment;
        double _linkRate;
        double _HouseholdIncome;
        double _LatestValuation;
        double _accruedInterestMTD;
        string _confirmMessageText;

        private string _applicationType1;
        private string _applicationType2;
        private string _applicationType3;

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

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public double AccruedInterestMTD
        {
            set 
            { 
                _accruedInterestMTD = value;
                lblAccruedInterestMTD.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double CommittedLoanValue
        {
            set
            {
                lblCommittedLoanValue.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double VariableInstallment
        {
            set { _variableInstallment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LatestValuation
        {
            set 
            { 
                _LatestValuation = value;
                lblLatestValuation.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double HouseholdIncome
        {
            set { _HouseholdIncome = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LoanAgreementAmount
        {
            set
            {
                lblLoanAgreementAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LinkRate
        {
            set
            {
                _linkRate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReasonDropDownVisible
        {
            set
            {
                lblNTUReason.Visible = !value;
                ddlNTUReason.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PaymentOptionDropDownVisible
        {
            set
            {
                lblPaymentOption.Visible = !value;
                ddlPaymentOption.Visible = value;
            }
            get
            {
                return ddlPaymentOption.Visible;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NTUReasonLabelText
        {
            set
            {
                lblNTUReason.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PaymentOptionText
        {
            set
            {
                lblPaymentOption.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool OfferStatusListVisible
        {
            set
            {
                ddlOfferStatusList.Visible = value;
                ddlOfferStatusList.Visible = !value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool BalanceRowVisible
        {
            set
            {
                BalanceRow.Visible = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool PromotionCheckBoxEnabled
        {
            set
            {
                PromotionClient.Enabled = value;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public bool PromotionCheckBoxChecked
        {
            set
            {
                PromotionClient.Checked = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LegalEntityNameText
        {
            set
            {
                lblLegalEntityName.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccountNumberText
        {
            set
            {
                lblAccountNumber.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SalesConsultantText
        {
            set
            {
                lblSaleConsultant.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NextResetDateText
        {
            set
            {
                lblNextResetDate.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CapEffectiveDateText
        {
            set
            {
                lblCapEffectiveDate.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OfferStartDateText
        {
            set
            {
                lblOfferStartDate.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OfferEndDateText
        {
            set
            {
                lblOfferEndDate.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalBondAmount
        {
            set
            {
                lblTotalBondAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OfferStatusText
        {
            set
            {
                lblOfferStatus.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BalanceToCap
        {
            set
            {
                lblBalanceToCap.Text = value;
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
        public string SubmitButtonText
        {
            set
            {
                SubmitButton.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set
            {
                SubmitButton.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CancelButtonVisible
        {
            set
            {
                CancelButton.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType CapGridPostBackType
        {
            set
            {
                CapOfferGrid.PostBackType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConfirmMessageText
        {
            set
            {
                _confirmMessageText = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GridSelectedIndex
        {
            get
            {
                return CapOfferGrid.SelectedIndexInternal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CapReasonSelectedValue
        {
            get
            {
                if (Request.Form[ddlNTUReason.UniqueID] != null && Request.Form[ddlNTUReason.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlNTUReason.UniqueID]);
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CapPaymentOptionSelectedValue
        {
            get
            {
                if (Request.Form[ddlPaymentOption.UniqueID] != null && Request.Form[ddlPaymentOption.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlPaymentOption.UniqueID]);
                else
                    return -1;
            }
            set
            {
                ddlPaymentOption.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CapPageVisible
        {
            set
            {
                CapPage.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CapReportVisible
        {
            set
            {
                CapReport.Visible = value;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_confirmMessageText))
            {
                ClientScript.RegisterClientScriptInclude("SAHLScripts", Page.ResolveClientUrl("~/Scripts/SAHLScripts.js"));
                SubmitButton.Attributes["onclick"] = "if(!confirmMessage('"+_confirmMessageText+"')) return false";
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CapOfferGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ICapApplicationDetail ro = e.Row.DataItem as ICapApplicationDetail;
                if (ro.CapTypeConfigurationDetail != null)
                {
                    // CAP Type
                    e.Row.Cells[1].Text = ro.CapTypeConfigurationDetail.CapType.Description;
                    
                    // Cap2 Base Rate
                    e.Row.Cells[2].Text = ro.CapTypeConfigurationDetail.Rate.ToString();

                    // Link Rate
                    e.Row.Cells[3].Text = _linkRate.ToString();
                    
                    // Current Bal incl Fee
                    e.Row.Cells[5].Text = (ro.Fee + ro.CapApplication.CurrentBalance).ToString();

                    // Type of Advance Required
                    if (ro.CapTypeConfigurationDetail.CapType.Key == 1)
                        e.Row.Cells[6].Text = _applicationType1;
                    else if (ro.CapTypeConfigurationDetail.CapType.Key == 2)
                        e.Row.Cells[6].Text = _applicationType2;
                    else if (ro.CapTypeConfigurationDetail.CapType.Key == 3)
                        e.Row.Cells[6].Text = _applicationType3;

                    // Increase in Instalment
                    e.Row.Cells[7].Text = (ro.Payment - _variableInstallment).ToString(); 
                }
                else
                {
                    e.Row.Cells[1].Text = "-";
                }

                // LTV
                if (_LatestValuation > 0)
                    e.Row.Cells[9].Text = Convert.ToString((ro.Fee + ro.CapApplication.CurrentBalance + _accruedInterestMTD) / _LatestValuation);
                else
                    e.Row.Cells[9].Text = "0.00";

                // PTI
                if (_HouseholdIncome > 0)
                    e.Row.Cells[10].Text = Convert.ToString(Convert.ToDouble(e.Row.Cells[8].Text) / _HouseholdIncome);
                else
                    e.Row.Cells[10].Text = "0.00";

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

        #region ICapOfferSales Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capDetailList"></param>
        public void BindCapGrid(IList<ICapApplicationDetail> capDetailList)
        {
            if (capDetailList!=null && capDetailList.Count >0)
            {
                int accountKey = capDetailList[0].CapApplication.Account.Key;
                int capofferKey = capDetailList[0].CapApplication.Key;
                int captypeconfigKey = capDetailList[0].CapApplication.CapTypeConfiguration.Key;
                ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();

                double clv = 0;
                capRepo.CapTypeDetermineAppType(accountKey, captypeconfigKey, capofferKey, out clv, out _applicationType1, out _applicationType2, out _applicationType3);
            }

            CapOfferGrid.Columns.Clear();

            CapOfferGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);

            CapOfferGrid.AddGridBoundColumn("", "Offer", Unit.Percentage(15), HorizontalAlign.Left, true);
            CapOfferGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Cap2 Base Rate", false, Unit.Percentage(7.5), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(7.5), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("Fee", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Advance Amount", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance incl. Fee", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("", "Type of Advance Req.", Unit.Percentage(15), HorizontalAlign.Left, true);
            CapOfferGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Increase in Instalment", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("Payment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment incl. CAP Premium", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "LTV", false, Unit.Percentage(7.5), HorizontalAlign.Center, true);
            CapOfferGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "PTI", false, Unit.Percentage(7.5), HorizontalAlign.Center, true);
            CapOfferGrid.DataSource = capDetailList;
            CapOfferGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="installmentData"></param>
        /// <param name="installmentHeaderValue"></param>
        public void BindInstalmentGrid(DataTable installmentData , string installmentHeaderValue)
        {
            InstallmentGrid.Columns.Clear();
            InstallmentGrid.AddGridBoundColumn("ServiceType", "Service Type", Unit.Percentage(15), HorizontalAlign.Left, true);
            InstallmentGrid.AddGridBoundColumn("LoanInstallment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, installmentHeaderValue, false, Unit.Percentage(20), HorizontalAlign.Right, true);
            InstallmentGrid.AddGridBoundColumn("Premiums", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Premiums", false, Unit.Percentage(20), HorizontalAlign.Right, true);
            InstallmentGrid.AddGridBoundColumn("ShortInstallments", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Short Term Instalment", false, Unit.Percentage(20), HorizontalAlign.Right, true);
            InstallmentGrid.AddGridBoundColumn("TotalInstallments", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Total Instalment", false, Unit.Percentage(25), HorizontalAlign.Right, true);
            InstallmentGrid.DataSource = installmentData;
            InstallmentGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capReasons"></param>
        public void BindReasonDropdown(IList<ICapNTUReason> capReasons)
        {
            ddlNTUReason.DataSource = capReasons;
            ddlNTUReason.DataTextField = "Description";
            ddlNTUReason.DataValueField = "Key";
            ddlNTUReason.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capPaymentOptions"></param>
        public void BindPaymentOptionDropDown(IList<ICapPaymentOption> capPaymentOptions)
        {
            ddlPaymentOption.DataSource = capPaymentOptions;
            ddlPaymentOption.DataTextField = "Description";
            ddlPaymentOption.DataValueField = "Key";
            ddlPaymentOption.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountKey"></param>
        public void LoadCapLetter(int AccountKey)
        {
            CapPageVisible = false;
            CapReportVisible = true;

            StringBuilder requestString = new StringBuilder();
            // Report
            requestString.Append("?ReportPath=");
            requestString.Append("/SAHL/Serv.CAP.CAP Letter");
            // Parameters
            requestString.Append("&AccountKey=");
            requestString.Append(AccountKey.ToString());
            requestString.Append("&Format=1");

            ReportViewerFrame.Attributes["src"] = "../Reports/ReportingServices.aspx" + requestString.ToString();
            ReportViewerFrame.Visible = true;


        }

        #endregion
     }
}
