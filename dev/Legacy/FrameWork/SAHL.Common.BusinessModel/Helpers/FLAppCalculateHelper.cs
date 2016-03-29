using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel.Helpers
{
    public class FLAppCalculateHelper
    {
        #region BMObjects

        private IDomainMessageCollection _dmc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent()).DomainMessages;

        private IAccount _acc;
        private IRateConfiguration _rateConfiguration;
        private IMortgageLoanAccount _mla;
        private IMortgageLoan _vML;
        private IMortgageLoan _fML;
        private IApplicationReAdvance _ra;
        private IApplicationFurtherAdvance _fa;
        private IApplicationFurtherLoan _fl;

        #region Repos & Svc's

        private ILookupRepository _lookupRepo;

        private ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private ICreditCriteriaRepository creditCriteriaRepository;
        private IFinancialsService _finsService;
        private ISPVService _spvService;
        private ICreditMatrixRepository _creditMatrixRepository;

        private IFinancialsService FinsService
        {
            get
            {
                if (_finsService == null)
                    _finsService = ServiceFactory.GetService<IFinancialsService>();

                return _finsService;
            }
        }

        private ICreditCriteriaRepository CreditCriteriaRepository
        {
            get
            {
                if (creditCriteriaRepository == null)
                    creditCriteriaRepository = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();

                return creditCriteriaRepository;
            }
        }

        private ISPVService SPVService
        {
            get
            {
                if (_spvService == null)
                    _spvService = ServiceFactory.GetService<ISPVService>();

                return _spvService;
            }
        }

        private ICreditMatrixRepository CreditMatrixRepository
        {
            get
            {
                if (_creditMatrixRepository == null)
                    _creditMatrixRepository = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

                return _creditMatrixRepository;
            }
        }

        #endregion Repos & Svc's

        #endregion BMObjects

        #region AccountValues

        private double _accCurrBalance;
        private int _term;
        private double _vMarketRate;
        private double _accMargin;
        private double _loanAgreeAmount;
        private double _accruedInterest;

        private bool _isInterestOnly;
        private double _latestValuationAmount;
        private double _discount;
        private int _osKey;
        private int _prodKey;
        private int _resetKey;
        private int _accSPVKey;
        private bool _ncaCompliant = true;

        //fix vals
        private double _accVFBalance;

        private double _baseRateF;
        private double _discountF;

        #endregion AccountValues

        #region ApplicationValues

        private double _raAmount;
        private IApplicationInformationVariableLoan _raVLI;
        private double _faAmount;
        private IApplicationInformationVariableLoan _faVLI;
        private double _flAmount;
        private IApplicationInformationVariableLoan _flVLI;
        private double _margin;
        private double _rate;
        private double _householdIncome;

        #endregion ApplicationValues

        #region Properties

        private ISPV _spv;

        public ISPV SPV
        {
            get { return _spv; }
            set { _spv = value; }
        }

        private bool _isExceptionCC;

        public bool IsExceptionCreditCriteria
        {
            get { return _isExceptionCC; }
            set { _isExceptionCC = value; }
        }

        private double _pti;

        public double PTI
        {
            get { return _pti; }
            set { _pti = value; }
        }

        private double _ltv;

        public double LTV
        {
            get { return _ltv; }
            set { _ltv = value; }
        }

        private double _amInstal;

        public double AmortisingInstalment
        {
            get { return _amInstal; }
            set { _amInstal = value; }
        }

        private double _Instal;

        public double Instalment
        {
            get { return _Instal; }
            set { _Instal = value; }
        }

        private ICategory _category;

        public ICategory AppCategory
        {
            get { return _category; }
            set { _category = value; }
        }

        private double _fees;

        public double Fees
        {
            get { return _fees; }
            set { _fees = value; }
        }

        public double ApplicationLoanAmount { get; set; }

        public double CalculatedLinkRate { get; set; }

        public ICalculated20YearLoanDetailsFor30YearTermLoan Calculated20YearLoanDetailsFor30YearTermLoan { get; set; }

        #endregion Properties

        public FLAppCalculateHelper(IAccount acc)
        {
            _acc = acc;

            foreach (IApplication app in _acc.Applications)
            {
                if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                {
                    switch (app.ApplicationType.Key)
                    {
                        case (int)OfferTypes.ReAdvance:
                            _ra = app as IApplicationReAdvance;
                            break;

                        case (int)OfferTypes.FurtherAdvance:
                            _fa = app as IApplicationFurtherAdvance;
                            break;

                        case (int)OfferTypes.FurtherLoan:
                            _fl = app as IApplicationFurtherLoan;
                            break;

                        default:
                            break;
                    }
                }
            }

            CommonSetup(_acc);
        }

        /// <summary>
        /// Constructor for the Calculator
        /// Each application will need to be passed in
        /// because each application could have amended values
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="ra"></param>
        /// <param name="fa"></param>
        /// <param name="fl"></param>
        public FLAppCalculateHelper(IAccount acc, IApplicationReAdvance ra, IApplicationFurtherAdvance fa, IApplicationFurtherLoan fl)
        {
            _acc = acc;
            _ra = ra;
            _fa = fa;
            _fl = fl;

            CommonSetup(_acc);
        }

        private void CommonSetup(IAccount acc)
        {
            //setup all the common stuff here to use later
            _mla = _acc as IMortgageLoanAccount;

            //total lending amount for all offers
            if (_ra != null)
            {
                ISupportsVariableLoanApplicationInformation svli = _ra.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                _raVLI = svli.VariableLoanInformation;
                _raAmount = _raVLI.LoanAmountNoFees.HasValue ? _raVLI.LoanAmountNoFees.Value : 0;
            }
            if (_fa != null)
            {
                ISupportsVariableLoanApplicationInformation svli = _fa.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                _faVLI = svli.VariableLoanInformation;
                _faAmount = _faVLI.LoanAmountNoFees.HasValue ? _faVLI.LoanAmountNoFees.Value : 0;
            }
            if (_fl != null)
            {
                ISupportsVariableLoanApplicationInformation svli = _fl.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                _flVLI = svli.VariableLoanInformation;
                _flAmount = _flVLI.LoanAmountNoFees.HasValue ? _flVLI.LoanAmountNoFees.Value : 0;
            }

            _vML = _mla.SecuredMortgageLoan;
            _rateConfiguration = _vML.RateConfiguration;
            _accCurrBalance = _vML.CurrentBalance;
            _term = _vML.RemainingInstallments;
            _loanAgreeAmount = _vML.SumBondLoanAgreementAmounts();
            _accruedInterest = (_vML.AccruedInterestMTD.HasValue ? _vML.AccruedInterestMTD.Value : 0);

            _vMarketRate = _vML.ActiveMarketRate;
            _isInterestOnly = _vML.HasInterestOnly();
            _osKey = acc.OriginationSource.Key;
            _prodKey = acc.Product.Key;
            _resetKey = _vML.ResetConfiguration.Key;
            _accMargin = _vML.RateConfiguration.Margin.Value;
            _accSPVKey = _vML.Account.SPV.Key;

            // For the Edge Product we should calculate PTI based on the Armotising Term
            // therefore we recalculate the remaining term at this point
            if (_prodKey == (int)Products.Edge)
            {
                int termEdge = _vML.RemainingInstallments;
                IFinancialAdjustment intOnlyFinancialAdjustment = null;
                int intOnlyRemainderMonths = 0;

                foreach (IFinancialAdjustment fa in _vML.FinancialAdjustments)
                {
                    if (fa.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.Edge
                        && fa.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active)
                    {
                        intOnlyFinancialAdjustment = fa;
                        break;
                    }
                }

                if (intOnlyFinancialAdjustment != null)
                {
                    System.DateTime dtStart = DateTime.Now;
                    System.DateTime dtEndFinancialAdjustment = intOnlyFinancialAdjustment.EndDate.HasValue ? intOnlyFinancialAdjustment.EndDate.Value : dtStart;
                    int intOnlyRemainderYears = (dtEndFinancialAdjustment.Year - dtStart.Year) * 12;
                    intOnlyRemainderMonths = dtEndFinancialAdjustment.Month - dtStart.Month;
                    intOnlyRemainderMonths += intOnlyRemainderYears;
                    termEdge -= intOnlyRemainderMonths;
                }
                _term = termEdge;
            }

            foreach (IAccountInformation ai in _acc.AccountInformations)
            {
                if (ai.AccountInformationType.Key == (int)AccountInformationTypes.NotNCACompliant)
                {
                    _ncaCompliant = false;
                    break;
                }
            }

            //Rate discounts apply to all FS's
            _discount = _vML.RateAdjustment; // _vML.Discount is now _vML.RateAdjustment --> _vML.LoanBalance.Balance.RateAdjustment
            _latestValuationAmount = _vML.GetActiveValuationAmount();

            //Get the Fixed ML
            if ((_acc as IAccountVariFixLoan) != null)
            {
                IAccountVariFixLoan _fAccount = _acc as IAccountVariFixLoan;
                _fML = _fAccount.FixedSecuredMortgageLoan;
                _accVFBalance = _fML.CurrentBalance;
                _accCurrBalance += _fML.CurrentBalance;
                _loanAgreeAmount += _fML.SumBondLoanAgreementAmounts();
                _accruedInterest += (_fML.AccruedInterestMTD.HasValue ? _fML.AccruedInterestMTD.Value : 0);

                _baseRateF = _fML.ActiveMarketRate;
                _discountF = _fML.RateAdjustment;
            }
        }

        /// <summary>
        /// Calculate further lending for all applications
        /// </summary>
        public void CalculateFurtherLending(bool recalcAllOffers)
        {
            //for a proper common recalc this method should not recalc if the
            //application GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer
            //additionally the application values need to be set up if the application information is accepted

            // setup and calculate

            double valuation = _latestValuationAmount;
            if (_ra != null)
            {
                if (_ra.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                {
                    PopulateApplicationData(_ra.CurrentProduct, _raVLI, OfferTypes.ReAdvance);
                    //Get the biggest valuation amount
                    if (_ra.ClientEstimatePropertyValuation.HasValue && _ra.ClientEstimatePropertyValuation.Value > valuation)
                        valuation = _ra.ClientEstimatePropertyValuation.Value;
                    Recalc(_raVLI, _ra, valuation, OfferTypes.ReAdvance, _ra.HasAttribute(OfferAttributeTypes.QuickPayLoan));
                }
            }
            if (_fa != null)
            {
                if (_fa.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                {
                    PopulateApplicationData(_fa.CurrentProduct, _faVLI, OfferTypes.FurtherAdvance);

                    //Get the biggest valuation amount
                    if (_fa.ClientEstimatePropertyValuation.HasValue && _fa.ClientEstimatePropertyValuation.Value > valuation)
                        valuation = _fa.ClientEstimatePropertyValuation.Value;
                    Recalc(_faVLI, _fa, valuation, OfferTypes.FurtherAdvance, _fa.HasAttribute(OfferAttributeTypes.QuickPayLoan));
                }
            }
            if (_fl != null)
            {
                if (_fl.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                {
                    PopulateApplicationData(_fl.CurrentProduct, _flVLI, OfferTypes.FurtherLoan);
                    //Get the biggest valuation amount
                    if (_fl.ClientEstimatePropertyValuation.HasValue && _fl.ClientEstimatePropertyValuation.Value > valuation)
                        valuation = _fl.ClientEstimatePropertyValuation.Value;
                    Recalc(_flVLI, _fl, valuation, OfferTypes.FurtherLoan, _fl.HasAttribute(OfferAttributeTypes.QuickPayLoan));
                }
            }

            // Determine the SPV based on the total application amount (RA + FA + FL + (if any fees applicable) + accrued interest
            SPV = SPVService.DetermineSPVForFurtherLending(_acc.Key, ApplicationLoanAmount + _accruedInterest, valuation);

            if (recalcAllOffers)
            {
                //Lastly do some combined stuff for total of all FL
                double ttlDisbursedAmount = _accCurrBalance + _raAmount + _faAmount + _flAmount;
                LTV = ttlDisbursedAmount / valuation;

                //Get new Amortising instalment
                AmortisingInstalment = LoanCalculator.CalculateFurtherLendingInstallment(ttlDisbursedAmount, _rate, _term, _accVFBalance, (_margin + _baseRateF + _discountF), false);

                //PTI is always calculated against amortising installment
                PTI = AmortisingInstalment / _householdIncome;

                // Calculate the Interest Only instalment if the account is Interest Only
                if (_isInterestOnly)
                    Instalment = LoanCalculator.CalculateFurtherLendingInstallment(ttlDisbursedAmount, _rate, _term, _accVFBalance, (_margin + _baseRateF + _discountF), true);
                else
                    Instalment = AmortisingInstalment;

                if (_acc.IsThirtyYearTerm)
                {
                    double thirtyYearPricingAdjustment = 0;

                    IEnumerable<IFinancialAdjustment> financialAdjustment30YearTerm = _vML.FinancialAdjustments.Where(x => x.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.Loanwith30YearTerm &&
                        x.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active);

                    if (financialAdjustment30YearTerm.Count() > 0)
                    {
                        thirtyYearPricingAdjustment = financialAdjustment30YearTerm.Sum(x => x.InterestRateAdjustment.Adjustment);
                    }

                    Calculated20YearLoanDetailsFor30YearTermLoan = LoanCalculator.Calculate20YearLoanDetailsFor30YearTermLoan(_vML.RateAdjustment, thirtyYearPricingAdjustment, ttlDisbursedAmount, _rate, 240, _householdIncome);
                }
            }
        }

        private void Recalc(IApplicationInformationVariableLoan vli, IApplication furtherLendingApplication, double valAmount, OfferTypes appType, bool IsQuickPayLoan)
        {
            double appltv = 0;
            double appInstalment = 0;
            double apppti = 0;
            double appLoanAmount = 0;
            double applicationAmount = vli.LoanAmountNoFees.HasValue ? vli.LoanAmountNoFees.Value : 0;
            //set values up that we will need to use later
            var rateConfiguration = vli.RateConfiguration;
            _margin = vli.RateConfiguration.Margin.Value;
            _rate = (vli.RateConfiguration.Margin.Value + _vMarketRate + _discount);
            _householdIncome = vli.HouseholdIncome.HasValue ? vli.HouseholdIncome.Value : 1D; //default to R1 just in case

            if (appType == OfferTypes.FurtherLoan)
            {
                double initiationFee;
                double registrationFee;
                double cancelFee;
                double interimInterest;
                double bondToRegister;
                double? initiationFeeDiscount;

                if (applicationAmount > 0)
                {
                    double appBTR = vli.BondToRegister.HasValue ? vli.BondToRegister.Value : 0;

                    if (applicationAmount > 0 && appBTR == 0)
                        appBTR = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(applicationAmount);
                    ApplicationCalculateMortgageLoanHelper.CalculateOriginationFees(applicationAmount, appBTR, OfferTypes.FurtherLoan, 0, 0, true, _ncaCompliant, true, false, out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, IsQuickPayLoan, _householdIncome, vli.EmploymentType.Key, valAmount, _fl.Account.Key, _fl.Account.Details.Any(x => x.DetailType.Key == (int)DetailTypes.StaffHomeLoan), DateTime.Now, false, _fl.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));
                    Fees = initiationFee + registrationFee + cancelFee + interimInterest;
                    //The fees must be calculated and added to the further lending amount before calcs etc
                    applicationAmount += Fees;
                    vli.FeesTotal = Fees;
                    //this is ugly here, could do this better...
                    _fl.SetInitiationFee(initiationFee, false);
                    _fl.SetRegistrationFee(registrationFee, false);
                    vli.BondToRegister = bondToRegister;
                }
            }

            switch (appType)
            {
                case OfferTypes.FurtherAdvance:
                    _faAmount = applicationAmount;
                    appLoanAmount = _accCurrBalance + _faAmount + _raAmount;
                    break;

                case OfferTypes.FurtherLoan:
                    _flAmount = applicationAmount; //this includes fees
                    appLoanAmount = _accCurrBalance + _flAmount + _faAmount + _raAmount;
                    break;

                case OfferTypes.ReAdvance:
                    _raAmount = applicationAmount;
                    appLoanAmount = _accCurrBalance + _raAmount;
                    break;

                default:
                    throw new NotSupportedException("The application type " + appType.ToString() + " is not supported.");
            }

            ApplicationLoanAmount = appLoanAmount;

            var isAlphaHousing = _acc.Details.Any(x => x.DetailType.Key == (int)DetailTypes.AlphaHousing);
            ICreditCriteria cc = GetCreditCriteria(isAlphaHousing, appLoanAmount, valAmount, vli.EmploymentType.Key, _householdIncome);

            var canBeRecategorized = AllowedInCategory(isAlphaHousing,
                                                       _acc.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan).Balance.LoanBalance.RateConfiguration.Margin.Value,
                                                       cc.Margin.Value);

            if (cc != null)
            {
                AppCategory = cc.Category;
                vli.Category = cc.Category;
                vli.CreditCriteria = cc;
                IsExceptionCreditCriteria = cc.ExceptionCriteria.HasValue ? cc.ExceptionCriteria.Value : false;

                //if the loan results in a breach of the LAA, use the new cc.Margin (if it is > than the current margin)
                var disbursedTotal = _accCurrBalance + _accruedInterest + _raAmount + _faAmount + _flAmount;
                if (disbursedTotal > _loanAgreeAmount && cc.Margin.Value > _accMargin)
                {
                    //Get the Rate Config from the new margin and accounts market rate
                    vli.RateConfiguration = CreditMatrixRepository.GetRateConfigurationByMarginKeyAndMarketRateKey(cc.Margin.Key, _rateConfiguration.MarketRate.Key);
                    vli.CreditMatrix = cc.CreditMatrix;
                }
                else 
                {
                    IMortgageLoanAccount mortgageLoanAcc = furtherLendingApplication.Account as IMortgageLoanAccount;
                    vli.CreditMatrix = mortgageLoanAcc.SecuredMortgageLoan.CreditMatrix;
                }
                CalculatedLinkRate = vli.RateConfiguration.Margin.Value;
                _rate = CalculatedLinkRate + _vMarketRate + _discount;
            }

            if (isAlphaHousing && !canBeRecategorized)
            {
                var categoryOfLoan = _acc.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open }).First().Category;
                AppCategory = categoryOfLoan;
                vli.Category = categoryOfLoan;
            }

            // Do calcs
            //First calc LTV for other calcs
            appltv = applicationAmount / valAmount;
            //Get new Amortising instalment
            if (applicationAmount > 0)
                appInstalment = LoanCalculator.CalculateFurtherLendingInstallment(applicationAmount, _rate, _term, 0, 0, false);
            //PTI is always calculated against amortising installment
            apppti = appInstalment / _householdIncome;
            // Calculate the Interest Only instalment if the account is Interest Only
            if (_isInterestOnly)
                appInstalment = LoanCalculator.CalculateFurtherLendingInstallment(applicationAmount, _rate, _term, 0, 0, true);

            vli.LTV = appltv;
            vli.PTI = apppti;
            vli.MonthlyInstalment = appInstalment;
        }

        private bool AllowedInCategory(bool isAlphaHousing, double existingLinkRate, double newLinkRate)
        {
            if (isAlphaHousing)
            {
                return existingLinkRate <= newLinkRate;
            }

            return true;
        }

        private void PopulateApplicationData(IApplicationProduct cp, IApplicationInformationVariableLoan vli, OfferTypes appType)
        {
            // calculate ltv, pti, instalment, and fees
            ISupportsVariableLoanApplicationInformation oldSvli = _mla.CurrentMortgageLoanApplication.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            // all this stuff could have changed, needs to be reset for recalc
            vli.MarketRate = _vMarketRate;
            vli.Term = _term;
            vli.ExistingLoan = _accCurrBalance; //Account Current Balance less Accrued Interest
            vli.PropertyValuation = _latestValuationAmount;

            //RequestedCashAmount copy across
            IApplicationMortgageLoanWithCashOut appMLWithCashOut = cp.Application as IApplicationMortgageLoanWithCashOut;
            if (appMLWithCashOut != null)
                appMLWithCashOut.RequestedCashAmount = vli.LoanAmountNoFees;
        }

        private ICreditCriteria GetCreditCriteria(bool isAlphaHousing, double loanAmount, double valAmount, int employmentTypeKey, double income)
        {
            ICreditCriteria cc = FinsService.GetCreditCriteriaByLTVAndIncome(_dmc, _osKey, _prodKey, (int)MortgageLoanPurposes.Switchloan, employmentTypeKey, loanAmount, valAmount, income, isAlphaHousing ? CreditCriteriaAttributeTypes.FurtherLendingAlphaHousing : CreditCriteriaAttributeTypes.FurtherLendingNonAlphaHousing);

            //Lets get an exception CC if we still have nothing
            if (cc == null && !isAlphaHousing)
            {
                //we could have gotten here because the Employment type is rubbish..., use Salaried if EmploymentType is unknown
                if (employmentTypeKey == (int)EmploymentTypes.Unknown)
                    cc = FinsService.GetCreditCriteriaException(_dmc, _osKey, _prodKey, (int)MortgageLoanPurposes.Switchloan, (int)EmploymentTypes.Salaried, loanAmount);
                else
                    cc = FinsService.GetCreditCriteriaException(_dmc, _osKey, _prodKey, (int)MortgageLoanPurposes.Switchloan, employmentTypeKey, loanAmount);
            }

            if (cc != null)
                return cc;

            return null;
        }
    }
}