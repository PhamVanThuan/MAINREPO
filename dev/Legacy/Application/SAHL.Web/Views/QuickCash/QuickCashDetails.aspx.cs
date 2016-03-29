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
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.QuickCash
{
    public partial class QuickCashDetails : SAHLCommonBaseView,IQuickCashDetails
    {
        #region Private Members

        private bool _showPaymentPanel;
        private bool _showButtons;
        private bool _showApprovedPanel;
        private bool _showBankAccountPanel;
        private ISupportsQuickCashApplicationInformation _applicationQuickCash;
        double requestedAmtTotal;
        double InitiationFeeTotal;
        double approxInterestTotal;
        double grandTotal;
        double totalAmt;

        #endregion     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            txtNumDaysInterest.Attributes["onkeyup"] = "calculateApproximateInterest()" + txtNumDaysInterest.Attributes["onkeyup"];
            txtRequiredAmt.Attributes["onkeyup"] = "calculateApproximateInterest()" + txtRequiredAmt.Attributes["onkeyup"];
            ddlPaymentType.Attributes["onchange"] = "calculateApproximateInterest()" + ddlPaymentType.Attributes["onchange"];

            panelPayments.Visible = _showPaymentPanel;
            ButtonRow.Visible = _showButtons;
            panelApproved.Visible = _showApprovedPanel;
            panelPayments.Visible = _showBankAccountPanel;
        }

        public void SetRatesForPaymentType(IRateConfiguration rateConfig)
        {
            CalculateAvailableAmount();

            if (rateConfig != null)
            {
                BindInterestRates(rateConfig);
                double interestRate = ((rateConfig.Margin.Value * 100) + (rateConfig.MarketRate.Value * 100)) / 100;
                txtApproxInterestAmt.Text = Convert.ToDouble((txtRequiredAmt.Amount * interestRate * Convert.ToInt32(txtNumDaysInterest.Text)) / 365).ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            if (ddlPaymentType.SelectedValue == ((int)QuickCashPaymentTypes.RegularPayment).ToString())
                txtNumDaysInterest.Text = "35";
            else
                txtNumDaysInterest.Text = "60";
        }

        private void BindInterestRates(IRateConfiguration rateConfig)
        {
            IDictionary<string, string> margins = new Dictionary<string, string>();
            margins.Add(rateConfig.Margin.Key.ToString(), rateConfig.Margin.Value.ToString(SAHL.Common.Constants.RateFormat));

            ddlLinkRate.DataValueField = "Key";
            ddlLinkRate.DataTextField = "Values";
            ddlLinkRate.DataSource = margins;
            ddlLinkRate.DataBind();

            txtPrime.Text = (rateConfig.MarketRate.Value).ToString(SAHL.Common.Constants.RateFormat);
            txtInterestRate.Text = Convert.ToDouble((rateConfig.MarketRate.Value + ((rateConfig.Margin.Value * 100) / 100))).ToString(SAHL.Common.Constants.RateFormat);
        }

        public int GetSelectedThirPartyPayment
        {
            get
            {
               return Convert.ToInt32(gridThirdPartyPayments.SelectedRow.Cells[0].Text);
            }
        }

        public int BankAccountKey
        {
            get
            {
                if (ddlBankAccount.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlBankAccount.SelectedValue);
                else
                    return 0;
            }
            set
            {
                ddlBankAccount.SelectedValue = value.ToString();
            }
        }
     
        private void CalculateAvailableAmount()
        {
            double availableAmount = 0;
            double totalRequestedAmount = 0;

            if (_applicationQuickCash != null)
            {
                if (ddlPaymentType.SelectedValue == ((int)QuickCashPaymentTypes.RegularPayment).ToString())
                {
                    for (int i = 0; i < _applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
                    {
                        if (_applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].Key > 0)
                             totalRequestedAmount += Convert.ToDouble(_applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
                    }

                    availableAmount = _applicationQuickCash.ApplicationInformationQuickCash.CreditApprovedAmount - totalRequestedAmount;
                }
                else
                    if (ddlPaymentType.SelectedValue == ((int)QuickCashPaymentTypes.UpfrontPayment).ToString())
                    {
                            for (int i = 0; i < _applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
                            {
                                if (_applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].Key > 0 && _applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                                    totalRequestedAmount += Convert.ToDouble(_applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
                            }
                            availableAmount = Convert.ToDouble(_applicationQuickCash.ApplicationInformationQuickCash.CreditUpfrontApprovedAmount) - totalRequestedAmount;
                    }
            }
            if (availableAmount > 0)
                txtAvailableAmt.Amount = availableAmount;
            else
                txtAvailableAmt.Amount = 0;
        }

        public ISupportsQuickCashApplicationInformation QuickCashInformation 
        {
            set
            {
                _applicationQuickCash = value;
            }
        }

        public string SetSubmitButtonText
        {
            set
            {
                btnSubmit.Text = value;
            }
        }

        public void BindQuickCashPaymentTypes(IEventList<IQuickCashPaymentType> qcPaymentTypes)
        {
            ddlPaymentType.DataValueField = "Key";
            ddlPaymentType.DataTextField = "Description";
            ddlPaymentType.DataSource = qcPaymentTypes;
            ddlPaymentType.DataBind();
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onPaymentTypeSelectedIndexChanged != null)
            {
                onPaymentTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlPaymentType.SelectedValue));
            }
        }
    
        public void BindBankAccount(IDictionary<string, string> bankAccounts)
        {
            ddlBankAccount.DataValueField = "Key";
            ddlBankAccount.DataTextField = "Values";
            ddlBankAccount.DataSource = bankAccounts;
            ddlBankAccount.DataBind();
        }

        public void BindApprovedPanel()
        {
            IApplicationMortgageLoanWithCashOut appMLwithCashOut = _applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformation.Application as IApplicationMortgageLoanWithCashOut;
            if (appMLwithCashOut != null)
                txtCashOutRequired.Amount = appMLwithCashOut.RequestedCashAmount;
            txtUpfrontApprovedAmt.Amount = _applicationQuickCash.ApplicationInformationQuickCash.CreditUpfrontApprovedAmount;
            txtQCApprovedAmount.Amount = _applicationQuickCash.ApplicationInformationQuickCash.CreditApprovedAmount;
        }

        public void BindQuickCashPaymentsGrid(bool postBack)
        {
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Payment Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Interest Rate", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Days Interest", Unit.Percentage(5), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Date Disbursed", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Required", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Approx Interest", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Initiation fee", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridQuickCashPaymentDetails.AddGridBoundColumn("", "Total", Unit.Percentage(15), HorizontalAlign.Left, true);

            gridQuickCashPaymentDetails.DataSource = _applicationQuickCash.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails;
            
            if (postBack)
                gridQuickCashPaymentDetails.PostBackType = GridPostBackType.SingleClick;

            gridQuickCashPaymentDetails.DataBind();

        }

        public void BindThirdPartyPaymentsGrid(IEventList<IApplicationExpense> appExpenses, bool setAutoPostBack)
        {
            gridThirdPartyPayments.Columns.Clear();
            gridThirdPartyPayments.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridThirdPartyPayments.AddGridBoundColumn("", "Expense Type", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridThirdPartyPayments.AddGridBoundColumn("", "Bank Account", Unit.Percentage(35), HorizontalAlign.Left, true);
            gridThirdPartyPayments.AddGridBoundColumn("", "Expense Name", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridThirdPartyPayments.AddGridBoundColumn("", "Expense Account No", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridThirdPartyPayments.AddGridBoundColumn("", "Reference", Unit.Percentage(14), HorizontalAlign.Left, true);
            gridThirdPartyPayments.AddGridBoundColumn("", "Amount", Unit.Percentage(10), HorizontalAlign.Left, true);

            gridThirdPartyPayments.DataSource = appExpenses;
            if (setAutoPostBack)
                gridThirdPartyPayments.PostBackType = GridPostBackType.SingleClick;
            gridThirdPartyPayments.DataBind();

        }
      
        protected void gridQuickCashPaymentDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onGridQuickCashPaymentSelectedIndexChanged != null)
                onGridQuickCashPaymentSelectedIndexChanged(sender, new KeyChangedEventArgs(gridQuickCashPaymentDetails.SelectedIndex));
        }

        protected void gridThirdPartyPayments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ongridThirdPartyPaymentSelectedIndexChanged != null)
                ongridThirdPartyPaymentSelectedIndexChanged(sender, new KeyChangedEventArgs(gridThirdPartyPayments.SelectedIndex));
        }

        public void BindBankAccountPanel(IApplicationInformationQuickCashDetail appInformationQCDetail)
        {
            ddlPaymentType.SelectedValue = appInformationQCDetail.QuickCashPaymentType.Key.ToString();
            txtInterestRate.Text = Convert.ToDouble(appInformationQCDetail.InterestRate).ToString(SAHL.Common.Constants.RateFormat);
            txtRequiredAmt.Amount = appInformationQCDetail.RequestedAmount;
            txtApproxInterestAmt.Text = Convert.ToDouble((appInformationQCDetail.RequestedAmount * appInformationQCDetail.InterestRate * Convert.ToInt32(txtNumDaysInterest.Text)) / 365).ToString(SAHL.Common.Constants.CurrencyFormat);

        }
        
        public int GetSetSelectedGridItem
        {
            get
            {
                return Convert.ToInt32(gridQuickCashPaymentDetails.SelectedRow.Cells[0].Text);
            }
            set
            {
                gridQuickCashPaymentDetails.SelectedIndex = value;
            }
        }

        protected void gridThirdPartyPayments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IApplicationExpense appExpense = e.Row.DataItem as IApplicationExpense;

            if (e.Row.DataItem != null)
            {
                if (appExpense.ExpenseType.PaymentType.Key == (int)PaymentTypes.QuickCashPayment)
                    e.Row.Font.Bold = true;
                cells[0].Text = appExpense.Key.ToString();
                cells[1].Text = appExpense.ExpenseType.Description;
                if (appExpense.ApplicationDebtSettlements != null && appExpense.ApplicationDebtSettlements.Count > 0)
                    cells[2].Text = appExpense.ApplicationDebtSettlements[0].BankAccount.AccountName + " " +
                    appExpense.ApplicationDebtSettlements[0].BankAccount.ACBBranch.ACBBank.ACBBankDescription + " " +
                    appExpense.ApplicationDebtSettlements[0].BankAccount.ACBBranch.ACBBranchDescription + appExpense.ApplicationDebtSettlements[0].BankAccount.AccountNumber;
                else
                    cells[2].Text = "";
                cells[3].Text = appExpense.ExpenseAccountName;
                cells[4].Text = appExpense.ExpenseAccountNumber;
                cells[5].Text = appExpense.ExpenseReference;
                cells[6].Text = appExpense.TotalOutstandingAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                totalAmt += Convert.ToDouble(appExpense.TotalOutstandingAmount);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                cells[0].Text = "Total";
                cells[6].Text = totalAmt.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        protected void gridQuickCashPaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IApplicationInformationQuickCashDetail QCDetail = e.Row.DataItem as IApplicationInformationQuickCashDetail;         
           
            if (e.Row.DataItem != null)
            {
                IApplication _app = QCDetail.OfferInformationQuickCash.ApplicationInformation.Application;

                cells[0].Text = QCDetail.Key.ToString();
                cells[1].Text = QCDetail.QuickCashPaymentType == null ? " " : QCDetail.QuickCashPaymentType.Description;
                cells[2].Text = QCDetail.InterestRate == null ? " " : Convert.ToDouble(QCDetail.InterestRate).ToString(SAHL.Common.Constants.RateFormat);

                int numDaysInterest = 0;

                if (QCDetail.QuickCashPaymentType != null && QCDetail.QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.RegularPayment)
                    numDaysInterest = 35;
                else
                    if (QCDetail.QuickCashPaymentType != null && QCDetail.QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                        numDaysInterest = 60;

                cells[3].Text = numDaysInterest.ToString();

                if (Convert.ToBoolean(QCDetail.Disbursed))
                {
                    if (QCDetail.ApplicationExpenses[0].ApplicationDebtSettlements[0].Disbursement != null)
                        cells[4].Text = Convert.ToDateTime(QCDetail.ApplicationExpenses[0].ApplicationDebtSettlements[0].Disbursement.ActionDate).ToString(SAHL.Common.Constants.DateFormat);
                }
                else
                        cells[4].Text = "-";

                double inititiationFee = 0;

                if (_app.ApplicationExpenses != null && _app.ApplicationExpenses.Count > 0)
                {
                    for (int i = 0; i < _app.ApplicationExpenses.Count; i++)
                    {
                        if (_app.ApplicationExpenses[i].ExpenseType != null && _app.ApplicationExpenses[i].ExpenseType.Key == (int)ExpenseTypes.InitiationFeeQuickCashProcessingFee)
                        {
                            inititiationFee += _app.ApplicationExpenses[i].TotalOutstandingAmount;
                        }
                    }
                }

                cells[5].Text = Convert.ToDouble(QCDetail.RequestedAmount).ToString(SAHL.Common.Constants.CurrencyFormat);
                double approximateInterest = Convert.ToDouble((QCDetail.RequestedAmount * numDaysInterest * QCDetail.InterestRate) / 365);
                cells[6].Text = approximateInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
                cells[7].Text = inititiationFee.ToString(SAHL.Common.Constants.CurrencyFormat);
                double totalPerPaymentType = Convert.ToDouble(QCDetail.RequestedAmount) + approximateInterest + inititiationFee;
                cells[8].Text = totalPerPaymentType.ToString(SAHL.Common.Constants.CurrencyFormat);

                requestedAmtTotal += Convert.ToDouble(QCDetail.RequestedAmount);
                approxInterestTotal += approximateInterest;
                InitiationFeeTotal += inititiationFee;
                grandTotal += totalPerPaymentType;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                gridQuickCashPaymentDetails.FooterStyle.Wrap = true;
                cells[5].Text = requestedAmtTotal.ToString(SAHL.Common.Constants.CurrencyFormat);
                cells[6].Text = approxInterestTotal.ToString(SAHL.Common.Constants.CurrencyFormat);
                cells[7].Text = InitiationFeeTotal.ToString(SAHL.Common.Constants.CurrencyFormat);
                cells[8].Text = grandTotal.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        public IApplicationInformationQuickCashDetail GetUpdatedQuickDetailRecord(IApplicationInformationQuickCashDetail qcDetail)
        {
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            if (ddlPaymentType.SelectedValue != "-select-")
                qcDetail.QuickCashPaymentType = lookups.QuickCashPaymentTypes.ObjectDictionary[ddlPaymentType.SelectedValue];
            qcDetail.RequestedAmount = Convert.ToDouble(txtRequiredAmt.Amount);
            qcDetail.Disbursed = false;
          
            return qcDetail;
        }

        #region IQuickCashDetails Members
        /// <summary>
        /// inherits <see cref="IQuickCashDetails.ShowPaymentsPanel"/>
        /// </summary>
        public bool ShowPaymentsPanel
        {
            set { _showPaymentPanel = value; }
        }

        /// <summary>
        /// inherits <see cref="IQuickCashDetails.ShowButtons"/>
        /// </summary>
        public bool ShowButtons
        {
            set { _showButtons = value; }
        }

        public bool ShowUpdateButton
        {
            set { btnSubmit.Visible = value; }
        }

        /// <summary>
        /// inherits <see cref="IQuickCashDetails.ShowApprovedPanel"/>
        /// </summary>
        public bool ShowApprovedPanel
        {
            set { _showApprovedPanel = value; }
        }

        /// <summary>
        /// inherits <see cref="IQuickCashDetails.ShowBankAccountPanel"/>
        /// </summary>
        public bool ShowBankAccountPanel
        {
            set { _showBankAccountPanel = value; }
        }

        public bool ShowThirdPartyPaymentsGrid
        {
            set { gridThirdPartyPayments.Visible = value; }
        }

        public string SetThirdParyPaymentsGridHeaderText
        { 
            set 
            {
                gridThirdPartyPayments.HeaderCaption = value;
                gridThirdPartyPayments.EmptyDataSetMessage = "There are no " + value;
            }
        }

        #endregion

        #region
        /// <summary>
        /// inherits <see cref="IQuickCashDetails.OnCancelButtonClicked"/>
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// inherits <see cref="IQuickCashDetails.OnSubmitButtonClicked"/>
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        public event KeyChangedEventHandler onGridQuickCashPaymentSelectedIndexChanged;

        public event KeyChangedEventHandler onPaymentTypeSelectedIndexChanged;

        public event KeyChangedEventHandler ongridThirdPartyPaymentSelectedIndexChanged;

        #endregion

    }
}
