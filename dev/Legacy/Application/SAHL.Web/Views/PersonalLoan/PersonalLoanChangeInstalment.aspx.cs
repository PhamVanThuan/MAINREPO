using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class PersonalLoanChangeInstalment : SAHLCommonBaseView, IPersonalLoanChangeInstalment
    {
        private IEventList<IFinancialService> _lstFinancialServices;
        private IFinancialService _financialService;
        private IAccountPersonalLoan _accountPersonalLoan;
        private double _newInstalment;
        private double _creditLifePremium;
        private double _monthlyServiceFee;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage)
                return;

            btnSubmit.Attributes["onclick"] = "if(!confirm('Are you sure you would like to Process Instalment Change for this Account ?')) return false";
        }

        /// <summary>
        /// OnViewPreRender Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            bool instalmentsVisible = _newInstalment == 0 ? false : true;

            if (instalmentsVisible)
            {
                lblNewInstalment.Text = _newInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblCreditLifePremium.Text = _creditLifePremium.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblMonthlyServiceFee.Text = _monthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotalInstalment.Text = (_newInstalment + _creditLifePremium + _monthlyServiceFee).ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            trCreditLifePremium.Visible = instalmentsVisible;
            trMonthlyServiceFee.Visible = instalmentsVisible;
            trTotalInstalment.Visible = instalmentsVisible;
        }

        /// <summary>
        /// Enable/Disable Calculate Button
        /// </summary>
        public bool EnableCalculateButton
        {
            set
            {
                btnCalculate.Enabled = value;
            }
        }

        /// <summary>
        /// Enable/Disable Submit Button
        /// </summary>
        public bool EnableSubmitButton
        {
            set
            {
                btnSubmit.Enabled = value;
            }
        }

        public string MemoComments
        {
            get
            {
                return txtComments.Text;
            }
        }

        /// <summary>
        /// New calculated Instalment
        /// </summary>
        public double NewInstalment
        {
            set
            {
                _newInstalment = value;
            }
            get
            {
                return _newInstalment;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double CreditLifePremium
        {
            get
            {
                return _creditLifePremium;
            }
            set
            {
                _creditLifePremium = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double MonthlyServiceFee
        {
            get
            {
                return _monthlyServiceFee;
            }
            set
            {
                _monthlyServiceFee = value;
            }
        }
        /// <summary>
        /// Bind Grid
        /// </summary>
        /// <param name="lstFinancialServices"></param>
        public void BindPersonalLoansGrid(IEventList<IFinancialService> lstFinancialServices)
        {
            _lstFinancialServices = lstFinancialServices;
            _financialService = lstFinancialServices[0];

            _accountPersonalLoan = _financialService.Account as IAccountPersonalLoan;

            PersonalLoansGrid.GridHeight = Unit.Pixel(200);
            PersonalLoansGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, false);
            PersonalLoansGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            PersonalLoansGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Arrears Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            PersonalLoansGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Interest Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            PersonalLoansGrid.AddGridBoundColumn("", "Initial Term", Unit.Percentage(12), HorizontalAlign.Center, true);
            PersonalLoansGrid.AddGridBoundColumn("", "Total Term", Unit.Percentage(12), HorizontalAlign.Center, true);
            PersonalLoansGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Center, true);
            PersonalLoansGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Loan Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            PersonalLoansGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Total Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);

            PersonalLoansGrid.DataSource = lstFinancialServices;
            PersonalLoansGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PersonalLoans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.DataItem != null)
            {
                IFinancialService financialService = e.Row.DataItem as IFinancialService;

                if (financialService != null)
                {
                    cells[0].Text = financialService.FinancialServiceType.Description;

                    // current balance                    
                    cells[1].Text = financialService.Balance.Amount.ToString();

                    // arrears balance
                    var arrearBalanceFinancialService =
                        _accountPersonalLoan.GetFinancialServicesByType(FinancialServiceTypes.ArrearBalance,
                                                                new AccountStatuses[] 
                                                                    { 
                                                                        AccountStatuses.Open, 
                                                                        AccountStatuses.Closed 
                                                                    }).OrderByDescending(x => x.Key).FirstOrDefault();

                    cells[2].Text = arrearBalanceFinancialService == null ? "0" : arrearBalanceFinancialService.Balance.Amount.ToString();

                    // interest rate
                    cells[3].Text = financialService.Balance.LoanBalance.InterestRate.ToString();

                    // get the initial term
                    var productinfo = financialService.Account.GetLatestApplicationByType(OfferTypes.UnsecuredLending).CurrentProduct as IApplicationProductPersonalLoan;
                    cells[4].Text = productinfo.ApplicationInformationPersonalLoan.Term.ToString() + " Months";
                    // total term
                    cells[5].Text = financialService.Balance.LoanBalance.Term.ToString() + " Months";
                    // remaining term
                    cells[6].Text = financialService.Balance.LoanBalance.RemainingInstalments.ToString() + " Months";

                    // personal loan instalment
                    cells[7].Text = financialService.Payment.ToString();

                    // get the total instalment - personal loan + sahl credit life + monthly service fee
                    double totalInstalment = financialService.Payment; // personal loan
                    totalInstalment += financialService.Account.InstallmentSummary.MonthlyServiceFee; // monthly service fee
                    var creditProtectionPlanFinancialService = financialService.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan).FirstOrDefault();
                    if (creditProtectionPlanFinancialService != null)
                        totalInstalment += creditProtectionPlanFinancialService.Payment; // sahl credit life

                    cells[8].Text = totalInstalment.ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CalculateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Handles Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles Calculate button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (CalculateButtonClicked != null)
                CalculateButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles click event of submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SubmitButtonClicked != null)
                SubmitButtonClicked(sender, e);
        }
    }
}