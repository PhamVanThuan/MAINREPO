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
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Web.Views.FurtherLending
{
    public partial class ApplicationSummary : SAHLCommonBaseView, IApplicationSummary
    {
        #region locals

        private bool _hasArrears;
        private bool _titleDeedOnFile;
        private int _currentApplicationKey;
        private bool _selectedApplicationOnly;

        private double _appAmount;
        private double _totalFees;
        private string _spvDescription;
        private int _term;
        private double _marketRateVar;
        private double _margin;
        private double _discount;
        private double _appIncome;

        private bool _variFix;
        private bool _interestOnly;
        private bool _showCancelButton;
        private bool _showHistoryButton;

        private IApplicationRepository _appRepo;
        private IReasonRepository _reasonRepo;
        private IStageDefinitionRepository _stageRepo;
        private IOrganisationStructureRepository _orgRepo;
        private IMemoRepository _memoRepo;

        private bool showTwentyYearFigures;

        private bool _showStopOrderDiscountEligibility;

        #endregion

        #region page events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            if (!_variFix)
            {
                trCurBalF1.Visible = false;
                trCurBalF2.Visible = false;
                trCurBalF3.Visible = false;
                trInstal1.Visible = false;
                trInstal2.Visible = false;
                trInstal3.Visible = false;
                trRateF1.Visible = false;
                trRateF2.Visible = false;
                if (_interestOnly)
                {
                    lbInstalDesc.Text = "Interest Only Instalment:";
                    trInstal4.Visible = true;
                }
                else
                {
                    lbInstalDesc.Text = "Amortising Instalment:";
                }
            }
            else
            {
                lbInstalDesc.Text = "Total:";
                lbCurrentBalanceDesc.Text = "Total:";
            }

            btnCancel.Visible = _showCancelButton;
            btnTransitionHistory.Visible = _showHistoryButton;

            lblStopOrderDiscountEligibilityLabel.Visible = lblStopOrderDiscountEligibility.Visible = _showStopOrderDiscountEligibility;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// Navigate to the transition history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTransitionHistory_Click(object sender, EventArgs e)
        {
            OnTransitionHistoryClicked(sender, e);
        }
        #endregion

        #region private methods

        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void GetApplicationData(IApplicationProduct cp)
        {
            _appAmount = 0;
            //_bondRequired = 0;

            if (cp is IApplicationProductDefendingDiscountLoan)
            {
                IApplicationProductDefendingDiscountLoan prod = cp as IApplicationProductDefendingDiscountLoan;
                GetApplicationData(prod.VariableLoanInformation);

                if (null != prod.InterestOnlyInformation && prod.InterestOnlyInformation.MaturityDate.HasValue)
                {
                    _interestOnly = true;
                }
            }
            else if (cp is IApplicationProductNewVariableLoan)
            {
                IApplicationProductNewVariableLoan prod = cp as IApplicationProductNewVariableLoan;
                GetApplicationData(prod.VariableLoanInformation);

                if (null != prod.InterestOnlyInformation && prod.InterestOnlyInformation.MaturityDate.HasValue)
                {
                    _interestOnly = true;
                }
            }
            else if (cp is IApplicationProductSuperLoLoan)
            {
                IApplicationProductSuperLoLoan prod = cp as IApplicationProductSuperLoLoan;
                GetApplicationData(prod.VariableLoanInformation);

                if (null != prod.InterestOnlyInformation && prod.InterestOnlyInformation.MaturityDate.HasValue)
                {
                    _interestOnly = true;
                }
            }
            else if (cp is IApplicationProductVariableLoan)
            {
                IApplicationProductVariableLoan prod = cp as IApplicationProductVariableLoan;
                GetApplicationData(prod.VariableLoanInformation);

                if (null != prod.InterestOnlyInformation && prod.InterestOnlyInformation.MaturityDate.HasValue)
                {
                    _interestOnly = true;
                }
            }
            else if (cp is IApplicationProductVariFixLoan)
            {
                _variFix = true;

                IApplicationProductVariFixLoan prod = cp as IApplicationProductVariFixLoan;
                GetApplicationData(prod.VariableLoanInformation);
            }

            else if (cp is IApplicationProductEdge)
            {
                IApplicationProductEdge prod = cp as IApplicationProductEdge;
                GetApplicationData(prod.VariableLoanInformation);

                if (null != prod.InterestOnlyInformation && prod.InterestOnlyInformation.MaturityDate.HasValue)
                {
                    _interestOnly = true;
                }
            }



        }

        private void GetApplicationData(IApplicationInformationVariableLoan appvl)
        {
            _appAmount = appvl.LoanAmountNoFees.HasValue ? appvl.LoanAmountNoFees.Value : 0;
            //_bondRequired = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;
            _spvDescription = appvl.SPV.Description;
            _term = appvl.Term.HasValue ? appvl.Term.Value : 0;
            //_pti = appvl.PTI.HasValue ? appvl.PTI.Value : 0;
            _marketRateVar = appvl.MarketRate.HasValue ? appvl.MarketRate.Value : 0;
            _margin = appvl.RateConfiguration.Margin.Value;
            //_instalment = appvl.MonthlyInstalment.HasValue ? appvl.MonthlyInstalment.Value : 0;
            //_existingLoan = appvl.ExistingLoan.HasValue ? appvl.ExistingLoan.Value : 0;
            //_applaa = appvl.LoanAgreementAmount.HasValue ? appvl.LoanAgreementAmount.Value : 0;
            //_appPropVal = appvl.PropertyValuation.HasValue ? appvl.PropertyValuation.Value : 0;
            _appIncome = appvl.HouseholdIncome.HasValue ? appvl.HouseholdIncome.Value : 1;//using this in calcs, so use 1 to prevent / 0 errors
        }

        private void GetFurtherLoanSpecificData(IApplication app)
        {
            double initiationFee = 0;
            double registrationFee = 0;
            //double cancelFee = 0;
            //double interimInterest = 0;
            //double bondToRegister = 0;

            IApplicationMortgageLoan appML = app as IApplicationMortgageLoan;
            if (appML != null && appML.TransferringAttorney != null && appML.TransferringAttorney.Length > 0)
                    lbTransferringAttorney.Text = appML.TransferringAttorney;

            //IApplicationRole conAttorney = appML.GetFirstApplicationRoleByType(OfferRoleTypes.ConveyanceAttorney);
            IApplicationRole conAttorney = AppRepo.GetActiveApplicationRoleForTypeAndKey(appML.Key, (int)OfferRoleTypes.ConveyanceAttorney);

            if (conAttorney != null)
                lbConveyancingAttorney.Text = conAttorney.LegalEntity.DisplayName;

            //Better to just read the values than recalc, esp for old/NTU/Declined applications
            //AppRepo.CalculateOriginationFees(_appAmount, _bondRequired, ApplicationTypes.FurtherLoan, 0, 0, true, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister);
            foreach (IApplicationExpense ae in app.ApplicationExpenses)
            {
                if (ae.ExpenseType.Key == (int)ExpenseTypes.RegistrationFee)
                    registrationFee = ae.TotalOutstandingAmount;

                if (ae.ExpenseType.Key == (int)ExpenseTypes.InitiationFeeBondPreparationFee)
                    initiationFee = ae.TotalOutstandingAmount;
            }

            
            _totalFees = initiationFee + registrationFee;
            
            lbInititationFee.Text = initiationFee.ToString(SAHL.Common.Constants.CurrencyFormat);
            lbRegistrationFee.Text = registrationFee.ToString(SAHL.Common.Constants.CurrencyFormat);
            lbFeeTotal.Text = _totalFees.ToString(SAHL.Common.Constants.CurrencyFormat);

            ISupportsVariableLoanApplicationInformation vlai = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (vlai != null)
            {
                double bondSaved = vlai.VariableLoanInformation.BondToRegister.HasValue ? vlai.VariableLoanInformation.BondToRegister.Value : 0;
                lbBondToRegister.Text = bondSaved.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        private void PopulateCalculatedValues(IMortgageLoan vML, IMortgageLoan fML, IApplication app)
        {
            double accBalance = 0;
            double accVal = vML.GetActiveValuationAmount();
			double pricingAdjustment = app.GetRateAdjustments();

            _marketRateVar = vML.ActiveMarketRate;

            lbSPV.Text = vML.Account.SPV.Description;
            lbAppSPV.Text = _spvDescription;

            lbTerm.Text = vML.RemainingInstallments.ToString();


            if (fML != null)
            {
                double fixCB = fML.CurrentBalance;
                double varCB = vML.CurrentBalance;
                accBalance = varCB + fixCB;
                //app amount
                double appCB = _appAmount + _totalFees;

                //Rates
                double fixMarketRate = fML.ActiveMarketRate;
                double appVarRate = _marketRateVar + _margin + _discount;
                double appFixRate = fixMarketRate + _margin + _discount;

                lbMarketRate.Text = vML.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                lbAppMarketRate.Text = _marketRateVar.ToString(SAHL.Common.Constants.RateFormat);

                lbMargin.Text = vML.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                lbAppMargin.Text = _margin.ToString(SAHL.Common.Constants.RateFormat);

                lbAccountDiscount.Text =vML.RateAdjustment.ToString(SAHL.Common.Constants.RateFormat);
                lbAppDiscount.Text = _discount.ToString(SAHL.Common.Constants.RateFormat);

                lbEffectiveRateVariable.Text = (vML.ActiveMarketRate + vML.RateConfiguration.Margin.Value + vML.RateAdjustment).ToString(SAHL.Common.Constants.RateFormat);
                lbAppEffectiveRateVariable.Text = appVarRate.ToString(SAHL.Common.Constants.RateFormat);

                lbMarketRateFixed.Text = fixMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                lbAppMarketRateFixed.Text = fixMarketRate.ToString(SAHL.Common.Constants.RateFormat);

                lblPricingAdjustment.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
                lblAppPricingAdjustment.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);

                lbEffectiveRateFixed.Text = (fixMarketRate + fML.RateConfiguration.Margin.Value + fML.RateAdjustment).ToString(SAHL.Common.Constants.RateFormat);
                lbAppEffectiveRateFixed.Text = (fixMarketRate + _margin + _discount).ToString(SAHL.Common.Constants.RateFormat);

                //Balance
                lbCurrentBalance.Text = accBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                lbAppCurrentBalance.Text = (accBalance + appCB).ToString(SAHL.Common.Constants.CurrencyFormat);

                lbFixedBalance.Text = fixCB.ToString(SAHL.Common.Constants.CurrencyFormat);
                lbAppFixedBalance.Text = fixCB.ToString(SAHL.Common.Constants.CurrencyFormat);

                lbVariableBalance.Text = varCB.ToString(SAHL.Common.Constants.CurrencyFormat);
                lbAppVariableBalance.Text = (varCB + appCB).ToString(SAHL.Common.Constants.CurrencyFormat);
                //Instalments
                //Need to recalc all app instalments as rates could have changed
                lbInstalmentFixed.Text = fML.Payment.ToString(SAHL.Common.Constants.CurrencyFormat);
                double fixAppInst = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(fixCB, appFixRate, _term, false);
                lbAppInstalmentFixed.Text = fixAppInst.ToString(SAHL.Common.Constants.CurrencyFormat);

                lbInstalmentVariable.Text = vML.Payment.ToString(SAHL.Common.Constants.CurrencyFormat);
                double varAppInst = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment((varCB + appCB), appVarRate, _term, false);
                lbAppInstalmentVariable.Text = varAppInst.ToString(SAHL.Common.Constants.CurrencyFormat);

                lbInstalmentTotal.Text = (vML.Payment + fML.Payment).ToString(SAHL.Common.Constants.CurrencyFormat);
                lbAppInstalmentTotal.Text = (fixAppInst + varAppInst).ToString(SAHL.Common.Constants.CurrencyFormat);
                
                //PTI
                lbPTI.Text = vML.Account.CalcAccountPTI.ToString(SAHL.Common.Constants.RateFormat);
                lbAppPTI.Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI((fixAppInst + varAppInst), _appIncome).ToString(SAHL.Common.Constants.RateFormat);
                //LTV
                lbLTV.Text = (accBalance / accVal).ToString(SAHL.Common.Constants.RateFormat);
                lbAppLTV.Text = ((accBalance + appCB) / accVal).ToString(SAHL.Common.Constants.RateFormat);

            }
            else
            {
                double accRate = (vML.ActiveMarketRate) + vML.RateConfiguration.Margin.Value + (vML.RateAdjustment);
                lbEffectiveRateVariable.Text = accRate.ToString(SAHL.Common.Constants.RateFormat);
                double appRate = _marketRateVar + _margin + _discount;
                lbAppEffectiveRateVariable.Text = appRate.ToString(SAHL.Common.Constants.RateFormat);
                
                //need the total application + existing loan amount for calcs, as the rates could have changed
                double appCB = vML.CurrentBalance + _appAmount + _totalFees;

                lbInstalmentTotal.Text = vML.Payment.ToString(SAHL.Common.Constants.CurrencyFormat);
                //calc the app instalment as rates could have changed
                double appInst = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(appCB, appRate, _term, _interestOnly);
                lbAppInstalmentTotal.Text = appInst.ToString(SAHL.Common.Constants.CurrencyFormat);

                lbPTI.Text = vML.Account.CalcAccountPTI.ToString(SAHL.Common.Constants.RateFormat);
               
                lbLTV.Text = (vML.CurrentBalance / accVal).ToString(SAHL.Common.Constants.RateFormat);
                lbAppLTV.Text = (appCB / accVal).ToString(SAHL.Common.Constants.RateFormat);

                lbCurrentBalance.Text = vML.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                lbAppCurrentBalance.Text = appCB.ToString(SAHL.Common.Constants.CurrencyFormat);

                lbMarketRate.Text = vML.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                lbAppMarketRate.Text = _marketRateVar.ToString(SAHL.Common.Constants.RateFormat);

                lbMargin.Text = vML.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                lbAppMargin.Text = _margin.ToString(SAHL.Common.Constants.RateFormat);

                lbAccountDiscount.Text = vML.RateAdjustment.ToString(SAHL.Common.Constants.RateFormat);
                lbAppDiscount.Text = _discount.ToString(SAHL.Common.Constants.RateFormat);

				lblPricingAdjustment.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
				lblAppPricingAdjustment.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);

                if (_interestOnly)
                {
                    double appAMInst = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment((vML.CurrentBalance + _appAmount + _totalFees), appRate, _term, false);
                    double appAMTotal = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(vML.CurrentBalance, accRate, vML.RemainingInstallments, false);

                    //need to calc account and offer amortising instalment
                    lbInstalmentTotalAM.Text = appAMTotal.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lbAppInstalmentTotalAM.Text = appAMInst.ToString(SAHL.Common.Constants.CurrencyFormat); 
                    lbAppPTI.Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(appAMInst, _appIncome).ToString(SAHL.Common.Constants.RateFormat);

                }
                else
                {
                    lbAppPTI.Text = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(appInst, _appIncome).ToString(SAHL.Common.Constants.RateFormat);
                }

                lbl20yrCalculated.Visible = showTwentyYearFigures;
                lbl20yrTerm.Visible = showTwentyYearFigures;
                lbl20yrCurrentBalance.Visible = showTwentyYearFigures;
                lbl20yrLTV.Visible = showTwentyYearFigures;
                lbl20yrPTI.Visible = showTwentyYearFigures;
                lbl20yrInstalment.Visible = showTwentyYearFigures;
                lbl20yrMarketRate.Visible = showTwentyYearFigures;
                lbl20yrLinkRate.Visible = showTwentyYearFigures;
                lbl20yrDiscount.Visible = showTwentyYearFigures;
                lbl20yrPricingAdjustment.Visible = showTwentyYearFigures;
                lbl20yrEffectiveRateVariable .Visible = showTwentyYearFigures;

                if (showTwentyYearFigures)
                {
                    double thirtyYearPricingAdjustment = 0;
                    IEnumerable<IFinancialAdjustment> financialAdjustment30YearTerm = vML.FinancialAdjustments.Where(x => x.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.Loanwith30YearTerm &&
                        x.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active);

                    if (financialAdjustment30YearTerm.Count() > 0)
                    {
                        thirtyYearPricingAdjustment = financialAdjustment30YearTerm.Sum(x => x.InterestRateAdjustment.Adjustment);
                    }

                    ICalculated20YearLoanDetailsFor30YearTermLoan calculated20YearLoanDetails = LoanCalculator.Calculate20YearLoanDetailsFor30YearTermLoan(pricingAdjustment, thirtyYearPricingAdjustment, appCB, appRate, 240, _appIncome);

                    lbl20yrTerm.Text = "240";
                    lbl20yrCurrentBalance.Text = appCB.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lbl20yrLTV.Text = (appCB / accVal).ToString(SAHL.Common.Constants.RateFormat);
                    lbl20yrPTI.Text = calculated20YearLoanDetails.PTI.ToString(SAHL.Common.Constants.RateFormat);
                    lbl20yrInstalment.Text = calculated20YearLoanDetails.Instalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lbl20yrMarketRate.Text = _marketRateVar.ToString(SAHL.Common.Constants.RateFormat);
                    lbl20yrLinkRate.Text = _margin.ToString(SAHL.Common.Constants.RateFormat);
                    lbl20yrDiscount.Text = _discount.ToString(SAHL.Common.Constants.RateFormat);
                    lbl20yrPricingAdjustment.Text = calculated20YearLoanDetails.PricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
                    lbl20yrEffectiveRateVariable.Text = calculated20YearLoanDetails.EffectiveRate.ToString(SAHL.Common.Constants.RateFormat);
                }

            }

            lbAppTerm.Text = _term.ToString();
        }

        private void PopulateLatestReadvance(IMortgageLoan vML)
        {
            bool hasRa = false;
            DateTime dt = DateTime.Now;
            double amnt = 0;
            DateTime? dtOfferStart = DateTime.Now;
            DateTime? dtOfferEnd = DateTime.Now;

            // Get the start and end date for the selected offer then find readvance from LT between the dates
            foreach (IApplication app in vML.Account.Applications)
            {
                if (app.Key == CurrentApplicationKey)
                {
                    dtOfferStart = app.ApplicationStartDate;
                    dtOfferEnd = app.ApplicationEndDate;
                }

            }
            
            foreach (IFinancialTransaction ft in vML.FinancialTransactions)
            {                              

                if (ft.TransactionType.Key == (short)DisbursementLoanTransactionTypes.ReAdvance)// || lt.TransactionType.Key == (short)DisbursementTransactionTypes.ReadvanceCorrection)
                {

                    if (ft.EffectiveDate >= dtOfferStart && ft.EffectiveDate <= dtOfferEnd)
                    {
                        hasRa = true;
                        dt = ft.EffectiveDate;
                        amnt = ft.Amount;

                    }
                }
            }

            if (hasRa)
            {
                lbReadvanceRequired.Text= amnt.ToString(SAHL.Common.Constants.CurrencyFormat);                  
                lbLastReadvanceDate.Text = dt.ToString(SAHL.Common.Constants.DateFormat);
            }
            else
            {
                lbLastReadvanceDate.Text = "None.";
                lbLatestValuationAmount.Text = "None.";
            }

            lbLastReadvanceDate.Visible = true;
            lbLastReadvanceDateDesc.Visible = true;
            lbLastReadvanceAmount.Visible = true;
            lbLastReadvanceAmountDesc.Visible = true;
        }

        #endregion

        #region IApplicationSummary Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnTransitionHistoryClicked;

        public bool ShowCancelButton
        {
            set { _showCancelButton = value; }
        }

        public bool ShowHistoryButton
        {
            set { _showHistoryButton = value; }
        }

        public bool Show20YearFigures
        {
            get
            {
                return showTwentyYearFigures;
            }
            set
            {
                showTwentyYearFigures = value;
            }
        }

        public bool ShowStopOrderDiscountEligibility
        {
            set { _showStopOrderDiscountEligibility = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vML"></param>
        /// <param name="fML"></param>
        public void BindDisplay(IMortgageLoan vML, IMortgageLoan fML)
        {
            if (!ShouldRunPage) return;

            lbAccountLegalName.Text = vML.Account.GetLegalName(LegalNameFormat.Full);
            lbAccountNumber.Text = vML.Account.Key.ToString();
            lbAccountProduct.Text = vML.Account.Product.Description;
            IBond bond = vML.GetLatestRegisteredBond();
            lbBondRegDate.Text = bond.BondRegistrationDate.ToString(SAHL.Common.Constants.DateFormat);
            lbBondRegAmount.Text = vML.SumBondRegistrationAmounts().ToString(SAHL.Common.Constants.CurrencyFormat);

            foreach (IApplication app in vML.Account.Applications)
            {
                //if current app not open display only the current application
                if (SelectedApplicationOnly)
                {
                    //must only happen for the required application
                    if (app.Key == CurrentApplicationKey)
                        ProcessApplication(app, vML, fML);
                }
                else
                {

                    if ((app.ApplicationStatus.Key == (int)OfferStatuses.Open ||
                        app.Key == CurrentApplicationKey)
                        && (
                            app.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                            app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                            app.ApplicationType.Key == (int)OfferTypes.FurtherLoan
                        ))
                    {

                        ProcessApplication(app, vML, fML);
                    }
                }
            }

            //this could have been set in ProcessApplication above, stop all processing
            if (!ShouldRunPage) return;

            double arrearsBalance = (vML.ArrearBalance + (fML != null ? fML.ArrearBalance : 0));
            if (arrearsBalance > 0)
                lbArrearsBalance.Text = arrearsBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            else
                lbArrearsBalance.Text = (0).ToString(SAHL.Common.Constants.CurrencyFormat);

            lbHasArrears.Text = _hasArrears ? "Yes" : "No";

            lbHouseholdIncome.Text = _appIncome.ToString(SAHL.Common.Constants.CurrencyFormat);
            if (_appIncome != vML.Account.GetHouseholdIncome())
                lbHouseholdIncome.ForeColor = System.Drawing.Color.Red;

            DateTime? dt = vML.GetActiveValuationDate();
            lbLatestValuationAmount.Text = vML.GetActiveValuationAmount().ToString(SAHL.Common.Constants.CurrencyFormat);
            if (dt.HasValue)
                lbLatestValuationDate.Text = dt.Value.ToString(SAHL.Common.Constants.DateFormat);

            lbAccruedInterest.Text = ((vML.AccruedInterestMTD.HasValue ? vML.AccruedInterestMTD.Value : 0) + (fML != null ? (fML.AccruedInterestMTD.HasValue ? fML.AccruedInterestMTD.Value : 0) : 0)).ToString(SAHL.Common.Constants.CurrencyFormat);
            lbHaveTitleDeed.Text = _titleDeedOnFile ? "Yes" : "No";
        }

        private void ProcessApplication(IApplication app, IMortgageLoan vML, IMortgageLoan fML)
        {
            _discount = 0D;

            IApplicationInformation latestAppInfo = app.GetLatestApplicationInformation();

            if (app.ApplicationStatus.Key == (int)OfferStatuses.Open && latestAppInfo.Product.Key != app.Account.Product.Key)
            {
                string err = String.Format("The application is {0} and the Account is {1}. This application can not be processed. Please NTU and recreate this application.", latestAppInfo.Product.Description, app.Account.Product.Description);
                SAHL.Common.CacheData.SAHLPrincipalCache spc = SAHL.Common.CacheData.SAHLPrincipalCache.GetPrincipalCache(CurrentPrincipal);
                spc.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(err, err));
                ShouldRunPage = false;
                return;
            }


            foreach (IApplicationInformationFinancialAdjustment fa in latestAppInfo.ApplicationInformationFinancialAdjustments)
            {
                if (fa.FromDate < DateTime.Now && fa.Discount.HasValue)
                    _discount += fa.Discount.Value;
            }

            GetApplicationData(app.CurrentProduct);
            if (app.ApplicationStartDate.HasValue)
                lbApplicationCreatedDate.Text = app.ApplicationStartDate.Value.ToString(SAHL.Common.Constants.DateFormat);

            switch (app.ApplicationType.Key)
            {
                case (int)OfferTypes.ReAdvance:
                    //Value is now set by PopulateLatestReadvance
                    lbReadvanceRequired.Text = _appAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    break;
                case (int)OfferTypes.FurtherAdvance:
                    lbFurtherAdvanceRequired.Text = _appAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                    break;
                case (int)OfferTypes.FurtherLoan:
                    lbFurtherLoanRequired.Text = _appAmount.ToString(SAHL.Common.Constants.CurrencyFormat);

                    GetFurtherLoanSpecificData(app);
                    break;
                default:
                    break;
            }

            if (app.Key == _currentApplicationKey)
            {
                PopulateCalculatedValues(vML, fML, app);

                ISupportsVariableLoanApplicationInformation aivl = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (aivl != null && aivl.VariableLoanInformation != null && aivl.VariableLoanInformation.EmploymentType != null)
                {
                    lbEmploymentType.Text = aivl.VariableLoanInformation.EmploymentType.Description;
                }
                else
                {
                    lbEmploymentType.Text = "Unknown";
                }

                if (aivl != null && aivl.VariableLoanInformation != null && aivl.VariableLoanInformation.EmploymentType != null)
                {
                    if (app.HasAttribute(OfferAttributeTypes.ManuallySelectedEmploymentType))
                    {
                        var confirmedEmploymentStageTransitions = StageRepo.GetStageTransitionList(app.Key, (int)GenericKeyTypes.Offer, new List<int>() { (int)StageDefinitionStageDefinitionGroups.ApplicationEmploymentTypeCreditConfirmed, (int)StageDefinitionStageDefinitionGroups.ApplicationEmploymentTypeManageApplicationConfirmed });
                        var latestConfrimedEmploymentStageTransition = confirmedEmploymentStageTransitions.OrderByDescending(x => x.TransitionDate).FirstOrDefault();

                        lbEmploymentType.Text = lbEmploymentType.Text + " (" + latestConfrimedEmploymentStageTransition.ADUser.ADUserName + ")";
                    }
                    else
                    {
                        lbEmploymentType.Text = lbEmploymentType.Text + " (System)";
                    }
                }


                #region QuickCash
                // get quick cash information
                ISupportsQuickCashApplicationInformation qcai = app as ISupportsQuickCashApplicationInformation;
                if (qcai != null && qcai.ApplicationInformationQuickCash != null)
                {
                    IApplicationInformationQuickCash applicationInformationQuickCash = qcai.ApplicationInformationQuickCash;

                    lbQuickCashApproved.Text = applicationInformationQuickCash.CreditApprovedAmount.ToString(SAHL.Common.Constants.CurrencyFormat);

                    IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
                    IReadOnlyEventList<IReason> reasonList = reasonRepo.GetReasonByGenericKeyAndReasonTypeKey(applicationInformationQuickCash.ApplicationInformation.Key, (int)ReasonTypes.QuickCashDecline);
                    gridQCDeclineReasons.BindData(reasonList);

                    if (reasonList != null && reasonList.Count > 0)
                    {
                        apQuickCash.Visible = true;
                        trQCDecline.Visible = true;
                    }
                    else
                    {
                        if (applicationInformationQuickCash.CreditApprovedAmount > 0 || applicationInformationQuickCash.CreditUpfrontApprovedAmount > 0)
                        {
                            apQuickCash.Visible = true;
                            trQCApproved1.Visible = true;
                            trQCApproved2.Visible = true;

                            lblCreditApprovedAmount.Text = applicationInformationQuickCash.CreditApprovedAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                            lblUpfrontCreditApprovedAmount.Text = applicationInformationQuickCash.CreditUpfrontApprovedAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                        }
                    }
                }
                else
                {
                    lbQuickCashApproved.Text = "N/A";
                }

                #endregion

                if (app.ApplicationType.Key == (int)OfferTypes.ReAdvance)
                    PopulateLatestReadvance(vML);

                 IApplicationRole aRole = app.GetLatestApplicationRoleByType(OfferRoleTypes.FLProcessorD);
                if (aRole != null)
                {
                    IADUser adu = OrgRepo.GetAdUserByLegalEntityKey(aRole.LegalEntityKey);
                    if (adu != null)
                        lblApplicationProcessor.Text = adu.ADUserName;
                }

                aRole = app.GetLatestApplicationRoleByType(OfferRoleTypes.FLOriginatorD);
                if (aRole != null)
                {
                    IADUser adu = OrgRepo.GetAdUserByLegalEntityKey(aRole.LegalEntityKey);
                    if (adu != null)
                        lbApplicationCreator.Text = adu.ADUserName;
                }

                // build genericKeylist for getting reasons
                List<int> genericKeys = new List<int>();
                genericKeys.Add(app.Key);
                foreach (IApplicationInformation appInfo in app.ApplicationInformations)
                {
                    genericKeys.Add(appInfo.Key);
                }

                #region NTU Reasons
                if (app.ApplicationStatus.Key == (int)OfferStatuses.NTU)
                {
                    apNTU.Visible = true;
                    //GET NTU reasons  - using offerkey and offerinformationkey (if exists)
                    IReadOnlyEventList<IReason> reasonList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeGroupKey(genericKeys, (int)ReasonTypeGroups.NTU);

                    gridNTUReasons.BindData(reasonList);
                }
                #endregion

                #region CreditDecision
                //Get Credit user and decision and comments
                if (latestAppInfo.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer || app.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                {
                    //Get Credit user and decision and comments
                    int aduserKey;
                    string decision;
                    DateTime decisionDate;

                    AppRepo.GetCurrentCreditDecision(app.Key, out decision, out aduserKey, out decisionDate);


                    if (aduserKey >= 0) // aduser = -1 if no descision has been made
                    {
                        apCredit.Visible = true;

                        IADUser adUser = aduserKey == 0 ? null : OrgRepo.GetADUserByKey(aduserKey);

                        //set what sections need to display
                        if (app.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                        {
                            trDeclineReason.Visible = true;

                            IReadOnlyEventList<IReason> reasonList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(genericKeys, (int)ReasonTypes.CreditDecline);
                            gridCreditDeclineReasons.BindData(reasonList);

                            tbCreditDecision.Text = "Declined.";
                        }
                        else
                        {
                            trComment.Visible = true;

                            BindCreditComments(app.Key, decisionDate, adUser);
                            tbCreditDecision.Text = decision;
                        }

                        if (adUser != null && adUser.LegalEntity != null)
                            tbCreditUser.Text = adUser.LegalEntity.DisplayName;
                    }

                }

                #endregion

                #region AdminDeclineReasons
                //TRAC # 11313
                //Always display decline reasons, even if a credit decision has been made
                //Always show decline reasons if they exists, even if the offer is not declined

                //if (app.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                //{
                    //GET Decline reasons  - using offerkey and offerinformationkeys (if exists)
                    IReadOnlyEventList<IReason> adminList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(genericKeys, (int)ReasonTypes.AdministrativeDecline);
                    IReadOnlyEventList<IReason> branchList = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(genericKeys, (int)ReasonTypes.BranchDecline);
                    IEventList<IReason> rsList = new SAHL.Common.Collections.EventList<IReason>();

                    foreach (IReason r in adminList)
                    {
                        rsList.Add(null, r);
                    }

                    foreach (IReason r in branchList)
                    {
                        rsList.Add(null, r);
                    }

                    IReadOnlyEventList<IReason> rList = new SAHL.Common.Collections.ReadOnlyEventList<IReason>(rsList);

                    if (rList.Count > 0)
                    {
                        apAdminDecline.Visible = true;
                        grdAdminDeclineReasons.BindData(rList);
                    }
                //}


                #endregion
            }
        }

        private void BindCreditComments(int appKey, DateTime dt, IADUser adUser)
        {
            if (adUser == null)
                return;

            IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoList = MemoRepo.GetMemoByGenericKeyADUserAndDate(appKey, (int)GenericKeyTypes.Offer, dt, adUser);

            gridCreditComments.BindData(memoList);

        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasArrears
        {
            set { _hasArrears = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentApplicationKey
        {
            get { return _currentApplicationKey; }
            set { _currentApplicationKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SelectedApplicationOnly
        {
            get { return _selectedApplicationOnly; }
            set { _selectedApplicationOnly = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TitleDeedOnFile
        {
            set { _titleDeedOnFile = value; }
        }

        #endregion

        public void BindGrid(IApplication application)
        {
            AddTrace(this, "BindGrid_Start");

            List<SAHL.Web.Views.Common.ApplicationSummary.GridRowItem> lstGridItems = new List<SAHL.Web.Views.Common.ApplicationSummary.GridRowItem>();

            IReadOnlyEventList<IApplicationRole> roles = application.GetApplicationRolesByGroup(OfferRoleTypeGroups.Client);
            AddTrace(this, "BindGrid_Start_A");
            foreach (IApplicationRole r in roles)
            {
                SAHL.Web.Views.Common.ApplicationSummary.GridRowItem itm = new SAHL.Web.Views.Common.ApplicationSummary.GridRowItem();
                itm.ApplicationKey = application.Key.ToString();
                itm.LegalEntityName = r.LegalEntity.DisplayName;
                ILegalEntityNaturalPerson leNP = r.LegalEntity as ILegalEntityNaturalPerson;
                if (leNP != null)
                {
                    itm.IDCompanyNo = leNP.IDNumber == null ? (leNP.PassportNumber == null ? "" : leNP.PassportNumber) : leNP.IDNumber;
                    itm.MaritalStatus = leNP.MaritalStatus != null ? leNP.MaritalStatus.Description : "";
                }
                else
                {
                    ILegalEntityGenericCompany leC = r.LegalEntity as ILegalEntityGenericCompany;
                    itm.IDCompanyNo = leC.RegistrationNumber;
                    itm.MaritalStatus = "N/A";
                }
                itm.Role = r.ApplicationRoleType.Description;
                bool foundIncomeContributor = false;
                foreach (IApplicationRoleAttribute attribute in r.ApplicationRoleAttributes)
                {
                    if (attribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                    {
                        foundIncomeContributor = true;
                        break;
                    }
                }
                AddTrace(this, "BindGrid_Start_B");
                itm.IncomeContributor = foundIncomeContributor ? "Yes" : "No";

                ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                IEmploymentType employType = LER.GetLegalEntityEmploymentTypeForApplication(r.LegalEntity, application);
                itm.EmploymentType = employType != null ? employType.Description : "-";

                double employIncome = LER.GetLegalEntityIncomeForApplication(r.LegalEntity, application);

                itm.Income = employIncome.ToString(SAHL.Common.Constants.CurrencyFormat);

                if (r.LegalEntity.Employment != null && r.LegalEntity.Employment.Count > 0)
                {
                    IEmployment employment = r.LegalEntity.Employment.Where(x => x.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Current).FirstOrDefault();
                    if (employment != null)
                    {
                        if (employment.UnionMember == null)
                            itm.UnionMember = "Unknown";
                        else if (employment.UnionMember == true)
                            itm.UnionMember = "Yes";
                        else
                            itm.UnionMember = "No";
                    }
                    else { itm.UnionMember = "Unknown"; }
                }
                else { itm.UnionMember = "Unknown"; }

                lstGridItems.Add(itm);
            }

            gridSummary.AutoGenerateColumns = false;
            gridSummary.AddGridBoundColumn("ApplicationKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridSummary.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(21), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("IDCompanyNo", "ID/Company No", Unit.Percentage(12), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("Role", "Role", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("MaritalStatus", "Marital Status", Unit.Percentage(21), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("IncomeContributor", "Income Cont", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("Income", "Income", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("EmploymentType", "Employ Type", Unit.Percentage(18), HorizontalAlign.Left, true);
            gridSummary.AddGridBoundColumn("UnionMember", "Union Member", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridSummary.DataSource = lstGridItems;
            gridSummary.DataBind();

            AddTrace(this, "BindGrid_End");
        }

        protected IApplicationRepository AppRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        protected IReasonRepository ReasonRepo
        {
            get
            {
                if (_reasonRepo == null)
                    _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _reasonRepo;
            }
        }

        protected IStageDefinitionRepository StageRepo
        {
            get
            {
                if (_stageRepo == null)
                    _stageRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                return _stageRepo;
            }
        }

        protected IOrganisationStructureRepository OrgRepo
        {
            get
            {
                if (_orgRepo == null)
                    _orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _orgRepo;
            }
        }

        protected IMemoRepository MemoRepo
        {
            get
            {
                if (_memoRepo == null)
                    _memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();

                return _memoRepo;
            }
        }

    }
}
