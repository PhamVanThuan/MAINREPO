using Castle.ActiveRecord;
using SAHL.Common;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class LoanCalculatorBase : SAHLCommonBasePresenter<ILoanCalculator>
    {
        #region Locals

        private ILookupRepository _lookupRepo;
        private IApplicationRepository _appRepo;
        private IControlRepository _ctrlRepo;
        private IFinancialAdjustmentRepository _financialAdjustmentRepository;
        private ICreditMatrixRepository _cmRepo;
        private IX2Repository _x2Repo;
        private IRuleService _ruleServ;

        protected IApplication _app;

        protected IApplicationStatus _statusOpen;
        protected ICreditMatrix _cm;
        protected ICategory _cat;
        protected IEmploymentType _empType;
        protected IOriginationSource _os;
        protected int _marketRateKey;
        protected int _maturityMonths = 6;
        protected int _rateConfigKey;
        protected IResetConfiguration _resetConfig;

        #endregion Locals

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanCalculatorBase(ILoanCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.EdgeTerm = Convert.ToInt32(CtrlRepo.GetControlByDescription("Edge Max Term").ControlNumeric);
            _view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);

            //TODO: get from lookups to avoid DB call for listProduct and listmlp
            int[] iPurposes = new int[]
                {
                    (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan,
                    (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                    (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
                };

            ReadOnlyEventList<IProduct> listProduct = AppRepo.GetOriginationProducts();
            ReadOnlyEventList<IMortgageLoanPurpose> listmlp = AppRepo.GetMortgageLoanPurposes(iPurposes);
            IEventList<IEmploymentType> listEmpType = LookupRepo.EmploymentTypes;

            // the code below has been removed and the 'Unknown' employment type is removed in the
            // bind method in the view. This is because the local list is actually a pointer to the lookup
            // and when the item is removed from the local list it is actually removed from the lookup aswell.
            //foreach (IEmploymentType empType in listEmpType)
            //{
            //    if (empType.Key == (int)EmploymentTypes.Unknown)
            //        listEmpType.Remove(_view.Messages, empType);
            //}

            IEventList<IApplicationSource> marketSource = new EventList<IApplicationSource>();
            IEnumerable<IApplicationSource> marketingSources = _lookupRepo.ApplicationSources.OrderBy(x => x.Description);
            foreach (IApplicationSource marketingSource in marketingSources)
            {
                if (marketingSource.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    marketSource.Add(_view.Messages, marketingSource);
            }

            _view.PopulateMarketingSource(marketSource);
            _view.BindProductDropdown(listProduct);
            _view.BindPurposeDropdown(listmlp);
            _view.BindEmploymentType(listEmpType);
        }

        protected void _view_OnCalculateButtonClicked(object sender, EventArgs e)
        {
            if (_view.MarketingSource.Length == 0)
            {
                _view.Messages.Add(new Error("Please specify a marketing source.", "Please specify a marketing source."));
                return;
            }

            // Business rule checks, populate the messages and return if any have been added.
            AppRepo.CreditDisqualifications(false, 0, 0, _view.HouseholdIncome, _view.LoanAmountRequired, _view.EstimatedPropertyValue, _view.EmploymentTypeKey, false, _view.Term, false);
            CheckBusinessRules();

            if (!_view.IsValid)
                return;

            CreateNewApplication();

            // Instead of all this old stuff, just use the common recalc method
            double initiationFee = 0;
            double registrationFee = 0;
            double cancelFee = 0;
            double interimInterest = 0;

            //double bondToRegister;
            double baseRate = 0;
            double baseRateFix = 0;
            double loanAmount = 0;
            double loanAmountVar = 0;
            double loanAmountFix = 0;
            double linkRate = 0;
            double interestRate = 0;
            double interestRateFix = 0;
            double instalmentTotal = 0;
            double instalmentIOTotal = 0;
            double instalmentVar = 0;
            double instalmentFix = 0;
            double instalmentEHLAM = 0D;
            double pti = 1D;
            double ptiFix = 0;
            double interestOverTermIO = 0;
            double interestOverTermVar = 0;
            double interestOverTermFix = 0;
            double minIncome;
            double ltv = 2D;
            double totalFees;
            double percentFix = 0;

            IEventList<IMarketRate> mRates = LookupRepo.MarketRates;
            foreach (IMarketRate mR in mRates)
            {
                if (mR.Key == 4) //18th reset rate
                    baseRate = mR.Value;

                if (mR.Key == _view.VarifixMarketRateKey) //VF 6month rate
                    baseRateFix = mR.Value;

                if (mR.Key == _view.VarifixMarketRateKey) //VF 5yr rate
                    baseRateFix = mR.Value;
            }

            IApplicationMortgageLoan appML = _app as IApplicationMortgageLoan;
            ISupportsVariableLoanApplicationInformation sVLAppInfo = appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            ISupportsInterestOnlyApplicationInformation ioAppInfo = appML.CurrentProduct as ISupportsInterestOnlyApplicationInformation;
            IApplicationInformationVariableLoan aivl = null;
            ICreditCriteria cc = null;

            if (sVLAppInfo != null)
            {
                aivl = sVLAppInfo.VariableLoanInformation;
            }

            if (aivl != null)
            {
                loanAmount = aivl.LoanAgreementAmount.HasValue ? aivl.LoanAgreementAmount.Value : 0;
                interimInterest = aivl.InterimInterest.HasValue ? aivl.InterimInterest.Value : 0D;

                //bondToRegister = aivl.BondToRegister.HasValue ? aivl.BondToRegister.Value : 0D;
                ltv = aivl.LTV.HasValue ? aivl.LTV.Value : 2D;
                pti = aivl.PTI.HasValue ? aivl.PTI.Value : 1D;
                cc = aivl.CreditCriteria;
                linkRate = cc.Margin.Value;
                interestRate = aivl.MarketRate.Value + aivl.RateConfiguration.Margin.Value;
                interestRateFix = cc.Margin.Value + baseRateFix;

                //the only time the instalment var != total instalment is for VF.
                //this split is done in the VF section below.
                instalmentVar = aivl.MonthlyInstalment.HasValue ? aivl.MonthlyInstalment.Value : 0D;
                instalmentTotal = instalmentVar;

                if ((_view.InterestOnly || _view.ProductKey == (int)Products.Edge) && ioAppInfo != null && ioAppInfo.InterestOnlyInformation != null)
                    instalmentIOTotal = ioAppInfo.InterestOnlyInformation.Installment.HasValue ? ioAppInfo.InterestOnlyInformation.Installment.Value : 0D;
            }

            foreach (IApplicationExpense applicationExpense in appML.ApplicationExpenses)
            {
                switch (applicationExpense.ExpenseType.Key)
                {
                    case (int)ExpenseTypes.InitiationFeeBondPreparationFee:
                        initiationFee = applicationExpense.TotalOutstandingAmount;
                        break;

                    case (int)ExpenseTypes.RegistrationFee:
                        registrationFee = applicationExpense.TotalOutstandingAmount;
                        break;

                    case (int)ExpenseTypes.CancellationFee:
                        cancelFee = applicationExpense.TotalOutstandingAmount;
                        break;
                    default:
                        break;
                }
            }
            totalFees = initiationFee + registrationFee + cancelFee;

            if (cc == null)
                _view.Messages.Add(new Error("The values captured do not fit our lending criteria. Try a lower loan amount.", "The values captured do not fit our lending criteria. Try a lower loan amount."));

            if (_view.Messages.Count > 0)
                return;

            _view.CreditMatrixKey = cc.CreditMatrix.Key;
            _view.CategoryKey = cc.Category.Key;

            //rates set
            _view.MarginKey = cc.Margin.Key;
            _view.ActiveMarketRate = baseRate;
            _view.LinkRate = linkRate;
            _view.InterestRateFix = interestRateFix;
            _view.InterestRate = interestRate;

            //Calculate interest over term
            //Need to do extra work for EHL here, some IO, some Var
            if (_view.ProductKey == (int)Products.Edge)
            {
                IApplicationProductEdge appEHL = appML.CurrentProduct as IApplicationProductEdge;
                int InterestOnlyTerm = appEHL.EdgeInformation.InterestOnlyTerm;

                instalmentEHLAM = appEHL.EdgeInformation.AmortisationTermInstalment;

                interestOverTermVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, _view.EdgeTerm - InterestOnlyTerm, false);
                interestOverTermVar += SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, InterestOnlyTerm, true);
            }
            else
            {
                interestOverTermVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, _view.Term, false);
                if (_view.InterestOnly)
                    interestOverTermIO = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmount, interestRate, _view.Term, _view.InterestOnly);
            }

            //Calculate fixed amounts
            if (_view.ProductKey == (int)Products.VariFixLoan)
            {
                IApplicationProductVariFixLoan appVF = appML.CurrentProduct as IApplicationProductVariFixLoan;
                IApplicationInformationVarifixLoan aivf = null;
                if (appVF != null)
                    aivf = appVF.VariFixInformation;

                percentFix = _view.FixPercent;
                loanAmountFix = (loanAmount * percentFix);
                loanAmountVar = (loanAmount * (1 - percentFix));

                //instalment VF accounts can not be interest only
                //fix
                instalmentFix = aivf.FixedInstallment;

                //var
                if (loanAmountVar > 0)
                    instalmentVar = (aivl.MonthlyInstalment.HasValue ? aivl.MonthlyInstalment.Value : 0D) - aivf.FixedInstallment;
                else
                    instalmentVar = 0;

                //pti
                ptiFix = pti;
                pti = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanAmount, interestRate, _view.Term, false), _view.HouseholdIncome);

                //interest over term
                //Fix
                interestOverTermFix = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmountFix, interestRateFix, _view.Term, false);

                //Var
                if (loanAmountVar > 0)
                    interestOverTermVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanAmountVar, interestRate, _view.Term, false);
                else
                    interestOverTermVar = 0;
            }

            //Calculate min income against the CC max pti
            minIncome = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateMinimumIncomeRequired(instalmentTotal, cc.PTI.Value);

            // set display of the form
            //Warnings:
            if (minIncome > _view.HouseholdIncome)
                _view.IncomeSufficient = false;

            //ltv pti
            _view.LTV = ltv;
            _view.PTI = pti;
            _view.PTIFix = ptiFix;

            //Percentages
            _view.FixPercent = _view.FixPercent;

            //Amounts
            _view.LoanAmountTotal = loanAmount;
            _view.LoanAmountFix = loanAmountFix;
            _view.LoanAmountVar = loanAmountVar;

            //instalment
            _view.InstalmentVar = instalmentVar;
            _view.InstalmentFix = instalmentFix;
            _view.InstalmentTotal = instalmentVar + instalmentFix;
            _view.InstalmentIOTotal = instalmentIOTotal;
            _view.InstalmentEHLAM = instalmentEHLAM;

            //finance charges
            _view.FinanceChargesVar = interestOverTermVar;
            _view.FinanceChargesFix = interestOverTermFix;
            _view.FinanceChargesIOTotal = interestOverTermIO;
            _view.FinanceChargesTotal = interestOverTermVar + interestOverTermFix;

            //Fees
            _view.CancellationFee = cancelFee;
            _view.RegistrationFee = registrationFee;
            _view.InitiationFee = initiationFee;
            _view.TotalFee = totalFees;
            _view.InterimInterest = interimInterest;

            AppRepo.CreditDisqualifications(true, ltv, pti, _view.HouseholdIncome, loanAmount, _view.EstimatedPropertyValue, _view.EmploymentTypeKey, false, _view.Term, false);
            CheckBusinessRules();

            RunRule(Rules.ApplicationProductEdgeLTVWarning, _app);
			RunRule("CalculatorAlphaHousingLoanMustBeNewVariableLoan", _view.ProductKey, ltv, _view.HouseholdIncome, _view.EmploymentTypeKey);
            RunRule("CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan", _view.ProductKey, ltv, _view.HouseholdIncome, _view.EmploymentTypeKey, _view.InterestOnly);
            if (!_view.IsValid)
                return;

            //if we got here the application qualifies
            _view.ApplicationQualifies = true;
        }

        protected void ApplicationCreateDefaults()
        {
            _os = AppRepo.GetOriginationSource((OriginationSources)OriginationSourceHelper.PrimaryOriginationSourceKey(_view.CurrentPrincipal));
            _resetConfig = AppRepo.GetApplicationDefaultResetConfiguration();
            _statusOpen = LookupRepo.ApplicationStatuses.ObjectDictionary[((int)OfferStatuses.Open).ToString()];

            _marketRateKey = (int)MarketRates.ThreeMonthJIBARRounded;

            if (LookupRepo.EmploymentTypes.ObjectDictionary.ContainsKey(_view.EmploymentTypeKey.ToString()))
                _empType = LookupRepo.EmploymentTypes.ObjectDictionary[_view.EmploymentTypeKey.ToString()];
        }

        protected void ApplicationCreateProductSetup()
        {
            //do the product specific stuff
            switch (_view.ProductKey)
            {
                case (int)Products.NewVariableLoan:
                    SetupNVL(_app.CurrentProduct);
                    break;

                case (int)Products.SuperLo:
                    SetupSuperLo(_app.CurrentProduct);
                    break;

                case (int)Products.VariFixLoan:
                    SetupVariFix(_app.CurrentProduct);
                    break;

                case (int)Products.Edge:
                    SetupEHL(_app.CurrentProduct);
                    break;
                default:
                    break;
            }

            //Mortgage Loan details
            IApplicationMortgageLoan appML = (IApplicationMortgageLoan)_app;
            appML.ApplicationStartDate = DateTime.Now;
            appML.ApplicationStatus = _statusOpen;
            appML.ClientEstimatePropertyValuation = _view.EstimatedPropertyValue;
            appML.ResetConfiguration = _resetConfig;

            appML.CalculateApplicationDetail(false, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnWorkFlowCancelButtonClicked(object sender, EventArgs e)
        {
            IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
            if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                X2Service.LogIn(_view.CurrentPrincipal);

            X2Service.CancelActivity(_view.CurrentPrincipal);

            X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        /// <summary>
        ///
        /// </summary>
        protected void CreateNewApplication()
        {
            if (_view.MarketingSource.Length == 0)
            {
                _view.Messages.Add(new Error("Please specify a marketing source.", "Please specify a marketing source."));
                return;
            }

            ApplicationCreateDefaults();

            //Create application
            switch (_view.MortgageLoanPurpose)
            {
                case MortgageLoanPurposes.Newpurchase:
                    _app = AppRepo.CreateNewPurchaseApplication(_os, (ProductsNewPurchaseAtCreation)_view.ProductKey, null);

                    break;

                case MortgageLoanPurposes.Refinance:
                    _app = AppRepo.CreateRefinanceApplication(_os, (ProductsRefinanceAtCreation)_view.ProductKey, null);
                    IApplicationMortgageLoanRefinance ar = (IApplicationMortgageLoanRefinance)_app;
                    ar.CapitaliseFees = _view.CapitaliseFees;
                    break;

                case MortgageLoanPurposes.Switchloan:
                    _app = AppRepo.CreateSwitchLoanApplication(_os, (ProductsSwitchLoanAtCreation)_view.ProductKey, null);
                    IApplicationMortgageLoanSwitch asw = (IApplicationMortgageLoanSwitch)_app;

                    asw.CapitaliseFees = _view.CapitaliseFees;
                    break;
                default:
                    break;
            }
            _app.ApplicationSource = _appRepo.GetApplicationSourceByKey(Convert.ToInt32(_view.MarketingSource));

            if (_view.IsOldMutualDeveloperLoan)
            {
                IApplicationAttribute applicationAttributeNew = _appRepo.GetEmptyApplicationAttribute();
                applicationAttributeNew.ApplicationAttributeType = _lookupRepo.ApplicationAttributesTypes.ObjectDictionary[((int)OfferAttributeTypes.OldMutualDeveloperLoan).ToString()];
                applicationAttributeNew.Application = _app;
                _app.ApplicationAttributes.Add(_view.Messages, applicationAttributeNew);
            }

            //do the product specific stuff
            ApplicationCreateProductSetup();
            _app.ValidateEntity();
        }

        /// <summary>
        ///
        /// </summary>
        protected void UpdateApplication()
        {
            if (_view.MarketingSource.Length == 0)
            {
                _view.Messages.Add(new Error("Please specify a marketing source.", "Please specify a marketing source."));
                return;
            }

            ApplicationCreateDefaults();

            MortgageLoanPurposes purp = MortgageLoanPurposes.Unknown;

            if (_app is IApplicationMortgageLoanNewPurchase)
                purp = MortgageLoanPurposes.Newpurchase;

            if (_app is IApplicationMortgageLoanSwitch)
                purp = MortgageLoanPurposes.Switchloan;

            if (_app is IApplicationMortgageLoanRefinance)
                purp = MortgageLoanPurposes.Refinance;

            //Create application
            switch (_view.MortgageLoanPurpose)
            {
                case MortgageLoanPurposes.Newpurchase:
                    if (_app is IApplicationUnknown)
                        _app = AppRepo.CreateNewPurchaseApplication(_os, (ProductsNewPurchaseAtCreation)_view.ProductKey, (IApplicationUnknown)_app);

                    if (purp != _view.MortgageLoanPurpose && purp != MortgageLoanPurposes.Unknown)
                        throw new Exception("Application is not Newpurchase, please change to " + purp.ToString() + "in the calculator.");

                    break;

                case MortgageLoanPurposes.Refinance:
                    if (_app is IApplicationUnknown)
                        _app = AppRepo.CreateRefinanceApplication(_os, (ProductsRefinanceAtCreation)_view.ProductKey, (IApplicationUnknown)_app);

                    if (purp != _view.MortgageLoanPurpose && purp != MortgageLoanPurposes.Unknown)
                        throw new Exception("Application is not Refinance, please change to " + purp.ToString() + "in the calculator.");

                    IApplicationMortgageLoanRefinance ar = _app as IApplicationMortgageLoanRefinance;

                    ar.CapitaliseFees = _view.CapitaliseFees;
                    break;

                case MortgageLoanPurposes.Switchloan:
                    if (_app is IApplicationUnknown)
                        _app = AppRepo.CreateSwitchLoanApplication(_os, (ProductsSwitchLoanAtCreation)_view.ProductKey, (IApplicationUnknown)_app);
                    if (purp != _view.MortgageLoanPurpose && purp != MortgageLoanPurposes.Unknown)
                        throw new Exception("Application is not Switchloan, please change to " + purp.ToString() + "in the calculator.");

                    IApplicationMortgageLoanSwitch asw = _app as IApplicationMortgageLoanSwitch;

                    asw.CapitaliseFees = _view.CapitaliseFees;
                    break;
                default:
                    break;
            }
            _app.ApplicationSource = _appRepo.GetApplicationSourceByKey(Convert.ToInt32(_view.MarketingSource));
            
            //do the product specific stuff
            ApplicationCreateProductSetup();
            if (_view.IsOldMutualDeveloperLoan)
            {
                IApplicationAttribute applicationAttributeNew = AppRepo.GetEmptyApplicationAttribute();
                applicationAttributeNew.ApplicationAttributeType = _lookupRepo.ApplicationAttributesTypes.ObjectDictionary[SAHL.Common.Globals.OfferAttributeTypes.DevelopmentLoan.ToString()];
                applicationAttributeNew.Application = _app;
                _app.ApplicationAttributes.Add(_view.Messages, applicationAttributeNew);
            }

            AppRepo.SaveApplication((IApplication)_app);
        }

        protected void X2CompleteNavigate()
        {
            IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
            if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                X2Service.LogIn(_view.CurrentPrincipal);

            Dictionary<string, string> Inputs = new Dictionary<string, string>();
            Inputs.Add("isEstateAgentApplication", _view.IsEstateAgentApplication.ToString());

            X2Service.CompleteActivity(_view.CurrentPrincipal, Inputs, false);

            X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
        }

        protected void SaveAndMoveOn()
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                UpdateApplication(); //persist

                X2CompleteNavigate(); //workflow complete and navigate

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void CheckBusinessRules()
        {
            IControl ctrl = CtrlRepo.GetControlByDescription("Calc - minValuation");
            double minPurchasePrice = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0);
            ctrl = CtrlRepo.GetControlByDescription("FlexiMinFixed");
            double minFixFinServiceAmount = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 50000);
            ctrl = CtrlRepo.GetControlByDescription("FlexiMinLoan");
            double minFixTotalLoanAmount = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 200000);

            if (_view.ProductKey == 0)
                _view.Messages.Add(new Error("No product was selected.", "No product was selected."));

            if (_view.ProductKey > 0)
                RunRule("ApplicationProductMortgageLoanTerm", _view.Term, (Products)_view.ProductKey);

            if ((int)_view.MortgageLoanPurpose == 1)
                _view.Messages.Add(new Error("No loan purpose was selected.", "No loan purpose was selected."));

            if (_view.EmploymentTypeKey == 0)
                _view.Messages.Add(new Error("No employment type was selected.", "No employment type was selected."));

            switch (_view.MortgageLoanPurpose)
            {
                case MortgageLoanPurposes.Switchloan: //2: Switch loan
                    if (_view.CurrentLoan <= 0)
                        RunRule("SwitchCurrentLoanAmountMinimum", null);
                    break;
                default:
                    break;
            }

            if (_view.ProductKey == (int)Products.VariFixLoan)
            {
                if (_view.LoanAmountRequired < minFixTotalLoanAmount)
                    RunRule("VarifixMinimumLoanAmount", minFixTotalLoanAmount);
                if (_view.LoanAmountRequired * _view.FixPercent < minFixFinServiceAmount)
                    RunRule("VarifixMinimumFixAmount", (_view.LoanAmountRequired * _view.FixPercent), minFixFinServiceAmount);
            }
        }

        protected DateTime MaturityDate(DateTime fromDate, int Term)
        {
            DateTime maturity = fromDate.AddMonths(Term + _maturityMonths);

            if (maturity.Day > 15) //get the next month
                maturity.AddMonths(1);

            // return the last of this month
            return new DateTime(maturity.Year, maturity.Month, 1).AddMonths(1).AddDays(-1);
        }

        protected void SetupVariableInformation(IApplicationInformationVariableLoan aivl)
        {
            if (aivl != null)
            {
                aivl.CashDeposit = _view.Deposit;

                aivl.EmploymentType = _empType;
                aivl.ExistingLoan = _view.CurrentLoan; //this should always be 0 for a new loan
                aivl.HouseholdIncome = _view.HouseholdIncome;
                aivl.LoanAmountNoFees = _view.LoanAmountRequired - (_view.CapitaliseFees ? _view.TotalFee : 0D);
                aivl.PropertyValuation = _view.EstimatedPropertyValue;
                aivl.Term = _view.Term;
            }

            if (_view.MortgageLoanPurpose == MortgageLoanPurposes.Switchloan || _view.MortgageLoanPurpose == MortgageLoanPurposes.Refinance)
            {
                IApplicationInformationVariableLoanForSwitchAndRefinance aivlsr = (IApplicationInformationVariableLoanForSwitchAndRefinance)aivl;
                if (aivlsr != null)
                    aivlsr.RequestedCashAmount = _view.CashOut;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aiio"></param>
        protected void SetupInterestOnly(IApplicationInformationInterestOnly aiio, bool isEHL)
        {
            if (_view.InterestOnly || isEHL)
            {
                if (aiio != null)
                {
                    //aiio.Installment = _view.InstalmentTotal;
                    if (isEHL)
                        aiio.MaturityDate = DateTime.Now.AddMonths(36);
                    else
                        aiio.MaturityDate = MaturityDate(DateTime.Now, _view.Term);

                    IApplicationInformationFinancialAdjustment ioro = AppRepo.GetEmptyApplicationInformationFinancialAdjustment();
                    ioro.Discount = 0;

                    if (isEHL)
                        ioro.Term = 36;
                    else
                        ioro.Term = -1;

                    if (isEHL)
                    {
                        ioro.FinancialAdjustmentTypeSource = FinancialAdjustmentRepository.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.Edge);
                    }
                    else
                    {
                        ioro.FinancialAdjustmentTypeSource = FinancialAdjustmentRepository.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.InterestOnly);
                    }

                    ioro.ApplicationInformation = aiio.ApplicationInformation;
                    aiio.ApplicationInformation.ApplicationInformationFinancialAdjustments.Add(_view.Messages, ioro);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        protected IApplicationProductNewVariableLoan SetupNVL(IApplicationProduct prod)
        {
            IApplicationProductNewVariableLoan nvl = (IApplicationProductNewVariableLoan)prod;

            IApplicationMortgageLoanNewPurchase npml = nvl.Application as IApplicationMortgageLoanNewPurchase;
            if (npml != null)
            {
                npml.PurchasePrice = _view.PurchasePrice;
            }

            nvl.LoanAmountNoFees = _view.LoanAmountRequired;
            if (_view.CapitaliseFees)
                nvl.LoanAmountNoFees -= _view.TotalFee;

            nvl.Term = _view.Term;

            SetupVariableInformation(nvl.VariableLoanInformation);
            SetupInterestOnly(nvl.InterestOnlyInformation, false);
            return nvl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        protected IApplicationProductEdge SetupEHL(IApplicationProduct prod)
        {
            IApplicationProductEdge ehl = (IApplicationProductEdge)prod;

            IApplicationMortgageLoanNewPurchase npml = ehl.Application as IApplicationMortgageLoanNewPurchase;
            if (npml != null)
            {
                npml.PurchasePrice = _view.PurchasePrice;
            }

            ehl.LoanAmountNoFees = _view.LoanAmountRequired;
            if (_view.CapitaliseFees)
                ehl.LoanAmountNoFees -= _view.TotalFee;

            SetupVariableInformation(ehl.VariableLoanInformation);
            ehl.Term = _view.EdgeTerm;
            SetupInterestOnly(ehl.InterestOnlyInformation, true);
            return ehl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        protected IApplicationProductSuperLoLoan SetupSuperLo(IApplicationProduct prod)
        {
            IApplicationProductSuperLoLoan sl = (IApplicationProductSuperLoLoan)prod;

            IApplicationMortgageLoanNewPurchase npml = sl.Application as IApplicationMortgageLoanNewPurchase;
            if (npml != null)
            {
                npml.PurchasePrice = _view.PurchasePrice;
            }

            sl.LoanAmountNoFees = _view.LoanAmountRequired;
            if (_view.CapitaliseFees)
                sl.LoanAmountNoFees -= _view.TotalFee;

            sl.Term = _view.Term;

            SetupVariableInformation(sl.VariableLoanInformation);
            SetupInterestOnly(sl.InterestOnlyInformation, false);

            IApplicationInformationSuperLoLoan sli = sl.SuperLoInformation;
            double annualThreshold = _view.LoanAmountRequired * 0.04;
            sli.PPThresholdYr1 = annualThreshold;
            sli.PPThresholdYr2 = annualThreshold;
            sli.PPThresholdYr3 = annualThreshold;
            sli.PPThresholdYr4 = annualThreshold;
            sli.PPThresholdYr5 = annualThreshold;
            sli.ElectionDate = DateTime.Now;

            IApplicationInformationFinancialAdjustment slro = AppRepo.GetEmptyApplicationInformationFinancialAdjustment();

            IMarginProduct mp = CMRepo.GetMarginProductByRateConfigAndOSP(_rateConfigKey, _os.Key, _view.ProductKey);
            if (mp != null)
                slro.Discount = mp.Discount;
            else
                slro.Discount = 0;

            slro.Term = -1;

            //TODO : Backend Revamp, Get answer from Gennine as to whether we care about refactoring SuperLo
            slro.FinancialAdjustmentTypeSource = FinancialAdjustmentRepository.GetFinancialAdjustmentTypeSourceByKey((int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.SuperLo);

            slro.ApplicationInformation = sli.ApplicationInformation;
            sli.ApplicationInformation.ApplicationInformationFinancialAdjustments.Add(_view.Messages, slro);

            return sl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        protected IApplicationProductVariFixLoan SetupVariFix(IApplicationProduct prod)
        {
            IApplicationProductVariFixLoan vfl = (IApplicationProductVariFixLoan)prod;

            IApplicationMortgageLoanNewPurchase npml = vfl.Application as IApplicationMortgageLoanNewPurchase;
            if (npml != null)
            {
                npml.PurchasePrice = _view.PurchasePrice;
            }

            //vfl.LoanAgreementAmount = _view.LoanAmountRequired;
            vfl.LoanAmountNoFees = _view.LoanAmountRequired;
            if (_view.CapitaliseFees)
                vfl.LoanAmountNoFees -= _view.TotalFee;

            vfl.Term = _view.Term;

            SetupVariableInformation(vfl.VariableLoanInformation);

            IApplicationInformationVarifixLoan aivf = vfl.VariFixInformation;

            //aivf.FixedInstallment = _view.InstalmentFix;
            aivf.FixedPercent = _view.FixPercent;
            aivf.ElectionDate = DateTime.Now;
            aivf.MarketRate = LookupRepo.MarketRates.ObjectDictionary[_view.VarifixMarketRateKey.ToString()];

            return vfl;
        }

        private void RunRule(string Rule, params object[] prms)
        {
            RuleServ.ExecuteRule(_view.Messages, Rule, prms);
        }

        protected ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
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

        protected IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctrlRepo;
            }
        }

        protected ICreditMatrixRepository CMRepo
        {
            get
            {
                if (_cmRepo == null)
                    _cmRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

                return _cmRepo;
            }
        }

        protected IX2Repository X2Repo
        {
            get
            {
                if (_x2Repo == null)
                    _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

                return _x2Repo;
            }
        }

        private IRuleService RuleServ
        {
            get
            {
                if (_ruleServ == null)
                    _ruleServ = ServiceFactory.GetService<IRuleService>();

                return _ruleServ;
            }
        }

        protected IFinancialAdjustmentRepository FinancialAdjustmentRepository
        {
            get
            {
                if (_financialAdjustmentRepository == null)
                    _financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();

                return _financialAdjustmentRepository;
            }
        }
    }
}