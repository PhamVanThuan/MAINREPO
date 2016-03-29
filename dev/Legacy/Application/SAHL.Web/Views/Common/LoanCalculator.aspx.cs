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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    public partial class LoanCalculator : SAHLCommonBaseView, ILoanCalculator
    {
        #region locals

        private double _baseRateV;
        private double _baseRateF;
        private double _discountV;
        private double _discountF;
        private bool _hasInterestOnly;
        private bool _isFixed;
        private int _spvkey;
        private int _productkey;
        private double _capitalisedLife;

        private bool _isInstallmentVZero;
        private bool _isCurrentBalanceVZero;
        private bool _showLoanPercSplit;

        private double _maxBondAmount;

        private IEventList<IMargin> _listMargin;

        Dictionary<string, double> _calcDict; //_calcDict = new Dictionary<string, double>();


        #endregion

        protected void cboValueToCalculate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Request.Form[Calculate.UniqueID] == null)
            {
                //changedCalcType();
            }
        }

        protected string CBOValue
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[cboValueToCalculate.UniqueID]))
                    return (Request.Form[cboValueToCalculate.UniqueID]);
                else
                    return (cboValueToCalculate.SelectedValue);
           }
        }

        protected void cboLnkRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInterestRate();
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            Navigator.Navigate("LoanCalculator");
        }

        protected void Calculate_Click(object sender, EventArgs e)
        {
            bool isValidVariable = true;
            bool isValidFixed = true;

            // validate variable values
            isValidVariable = ValidateValuesV();
            
            // validate fixed values if they are visible
            if (pnlFixedLoan.Visible == true)
                isValidFixed = ValidateValuesF();

            if (isValidVariable && isValidFixed)
            {
                // Calculate by default
                CalculateValuesV();
                // Calculate only if there is a fixed portion available
                if (pnlFixedLoan.Visible)
                {
                    CalculateValuesF();
                }
  
                CalculateSplitPercentage();

                if (_hasInterestOnly == true)
                {
                    if ((cboValueToCalculate.SelectedValue == "Current Balance") ||
                        (cboValueToCalculate.SelectedValue == "Term"))
                    {
                        Amortisation.Enabled = true;
                    }
                }
                else
                {
                    Amortisation.Enabled = true;
                }
            }
            else
            {
                Amortisation.Enabled = false;
            }

        }

        void CalculateSplitPercentage()
        {
            //if the fields are visible then do the following
            string textVal = string.Empty;
            double SplitV; // Variable Loan
            double SplitF; // Fixed Loan

            if (Request.Form[txtCurrentBalanceV.UniqueID] != null)
            {
                textVal = Request.Form[txtCurrentBalanceV.UniqueID];
                SplitV = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

                //#4132 
                SplitV -= _capitalisedLife;

                if (Page.Request.Form[txtCurrentBalanceF.UniqueID] != null)
                {
                    textVal = Request.Form[txtCurrentBalanceF.UniqueID];
                    SplitF = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

                    SplitV = SplitV / (SplitV + SplitF);
                    SplitF = 1 - SplitV;

                    txtLoanSplitV.Text = SplitV.ToString(SAHL.Common.Constants.RateFormat);
                    txtLoanSplitF.Text = SplitF.ToString(SAHL.Common.Constants.RateFormat);
                }
                else
                {
                    SplitV = 1;
                    txtLoanSplitV.Text = SplitV.ToString(SAHL.Common.Constants.RateFormat);
                }
            }
        }

        private void CalculateValuesF()
        {
            double interestRate;
            int remainingTerm = 0;          // Remaining Term
            double currentBalance = 0;      // Current Balance
            double totalInstalment = 0;     // Total Instalment
            double TotalInterest = 0;       // Total Interest
            double InstCapital = 0;         // Instalment Capital
            double InstalmentInterest = 0;  // Instalment interest

            string val = string.Empty;
            string WhatToCalculate = cboValueToCalculate.SelectedValue;

            val = Request.Form[txtRemainingTermV.UniqueID];
            remainingTerm = String.IsNullOrEmpty(val) ? 0 : Convert.ToInt32(val);

            val = Request.Form[txtCurrentBalanceF.UniqueID];
            currentBalance = String.IsNullOrEmpty(val) ? 0 : Convert.ToDouble(val);

            val = Request.Form[txtInstallmentTotalF.UniqueID];
            totalInstalment = String.IsNullOrEmpty(val) ? 0 : Convert.ToDouble(val);

            interestRate = CalculateInterestRateF();

            double AmortisationInstallment = 0;
            AmortisationInstallment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, remainingTerm, _hasInterestOnly);


            switch (WhatToCalculate)
            {
                case "Instalment":
                    // Interest Rate
                    // Installment: Total
                    // Installment: Interest
                    // Installment: Capital
                    // Loan Split Percentage
                    // Total Interest
                    totalInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, remainingTerm, _hasInterestOnly);
                    CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, 0, out InstalmentInterest, out InstCapital);
                    TotalInterest = CalculatingTotalInterest(totalInstalment, remainingTerm, currentBalance);
                    break;
                case "Term":
                    // Interest Rate
                    // Remaining Term
                    // Installment: Interest
                    // Installment: Capital
                    // Loan Split Percentage
                    // Total Interest
                    remainingTerm = (int)CalculateRemainingTerm(currentBalance, totalInstalment, interestRate);
                    CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, 0, out InstalmentInterest, out InstCapital);
                    TotalInterest = CalculatingTotalInterest(totalInstalment, remainingTerm, currentBalance);
                    break;
                case "Current Balance":
                    // Interest Rate
                    // Current Balance
                    // Installment: Interest
                    // Installment: Capital
                    // Loan Split Percentage
                    // Total Interest
                    currentBalance = CalculatingCurrentBalance(totalInstalment, remainingTerm, interestRate);
                    CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, 0, out InstalmentInterest, out InstCapital);
                    TotalInterest = CalculatingTotalInterest(totalInstalment, remainingTerm, currentBalance);
                    break;
            }

            double marketRate = 0;
            if (Request.Form[txtMarketRateF.UniqueID] == null)
                marketRate = _baseRateF;
            else
            {
                marketRate = double.Parse(Request.Form[txtMarketRateV.UniqueID].Replace("%", ""));
                marketRate = marketRate / 100;
            }
            
            SetAmortisationValues(false, currentBalance, interestRate, remainingTerm, totalInstalment, AmortisationInstallment, marketRate);

            txtCurrentBalanceF.Amount = currentBalance;
            txtInstallmentCapitalF.Text = InstCapital.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentInterestF.Text = InstalmentInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentTotalF.Amount = totalInstalment;
            txtTotalInterestF.Text = TotalInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtRemainingTermF.Text = remainingTerm.ToString();
            AmortisationInstallmentFixed.Text = AmortisationInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            BindInterestRate();
            if ((WhatToCalculate == "Term") && (_isCurrentBalanceVZero == true) && (_isInstallmentVZero == true))
            {
                txtRemainingTermV.Text = remainingTerm.ToString();
            }
        }

        private void CalculateValuesV()
        {
            double interestRate;
            int remainingTerm = 0;          // Remaining Term
            double currentBalance = 0;      // Current Balance
            double totalInstalment = 0;     // Total Instalment
            double TotalInterest = 0;       // Total Interest
            double InstCapital = 0;         // Instalment Capital
            double InstalmentInterest = 0;  // Instalment interest

            string val = string.Empty;
            string WhatToCalculate = cboValueToCalculate.SelectedValue;


            val = Request.Form[txtRemainingTermV.UniqueID];
            remainingTerm = String.IsNullOrEmpty(val) ? 0 : Convert.ToInt32(val);

            val = Request.Form[txtCurrentBalanceV.UniqueID];
            currentBalance = String.IsNullOrEmpty(val) ? 0 : Convert.ToDouble(val);

            currentBalance -= _capitalisedLife;

            val = Request.Form[txtInstallmentTotalV.UniqueID];
            totalInstalment = String.IsNullOrEmpty(val) ? 0 : Convert.ToDouble(val);

            interestRate = CalculateInterestRateV();

            double AmortisationInstallment = 0;
            AmortisationInstallment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, remainingTerm, _hasInterestOnly);

            switch (WhatToCalculate)
            {
                case "Instalment":
                    // Interest Rate
                    // Installment: Total
                    // Installment: Interest
                    // Installment: Capital
                    // Loan Split Percentage
                    // Total Interest
                    totalInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, remainingTerm, _hasInterestOnly);
                    //CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, _capitalisedLife, out InstalmentInterest, out InstCapital);
                    CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, 0, out InstalmentInterest, out InstCapital);
                    TotalInterest = CalculatingTotalInterest(totalInstalment, remainingTerm, currentBalance);
                    break;
                case "Term":
                    // Interest Rate
                    // Remaining Term
                    // Installment: Interest
                    // Installment: Capital
                    // Loan Split Percentage
                    // Total Interest

                    // ?? WTF is all this ?? If we are using the totalInstalment to calculate other values, why are we recalculating the totalInstalment!??!
                    // This is needed as if the term differs when rounded we need to recalc installment accordingly
                    double newTerm = CalculateRemainingTerm(currentBalance, totalInstalment, interestRate);
                    if ((int)(Math.Ceiling(newTerm)) - newTerm > 0)
                    {
                        //we have rounded the value
                        totalInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, (int)(Math.Ceiling(newTerm)), _hasInterestOnly);
                        totalInstalment = ((double)((int)(Math.Ceiling(totalInstalment * 100)))) / 100;
                    }

                    remainingTerm = (int)(Math.Ceiling(newTerm));
                    //remainingTerm = (int)CalculateRemainingTerm(currentBalance, totalInstalment, interestRate);
                    //CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, _capitalisedLife, out InstalmentInterest, out InstCapital);
                    CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, 0, out InstalmentInterest, out InstCapital);
                    TotalInterest = CalculatingTotalInterest(totalInstalment, remainingTerm, currentBalance);
                    break;
                case "Current Balance":
                    // Interest Rate
                    // Current Balance
                    // Installment: Interest
                    // Installment: Capital
                    // Loan Split Percentage
                    // Total Interest
                    currentBalance = CalculatingCurrentBalance(totalInstalment, remainingTerm, interestRate);
                    //CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, _capitalisedLife, out InstalmentInterest, out InstCapital);
                    CalculateInstalmentPortion(currentBalance, interestRate, totalInstalment, 0, out InstalmentInterest, out InstCapital);
                    TotalInterest = CalculatingTotalInterest(totalInstalment, remainingTerm, currentBalance);
                    break;
            }
            txtCurrentBalanceV.Amount = currentBalance + _capitalisedLife;
            txtInstallmentCapitalV.Text = InstCapital.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentInterestV.Text = InstalmentInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentTotalV.Amount = totalInstalment;
            txtTotalInterestV.Text = TotalInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtRemainingTermV.Text = remainingTerm.ToString();
            BindInterestRate();
            AmortisationInstallmentVariable.Text = AmortisationInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            // 0 will be the generic screen
            // 1 will  be the Loan Calculator

            //FromScreen = 1;

            double marketRate = 0;
            if (Request.Form[txtMarketRateV.UniqueID] == null)
                marketRate = _baseRateV;
            else
            {
                marketRate = double.Parse(Request.Form[txtMarketRateV.UniqueID].Replace("%", ""));
                marketRate = marketRate / 100;
            }

            SetAmortisationValues(true, currentBalance, interestRate, remainingTerm, totalInstalment, AmortisationInstallment, marketRate);
        }

        protected void Amortisation_Click(object sender, EventArgs e)
        {
            Calculate_Click(sender, e);

            OnAmortisationScheduleClicked(sender, e);
        }

        protected void LastRateChange_Click(object sender, EventArgs e)
        {
            Navigator.Navigate("LastRateChange");
        }

        protected void grdMortgageLoan_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text + " months";
            }
        }

        protected void BindCalculationType()
        {
            cboValueToCalculate.Items.Add(new ListItem("Instalment", "Instalment"));
            cboValueToCalculate.Items.Add(new ListItem("Term", "Term"));
            cboValueToCalculate.Items.Add(new ListItem("Current Balance", "Current Balance"));
        }

        private static void CalculateInstalmentPortion(double CurrentBalance, double Rate, double Instalment, double CapitalisedLife, out double Interest, out double Capital)
        {
            Interest = Math.Round(((Rate / 12) * (CurrentBalance - CapitalisedLife)), 2);
            Capital = Instalment - Interest;
        }

        private static double CalculatingTotalInterest(double PMT, int nper, double CurBal)
        {
            // Calculating the Total Interest
            // Total Interest = ((Instalment: Total * nper) – Current Balance) 
            // TotalInterest - Total Interest
            // Total - total
            // nper - Remaining Term
            // CurBal - Current Balance
            double TotalInterest = (PMT * nper) - CurBal;
            return TotalInterest;
        }

        private static double CalculateRemainingTerm(double LV, double PMT, double i)
        {
            // Calculate the remaining term.
            // LV - Current Balance
            // i - interest
            // PMT - Remaining Amount
            // nper - New term
            //interest must be monthly for this calc
            i = i / 12;
            double nper;
            double top = System.Math.Log(1 - ((LV * i) / PMT));
            double bottom = System.Math.Log(1 / (1 + i));

            nper = top / bottom;
            return nper;
        }
        private static double CalculatingCurrentBalance(double PMT, int nper, double i)
        {
            // Calculating the Current Balance
            // PMT - Remaining total
            // nper - Remaining Term
            // i - Interest
            // LV - Current Balance

            i = i / 12;
            double top = 0;
            double bottom = i;
            double LV = 0;

            top = 1 - (1 / System.Math.Pow((1 + i), nper));

            LV = PMT * (top / bottom);
            return LV;

        }

        private double GetMarginByKeyFromList(int MarginKey)
        {
            foreach (IMargin m in _listMargin)
            {
                if (m.Key == MarginKey)
                    return m.Value;
            }
            throw new Exception("The margin does not exist in the list.");
        }

        private void BindInterestRate()
        {

            txtInterestRateV.Text = CalculateInterestRateV().ToString(SAHL.Common.Constants.RateFormat);
            
            if (_isFixed)
            {
                txtInterestRateF.Text = CalculateInterestRateF().ToString(SAHL.Common.Constants.RateFormat);
                txtLinkRateF.Text = SelectedMargin.ToString(SAHL.Common.Constants.RateFormat);
            }
        }

        #region Validations

        private bool ValidateValuesV()
        {
            bool validTerm = true;
            bool validCurrentBalance = true;
            bool validInstalment = true;

            string interestRateS = txtInterestRateV.Text.Replace("%", "");
            decimal interestRate = 0;
            if (decimal.TryParse(interestRateS, out interestRate))
            {
                if (interestRate > 100)
                {
                    string errorMessage = "The market rate and link rate musn't exceed 100%.";
                    this.Messages.Add(new Error(errorMessage, errorMessage));
                    return false;
                }
            }

            switch (cboValueToCalculate.SelectedValue)
            {
                case "Instalment":
                    validTerm = ValidateRemainingTermV();
                    validCurrentBalance = ValidateCurrentBalanceV();

                    return (validTerm && validCurrentBalance);

                case "Term":
                    validInstalment = ValidateInstallmentTotalV();
                    validCurrentBalance = SetCurrentBalance();
                    return (validInstalment && validCurrentBalance);

                case "Current Balance":
                    validTerm = ValidateRemainingTermV();
                    validInstalment = ValidateInstallmentTotalV();
                    return (validTerm && validInstalment);
            }

            return false;
        }

        private bool ValidateValuesF()
        {
            bool validCurrentBalance = true;
            bool validInstalment = true;

            // if this portion is not valid, there is no need to validate it.
            if (!pnlFixedLoan.Visible)
                return true;

            ////Peet van der Walt - #2701
            string interestRateS = txtInterestRateF.Text.Replace("%", "");
            decimal interestRate = 0;
            if (decimal.TryParse(interestRateS, out interestRate))
            {
                if (interestRate > 100)
                {
                    string errorMessage = "The market rate and link rate musn't exceed 100%.";
                    this.Messages.Add(new Error(errorMessage, errorMessage));
                    return false;
                }
            }

            switch (cboValueToCalculate.SelectedValue)
            {
                case "Instalment":
                    validCurrentBalance = ValidateCurrentBalanceF();
                    if (validCurrentBalance)
                        validCurrentBalance = SetCurrentBalance();

                    return validCurrentBalance;

                case "Term":
                    validCurrentBalance = ValidateCurrentBalanceF();
                    if (validCurrentBalance)
                        validCurrentBalance = SetCurrentBalance();

                    validInstalment = ValidateInstallmentTotalF();
                    return (validCurrentBalance && validInstalment);

                case "Current Balance":
                    return ValidateInstallmentTotalF();
            }
            return false;
        }
        
        bool ValidateRemainingTermV()
        {
            string textVal = Request.Form[txtRemainingTermV.UniqueID];
            int remainingTerm = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToInt32(textVal);
            string errorMessage = "";

            if (Request.Form[txtRemainingTermV.UniqueID] == null)
            {
                errorMessage = "Please enter a remaining term.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if (remainingTerm <= 0)
            {
                errorMessage = "The remaining term must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            
            if (_spvkey != (int)SAHL.Common.Globals.SPVs.BlueBannerAgencyAccount //16
                && _spvkey != (int)SAHL.Common.Globals.SPVs.MainStreet65PtyLtd // 17
                && _spvkey != (int)SAHL.Common.Globals.SPVs.TheThekwiniWarehousingConduitPtyLtd // 24
                && _spvkey != (int)SAHL.Common.Globals.SPVs.BlueBannerSecuritisationVehicleRC1PtyLtd) //26
            {
                if (_productkey == (int)SAHL.Common.Globals.Products.VariableLoan
                 || _productkey == (int)SAHL.Common.Globals.Products.VariFixLoan)
                {
                    if (remainingTerm > 240)
                    {
                        errorMessage = "The total term may not exceed 240 months.";
                        this.Messages.Add(new Error(errorMessage, errorMessage));
                        return false;
                    }
                }
                else if (_productkey == (int)SAHL.Common.Globals.Products.SuperLo)
                {
                    if (remainingTerm > 276)
                    {
                        errorMessage = "The total term may not exceed 276 months.";
                        this.Messages.Add(new Error(errorMessage, errorMessage));
                        return false;
                    }
                }
            }
            else
            {
                if (remainingTerm > 480)
                {
                    errorMessage = "The total term may not exceed 480 months.";
                    this.Messages.Add(new Error(errorMessage, errorMessage));
                    return false;
                }
            }

            return true;
        }
        
        bool ValidateCurrentBalanceV()
        {
            string textVal = Request.Form[txtCurrentBalanceV.UniqueID];
            double currentBalance = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

            string errorMessage = "";

            if (currentBalance <= 0)
            {
                errorMessage = "The current balance must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                _isCurrentBalanceVZero = true;
                return false;
            }

            if (currentBalance > _maxBondAmount)
            {
                errorMessage = "The current balance must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            return true;
        }

        bool ValidateInstallmentTotalV()
        {
            string errorMessage = "";

            string textVal = Request.Form[txtInstallmentTotalV.UniqueID];
            double totalInstalment = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);
            textVal = Request.Form[txtCurrentBalanceV.UniqueID];
            double currentBalance = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

            double interestRate = CalculateInterestRateV();

            currentBalance -= _capitalisedLife;

            if (totalInstalment <= 0)
            {
                errorMessage = "The instalment must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                _isInstallmentVZero = true;
                return false;
            }

            if (totalInstalment > _maxBondAmount)
            {
                errorMessage = "The instalment must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if ((cboValueToCalculate.SelectedValue == "Term") || (cboValueToCalculate.SelectedValue == "Current Balance"))
            {
                if (_hasInterestOnly == true)
                {
                    //calculate interest only installment...
                    double intOnlyInst = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, 240, _hasInterestOnly);
                    if (totalInstalment <= intOnlyInst)
                    {
                        string Amnt = intOnlyInst.ToString(SAHL.Common.Constants.CurrencyFormat);

                        errorMessage = string.Format("The Instalment: Total amount must be greater than {0}", Amnt);
                        this.Messages.Add(new Error(errorMessage, errorMessage));
                        return false;
                    }
                }
            }

            return true;
        }
        
        bool SetCurrentBalance()
        {
            if (pnlFixedLoan.Enabled)
            {
                double TotalBalance = 0;

                string textVal = Request.Form[txtCurrentBalanceV.UniqueID];
                double currentBalanceV = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

                textVal = Request.Form[txtCurrentBalanceF.UniqueID];
                double currentBalanceF = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

                TotalBalance = currentBalanceV + currentBalanceF;

                if (TotalBalance > _maxBondAmount)
                {
                    string errorMessage = "The total balance must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    this.Messages.Add(new Error(errorMessage, errorMessage));
                    return false;
                }
            }
            return true;
        }

        bool ValidateCurrentBalanceF()
        {
            string textVal = Request.Form[txtCurrentBalanceF.UniqueID];
            double currentBalance = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

            string errorMessage = "";

            if (currentBalance <= 0)
            {
                errorMessage = "The current balance must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if (currentBalance > _maxBondAmount)
            {
                errorMessage = "The current balance must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            return true;

        }
    
        bool ValidateInstallmentTotalF()
        {
            string errorMessage = "";

            string textVal = Request.Form[txtInstallmentTotalF.UniqueID];
            double totalInstalment = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);
            textVal = Request.Form[txtCurrentBalanceF.UniqueID];
            double currentBalance = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

            double interestRate = CalculateInterestRateF();

            currentBalance -= _capitalisedLife;

            if (totalInstalment <= 0)
            {
                errorMessage = "The instalment must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if (totalInstalment > _maxBondAmount)
            {
                errorMessage = "The instalment must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if ((cboValueToCalculate.SelectedValue == "Term") || (cboValueToCalculate.SelectedValue == "Current Balance"))
            {
                if (_hasInterestOnly == true)
                {
                    //calculate interest only installment...
                    double intOnlyInst = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, 240, _hasInterestOnly);
                    if (totalInstalment <= intOnlyInst)
                    {
                        string Amnt = intOnlyInst.ToString(SAHL.Common.Constants.CurrencyFormat);

                        errorMessage = string.Format("The Instalment: Total amount must be greater than {0}", Amnt);
                        this.Messages.Add(new Error(errorMessage, errorMessage));
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowLoanPercSplit
        {
            set { _showLoanPercSplit = value; }
        }

        #region common Methods

        //private static bool isTermNumeric(string val, out int result)
        //{
        //    return int.TryParse(val, out result);
        //}
        private double CalculateInterestRateV()
        {
            double _calcInterestRateV = 0;
            if (Request.Form[txtMarketRateV.UniqueID] == null)
                _calcInterestRateV = _baseRateV;
            else
                _calcInterestRateV = double.Parse(Request.Form[txtMarketRateV.UniqueID].Replace("%", ""));

            _calcInterestRateV = _calcInterestRateV / 100;

            return (SelectedMargin + _calcInterestRateV + _discountV);
        }

        private double CalculateInterestRateF()
        {
            double _calcInterestRateF = 0;

            if (Request.Form[txtMarketRateF.UniqueID] == null)
                _calcInterestRateF = _baseRateF;
            else
                _calcInterestRateF = double.Parse(Request.Form[txtMarketRateF.UniqueID].Replace("%", ""));

            _calcInterestRateF = _calcInterestRateF / 100;

            return (SelectedMargin + _calcInterestRateF + _discountF);
        }
        
        #endregion

        #endregion

        public void ChangedCalcType()
        {
            // Set all the controls to readonly
            txtRemainingTermV.ReadOnly = true;
            txtCurrentBalanceV.ReadOnly = true;
            txtCurrentBalanceF.ReadOnly = true;
            txtInstallmentTotalV.ReadOnly = true;
            txtInstallmentTotalF.ReadOnly = true;

            switch (CBOValue)
            {
                case "Term":
                    txtCurrentBalanceV.ReadOnly = false;
                    txtCurrentBalanceF.ReadOnly = false;
                    txtInstallmentTotalV.ReadOnly = false;
                    txtInstallmentTotalF.ReadOnly = false;

                    AmortisatisionRow1.Visible = false;
                    AmortisatisionRow2.Visible = false;

                    txtRemainingTermF.CssClass = txtRemainingTermF.CssClass + " genericCalcResult";
                    txtRemainingTermV.CssClass = txtRemainingTermV.CssClass + " genericCalcResult";

                    txtRemainingTermF.Mandatory = false;
                    txtRemainingTermV.Mandatory = false;
                    txtCurrentBalanceV.Mandatory = true;
                    txtCurrentBalanceF.Mandatory = true;
                    txtInstallmentTotalV.Mandatory = true;
                    txtInstallmentTotalF.Mandatory = true;

                    break;
                case "Instalment":
                    txtRemainingTermV.ReadOnly = false;
                    txtCurrentBalanceV.ReadOnly = false;
                    txtCurrentBalanceF.ReadOnly = false;

                    if (_hasInterestOnly)
                    {
                        AmortisatisionRow1.Visible = true;
                        AmortisatisionRow2.Visible = true;
                    }
                    else
                    {
                        AmortisatisionRow1.Visible = false;
                        AmortisatisionRow2.Visible = false;
                    }

                    txtInstallmentTotalF.CssClass = txtInstallmentTotalF.CssClass + " genericCalcResult";
                    txtInstallmentTotalV.CssClass = txtInstallmentTotalV.CssClass + " genericCalcResult";

                    txtRemainingTermF.Mandatory = true;
                    txtRemainingTermV.Mandatory = true;
                    txtCurrentBalanceV.Mandatory = true;
                    txtCurrentBalanceF.Mandatory = true;
                    txtInstallmentTotalV.Mandatory = false;
                    txtInstallmentTotalF.Mandatory = false;

                    break;
                case "Current Balance":
                    txtRemainingTermV.ReadOnly = false;
                    txtInstallmentTotalV.ReadOnly = false;
                    txtInstallmentTotalF.ReadOnly = false;

                    AmortisatisionRow1.Visible = false;
                    AmortisatisionRow2.Visible = false;

                    txtCurrentBalanceF.CssClass = txtCurrentBalanceF.CssClass + " genericCalcResult";
                    txtCurrentBalanceV.CssClass = txtCurrentBalanceV.CssClass + " genericCalcResult";

                    txtRemainingTermF.Mandatory = true;
                    txtRemainingTermV.Mandatory = true;
                    txtCurrentBalanceV.Mandatory = false;
                    txtCurrentBalanceF.Mandatory = false;
                    txtInstallmentTotalV.Mandatory = true;
                    txtInstallmentTotalF.Mandatory = true;

                    break;
            }
        }

        #region Dictionary stuff

         private void SetAmortisationValues(Boolean Variable, double CurrentBalance, double InterestRate, double RemainingTerm, double InstalmentTotal, double AmorInstallment, double MarketRate)
        {
            if (Variable)
            {
                CalcDict.Add("CurrentBalanceV", CurrentBalance);
                CalcDict.Add("InterestRateV", InterestRate);
                CalcDict.Add("RemainingTermV", RemainingTerm);
                CalcDict.Add("InstalmentTotalV", InstalmentTotal);
                CalcDict.Add("AmorInstallmentV", AmorInstallment);
                CalcDict.Add("MarketRateV", MarketRate);

            }
            else
            {
                CalcDict.Add("CurrentBalanceF", CurrentBalance);
                CalcDict.Add("InterestRateF", InterestRate);
                CalcDict.Add("RemainingTermF", RemainingTerm);
                CalcDict.Add("InstalmentTotalF", InstalmentTotal);
                CalcDict.Add("AmorInstallmentF", AmorInstallment);
                CalcDict.Add("MarketRateF", MarketRate);
            }
        }

        #endregion

        #region ILoanCalculator Members

        public void BindFinancialServiceGrid(IList<BindableFinancialService> ListFinancialService)
        {
            grdMortgageLoan.AddGridBoundColumn("Service", "Service", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdMortgageLoan.AddGridBoundColumn("Rate", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Interest Rate", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            grdMortgageLoan.AddGridBoundColumn("Term", SAHL.Common.Constants.NumberFormat, GridFormatType.GridString, "Remaining Term", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            grdMortgageLoan.AddGridBoundColumn("LoanSplit", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Loan Split Percentage", false, Unit.Percentage(10), HorizontalAlign.Right, _showLoanPercSplit);
            grdMortgageLoan.AddGridBoundColumn("Period", "Period", Unit.Percentage(5), HorizontalAlign.Right, true);
            grdMortgageLoan.AddGridBoundColumn("CurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", true, Unit.Percentage(12), HorizontalAlign.Right, true);
            grdMortgageLoan.AddGridBoundColumn("InstalmentCapital", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment: Capital", true, Unit.Percentage(12), HorizontalAlign.Right, true);
            grdMortgageLoan.AddGridBoundColumn("InstalmentInterest", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment: Interest", true, Unit.Percentage(12), HorizontalAlign.Right, true);
            grdMortgageLoan.AddGridBoundColumn("Payment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment: Total", true, Unit.Percentage(12), HorizontalAlign.Right, true);

            grdMortgageLoan.DataSource = ListFinancialService;
            grdMortgageLoan.DataBind();

            BindCalculationType();
        }

        public void BindLinkRates(IEventList<IMargin> ListMargin)
        {
            _listMargin = ListMargin;

            cboLnkRate.DataTextField = "Description";
            cboLnkRate.DataValueField = "Key";
            cboLnkRate.DataSource = _listMargin;
            cboLnkRate.DataBind();
        }

        public void BindMortgageLoans(IMortgageLoan vml, IMortgageLoan fml, double splitV, double CapitalisedLife)
        {
            _hasInterestOnly = vml.HasInterestOnly();
            _spvkey = vml.Account.SPV.Key;
            _productkey = vml.Account.Product.Key;
            _capitalisedLife = CapitalisedLife;
            double interest;
            double capital;

            txtRemainingTermV.Text = vml.RemainingInstallments.ToString();
            cboLnkRate.SelectedValue = vml.RateConfiguration.Margin.Key.ToString();
            _baseRateV = vml.ActiveMarketRate;
            txtMarketRateV.Text = _baseRateV.ToString(SAHL.Common.Constants.RateFormat);
            txtInterestRateV.Text = vml.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
            txtCurrentBalanceV.Amount = vml.CurrentBalance;
            txtLoanSplitV.Text = splitV.ToString(SAHL.Common.Constants.RateFormat);
            CalculateInstalmentPortion(vml.CurrentBalance, vml.InterestRate, vml.Payment, CapitalisedLife, out interest, out capital);
            txtInstallmentCapitalV.Text = capital.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentInterestV.Text = interest.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentTotalV.Amount = vml.Payment;
            //can not calculate with 0 or negative values
            if ((vml.CurrentBalance - _capitalisedLife) <= 0)
                txtTotalInterestV.Text = 0.ToString(SAHL.Common.Constants.CurrencyFormat);
            else
                txtTotalInterestV.Text = (SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm((vml.CurrentBalance - _capitalisedLife), vml.InterestRate, vml.RemainingInstallments, _hasInterestOnly)).ToString(SAHL.Common.Constants.CurrencyFormat);

            _discountV = vml.RateAdjustment;
            if (_discountV != 0.0)
            {
                lblDiscountV.Text = _discountV.ToString(SAHL.Common.Constants.RateFormat);
                rowDiscountV.Visible = true;
            }

            if (fml != null)
            {
                _isFixed = true;
                txtRemainingTermF.Text = fml.RemainingInstallments.ToString();
                txtLinkRateF.Text = fml.RateConfiguration.Margin.Description;
                _baseRateF = fml.ActiveMarketRate; 
                txtMarketRateF.Text = _baseRateF.ToString(SAHL.Common.Constants.RateFormat);
                txtInterestRateF.Text = fml.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
                txtCurrentBalanceF.Amount = fml.CurrentBalance;
                txtLoanSplitF.Text = (1 - splitV).ToString(SAHL.Common.Constants.RateFormat);
                CalculateInstalmentPortion(fml.CurrentBalance, fml.InterestRate, fml.Payment, CapitalisedLife, out interest, out capital);
                txtInstallmentCapitalF.Text = capital.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtInstallmentInterestF.Text = interest.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtInstallmentTotalF.Amount = fml.Payment;
                //can not calculate with 0 or negative values
                if (fml.CurrentBalance <= 0)
                    txtTotalInterestF.Text = 0.ToString(SAHL.Common.Constants.CurrencyFormat);
                else
                    txtTotalInterestF.Text = (SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(fml.CurrentBalance, fml.InterestRate, fml.RemainingInstallments, _hasInterestOnly)).ToString(SAHL.Common.Constants.CurrencyFormat);
                
                _discountF = fml.RateAdjustment;

                if (_discountF != 0.0)
                {
                    lblDiscountF.Text = _discountF.ToString(SAHL.Common.Constants.RateFormat);
                    rowDiscountF.Visible = true;
                }

            }
            else
            {
                LoanSplitRow.Attributes.Add("style", "display: none");
                pnlFixedLoan.Attributes.Add("style", "display: none");
                pnlFixedLoan.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnAmortisationScheduleClicked;

        public Dictionary<string, double> CalcDict
        {
            get
            {
                if (_calcDict == null)
                    _calcDict = new Dictionary<string, double>();

                return _calcDict;
            }
        }

        public double MaxBondAmount
        {
            get
            {
                return _maxBondAmount;

            }
            set
            {
                _maxBondAmount = value;
            }
        }

        #endregion

        #region Properties

        private double SelectedMargin
        {
            get 
            {
                return GetMarginByKeyFromList(Convert.ToInt32(cboLnkRate.SelectedValue)); ; 
            }
        }

        #endregion

    }
}
