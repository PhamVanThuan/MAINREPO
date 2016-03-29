using System;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class Views_RateChange : SAHLCommonBaseView, IRateChange
    {
        /// <summary>
        /// Create copy of Mortgage Loan List
        /// </summary>
        private IEventList<IMortgageLoan> mlLst;
        /// <summary>
        /// Makes copy of Margin list
        /// </summary>
        private IEventList<IMargin> marginLst;
        
        private double CapBalance;
        private const int interestPeriods = 12;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Still working on code")]
        private string memoComments;
        private int maximumTerm;

        /// <summary>
        /// Gets or sets the Maxumim Rate Term in months
        /// </summary>
        public int MaximumTerm
        {
            get { return maximumTerm; }
            set { maximumTerm = value; }
        }

        private double? _currentPTI;
        private double? _newPTI;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage)
                return;
        }

        /// <summary>
        /// OnViewPreRender Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;
        }

        /// <summary>
        /// sets visibility of rate controls
        /// </summary>
        public bool SetRatesControlVisibility
        {
            set
            {
                RateTable1.Visible = value;
                RateTable2.Visible = value;
            }
        }

        /// <summary>
        /// sets visibility controls for interest only - term change
        /// </summary>
        public bool SetTermControlVisibilityForInterestOnly
        {
            set
            {
                AmortisatisionRow1.Visible = value;
                AmortisatisionRow2.Visible = value;
            }
        }

        /// <summary>
        /// sets visibility of controls for Varifix - term change
        /// </summary>
        public bool SetTermControlVisibilityForVarifix
        {
            set
            {
                TermFixed.Visible = value;
            }
        }

        public bool SetTermControlVisibilityForTermComment
        {
            set
            {
                TermCommentsPanel.Visible = value;
            }
        }

        /// <summary>
        /// sets visibility of controls for Varifix - rate change
        /// </summary>
        public bool SetRateControlVisibilityForVarifix
        {
            set
            {
                RateFixed.Visible = value;
            }
        }

        /// <summary>
        /// sets visibility of controls - rate change : Interest Only
        /// </summary>
        public bool SetRateAmortisationControlVisibility
        {
            set
            {
                RateAmortisationRow1.Visible = value;
                RateAmortisationRow2.Visible = value;
            }
        }

        /// <summary>
        /// Sets visibility of term controls
        /// </summary>
        public bool SetTermControlsVisibility
        {
            set
            {
                TermTable1.Visible = value;
                TermTable2.Visible = value;
            }
        }

        /// <summary>
        /// sets visibility controls for PTI - term change
        /// </summary>
        public bool SetPTIVisibility
        {
            set
            {
                pnlPTI.Visible = value;
            }
        }

        /// <summary>
        /// Enable/Disable Submit Button
        /// </summary>
        public bool SetAbilityofSubmitButton
        {
            set
            {
                SubmitButton.Enabled = value;
            }
        }

        public bool SetAbilityofCalculateButton
        {
            set
            {
                CalcNewTerm.Visible = value;
            }
        }

        /// <summary>
        /// Bind Grid - Term - Interest Only Loans
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        public void BindGridTermInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans)
        {
            mlLst = lstMortgageLoans;
            RatesGrid.GridHeight = Unit.Pixel(200);
            RatesGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Discount", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amortising Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "New Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false);
            RatesGrid.DataSource = lstMortgageLoans;
            RatesGrid.DataBind();
        }

        /// <summary>
        /// Bind Grid - Term - Non Interest Only Loans
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        public void BindGridTermNotInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans)
        {
            mlLst = lstMortgageLoans;
            RatesGrid.GridHeight = Unit.Pixel(200);
            RatesGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Discount", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amortising Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "New Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false);
            RatesGrid.DataSource = lstMortgageLoans;
            RatesGrid.DataBind();
        }

        /// <summary>
        /// Bind Grid - Installment - Non Interest Only Loans
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        public void BindGridInstallmentNotInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans)
        {
            mlLst = lstMortgageLoans;
            RatesGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Discount", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amortising Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "New Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.DataSource = lstMortgageLoans;
            RatesGrid.DataBind();
        }

        /// <summary>
        /// Bind Grid - Installment - Interest Only Loans
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        public void BindGridInstallmentInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans)
        {
            mlLst = lstMortgageLoans;
            RatesGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Discount", false, Unit.Percentage(10), HorizontalAlign.Center, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amortising Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "New Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.DataSource = lstMortgageLoans;
            RatesGrid.DataBind();
        }

        /// <summary>
        /// Bind Grid - Rate Change - Interest Only Loans
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        public void BindGridRateChangeInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans)
        {
            RatesGrid.GridHeight = Unit.Pixel(200);
            mlLst = lstMortgageLoans;
            RatesGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Discount", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            RatesGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Left, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amortising Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "New Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false);
            RatesGrid.DataSource = lstMortgageLoans;
            RatesGrid.DataBind();
        }

        /// <summary>
        /// Bind Grid - RateChange - Non Interest Only Loans
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        public void BindGridRateChangeNotInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans)
        {
            RatesGrid.GridHeight = Unit.Pixel(200);
            mlLst = lstMortgageLoans;
            RatesGrid.AddGridBoundColumn("", "Service", Unit.Percentage(15), HorizontalAlign.Left, true); // 0 - Service
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(12), HorizontalAlign.Right, true); // 1 - Curr Bal
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Link Rate", false, Unit.Percentage(10), HorizontalAlign.Center, true); // 2 - Link Rate
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Discount", false, Unit.Percentage(10), HorizontalAlign.Center, true); // 3 - Discount
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(8), HorizontalAlign.Center, true); //4 - Rate
            RatesGrid.AddGridBoundColumn("", "Remaining Term", Unit.Percentage(12), HorizontalAlign.Left, true); // 5 - Rem Term
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amortising Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false); // 6 - Amort Inst
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, true); // 7 - Curr Ins
            RatesGrid.AddGridBoundColumn("", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "New Instalment", false, Unit.Percentage(12), HorizontalAlign.Right, false); // 8 - New Inst
            RatesGrid.DataSource = lstMortgageLoans;
            RatesGrid.DataBind();
        }

        /// <summary>
        /// RowDataBound Event for RatesGrid. Binds all possible colums, the different Grid Bind methods shows
        /// whats visible and whats hidden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RatesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IMortgageLoan ml = e.Row.DataItem as IMortgageLoan;
            if (e.Row.DataItem != null)
            {
                if (ml != null)
                {
                    if(ml.CAP != null)
                        CapBalance = Convert.ToDouble(ml.CAP.CAPBalance);

                    cells[0].Text = ml.FinancialServiceType.Description;
                    cells[1].Text = ml.CurrentBalance.ToString();
                    cells[2].Text = ml.RateConfiguration.Margin.Value.ToString();
                    cells[3].Text = ml.RateAdjustment.ToString();
                    cells[4].Text = ml.InterestRate.ToString();
                    cells[5].Text = ml.RemainingInstallments.ToString() + " Months";

                    //If the Current Balance > 0 And the Remaining Installments > 0 -- Do the Calculation
                    if (Convert.ToDouble(ml.CurrentBalance) > 0.0 && ml.RemainingInstallments > 0)
                    {
                        cells[6].Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(ml, Convert.ToDouble(ml.InterestRate), Convert.ToInt32(ml.RemainingInstallments), false).ToString();
                    }
                    else
                    {
                        cells[6].Text = 0.ToString();
                        //cells[6].Text = 0.ToString(SAHL.Common.Constants.CurrencyFormat);
                    }
                    cells[7].Text = ml.Payment.ToString();

                    // try and use the ActiveMarketRate, but for RCS loans this can be null, so in that case
                    // just use the base rate
                    double marketRate = ml.ActiveMarketRate;
                    //if (marketRate <= 0D)
                    //    marketRate = ml.ActiveMarketRate;

                    if (ml.HasInterestOnly())
                        cells[8].Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInterestOnlyInstallment(ml, CapBalance, ml.InterestRate, marketRate, ml.ActiveMarketRate).ToString();
                    else
                    {
                        cells[8].Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(ml.InterestRate, marketRate, ml.ActiveMarketRate, ml, CapBalance, Convert.ToInt32(ml.RemainingInstallments), interestPeriods).ToString();
                        //if (ml.FinancialServiceType.Key == 1) // Variable
                        //    cells[8].Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(ml.InterestRate, marketRate, ml.ActiveMarketRate, ml as IMortgageLoanAccount, CapBalance, Convert.ToInt32(ml.RemainingInstallments), interestPeriods).ToString();
                        //else
                        //    cells[8].Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(ml.InterestRate, marketRate, ml.ActiveMarketRate, ml.CurrentBalance, CapBalance, ml.RemainingInstallments, interestPeriods).ToString();
                    }
                }
                cells[8].Style["color"] = "#4040BB";
                cells[8].Style["font-weight"] = "bold";
            }
        }

        /// <summary>
        /// Populate link rates drop down
        /// </summary>
        /// <param name="margin"></param>
        public void PopulateLinkRates(IEventList<IMargin> margin)
        {
            marginLst = margin;

            NewLinkRate.DataSource = margin;
            NewLinkRate.DataValueField = "Key";
            NewLinkRate.DataTextField = "Description";
            NewLinkRate.PleaseSelectItem = (margin.Count > 1);
            NewLinkRate.DataBind();
        }

        /// <summary>
        /// Sets identification text Submit Button
        /// </summary>
        /// <param name="action"></param>
        /// <param name="accesskey"></param>
        public void SetSubmitButtonText(string action, string accesskey)
        {
            SubmitButton.Text = action;
            SubmitButton.AccessKey = accesskey;
            SubmitButton.Attributes["onclick"] = "if(!confirm('Are you sure you would like to " + action + " for this Account ?')) return false";
        }

        /// <summary>
        /// Returns logged on user
        /// </summary>
        public string GetLoggedOnUser
        {
            get { return CurrentPrincipal.Identity.Name; }
        }

        public string MemoComments
        {
            get { return TermMemoComments.Text; }
        }

        /// <summary>
        /// Get Term Captured on Screen
        /// </summary>
        public int CapturedTerm
        {
            set { NewRemainingTerm.Text = value.ToString(); }
            get
            {
                if (NewRemainingTerm.Text.Length > 0)
                    return int.Parse(NewRemainingTerm.Text);
                return 0;
            }
        }

        /// <summary>
        /// Change event of LinkRate drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NewLinkRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NewLinkRate.SelectedValue != "-select-")
            {
                double interestRate;
                double installment;

                for (int j = 0; j < mlLst.Count; j++)
                {
                    interestRate = Convert.ToDouble(marginLst[NewLinkRate.SelectedIndex - 1].Value) + Convert.ToDouble(mlLst[j].ActiveMarketRate) + Convert.ToDouble(mlLst[j].RateAdjustment);
                    if (mlLst[j].HasInterestOnly())
                        installment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInterestOnlyInstallment(mlLst[j], CapBalance, interestRate, Convert.ToDouble(mlLst[j].ActiveMarketRate), Convert.ToDouble(mlLst[j].ActiveMarketRate));
                    else
                        installment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(interestRate, Convert.ToDouble(mlLst[j].ActiveMarketRate), Convert.ToDouble(mlLst[j].ActiveMarketRate), mlLst[j], CapBalance, Convert.ToInt32(mlLst[j].RemainingInstallments), 12);
                    if (mlLst[j].FinancialServiceType.Key == 1)
                    {
                        RateMarketRateVariable.Text = mlLst[j].ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                        RateDiscountVariable.Text = Convert.ToDouble(mlLst[j].RateAdjustment).ToString(SAHL.Common.Constants.RateFormat);
                        RateNewIntRateVariable.Text = interestRate.ToString(SAHL.Common.Constants.RateFormat);
                        RateNewInstallmentVariable.Text = installment.ToString(SAHL.Common.Constants.CurrencyFormat);
                        if (mlLst[j].HasInterestOnly())
                            RateAmortisationInstallmentVariable.Text = (SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(mlLst[j], interestRate, mlLst[j].RemainingInstallments, false)).ToString(SAHL.Common.Constants.CurrencyFormat);
                    }
                    else
                    {
                        RateMarketRateFixed.Text = mlLst[j].ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                        RateDiscountFixed.Text = Convert.ToDouble(mlLst[j].RateAdjustment).ToString(SAHL.Common.Constants.RateFormat);
                        RateNewIntRateFixed.Text = interestRate.ToString(SAHL.Common.Constants.RateFormat);
                        RateNewInstallmentFixed.Text = installment.ToString(SAHL.Common.Constants.CurrencyFormat);
                        if (mlLst[j].HasInterestOnly())
                            RateAmortisationInstallmentFixed.Text = (SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(mlLst[j], interestRate, mlLst[j].RemainingInstallments, false)).ToString(SAHL.Common.Constants.CurrencyFormat);
                    }
                }
            }
        }

        /// <summary>
        /// Button Click event to calculate new term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CalcNewTerm_Click(object sender, EventArgs e)
        {
            CalculateTerms();
        }

        public void CalculateTerms()
        {
            // Use each entry in the grid, check the type and do the calc.
            if (RatesGrid.Rows.Count == 0)
            {
                const string err = "There are no Financial Services to do the calculation on.";
                Messages.Add(new SAHL.Common.DomainMessages.Error(err, err));
                return;
            }

            if (NewRemainingTerm.Text.Length > 0)
            {
                int testint = int.Parse(NewRemainingTerm.Text);
                if (testint == 0 | testint > maximumTerm)
                {
                    string err = string.Format("The remaining term must be between 1 and {0} months.", maximumTerm);
                    Messages.Add(new SAHL.Common.DomainMessages.Error(err, err));
                    return;
                }
            }
            else
            {
                string err = string.Format("The remaining term must be between 1 and {0} months.", maximumTerm);
                Messages.Add(new SAHL.Common.DomainMessages.Error(err, err));
                return;
            }

            if (int.Parse(NewRemainingTerm.Text) > 0)
            {
                double installment = 0;

                for (int x = 0; x < mlLst.Count; x++)
                {
                    if (mlLst[x].AccountStatus.Key == (int)AccountStatuses.Open)
                    {
                        if (mlLst[x].HasInterestOnly())
                            installment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInterestOnlyInstallment(mlLst[x], CapBalance, Convert.ToDouble(mlLst[x].InterestRate), Convert.ToDouble(mlLst[x].ActiveMarketRate), Convert.ToDouble(mlLst[x].ActiveMarketRate));
                        else
                            installment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(Convert.ToDouble(mlLst[x].InterestRate), Convert.ToDouble(mlLst[x].ActiveMarketRate), Convert.ToDouble(mlLst[x].ActiveMarketRate), mlLst[x], CapBalance, int.Parse(NewRemainingTerm.Text), 12);

                        if (mlLst[x].FinancialServiceType.Key == 1) // Variable
                        {
                            TermNewInstallmentVariable.Text = installment.ToString(SAHL.Common.Constants.CurrencyFormat);
                            if (mlLst[x].HasInterestOnly())
                                AmortisationInstallmentVariable.Text = (SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(mlLst[x], mlLst[x].InterestRate, double.Parse(NewRemainingTerm.Text), false)).ToString(SAHL.Common.Constants.CurrencyFormat);
                        }
                        else
                        {
                            TermNewInstallmentFixed.Text = installment.ToString(SAHL.Common.Constants.CurrencyFormat);
                            if (mlLst[x].HasInterestOnly())
                                AmortisationInstallmentFixed.Text = (SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(mlLst[x], mlLst[x].InterestRate, double.Parse(NewRemainingTerm.Text), false)).ToString(SAHL.Common.Constants.CurrencyFormat);
                        }
                    }
                }

                if (mlLst.Count > 0)
                {
                    SetPTIVisibility = true;
                    IAccount _account = mlLst[0].Account;

                    double householdIncome = _account.GetHouseholdIncome();
                    if (householdIncome != 0)
                    {
                        _currentPTI = _account.CalcAccountPTI;
                        CurrentPTI.Text = _currentPTI.HasValue ? _currentPTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "";

                        _newPTI = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(installment, householdIncome);
                        NewPTI.Text = _newPTI.HasValue ? _newPTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "";
                    }
                }

                if (installment >= 0)
                    SubmitButton.Enabled = true;
            }
        }

        /// <summary>
        /// Handles Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles click event of submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (SubmitButtonClicked != null)
                SubmitButtonClicked(sender, new KeyChangedEventArgs(Convert.ToInt32(NewLinkRate.SelectedIndex)));
        }

        /// <summary>
        /// Event handler for Submit Button Clicked
        /// </summary>
        public event KeyChangedEventHandler SubmitButtonClicked;

        public event EventHandler CancelButtonClicked;

        protected void TermMemoComments_TextChanged(object sender, EventArgs e)
        {
        }
    }
}