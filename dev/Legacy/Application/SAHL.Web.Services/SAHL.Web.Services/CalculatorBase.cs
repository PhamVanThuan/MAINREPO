using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace SAHL.Web.Services
{
    public class CalculatorBase
    {
        private ILegalEntity _legalEntity;

        protected ILegalEntity LegalEntity
        {
            get { return _legalEntity; }
            set { _legalEntity = value; }
        }

        public ICategory category;
        public ICreditMatrix creditMatrix;

        private IMemoRepository memorepository;

        ///<summary>
        ///</summary>
        public IMemoRepository MemoRepository
        {
            get
            {
                if (memorepository == null)
                    memorepository = RepositoryFactory.GetRepository<IMemoRepository>();
                return memorepository;
            }
        }

        private IEmploymentRepository employmentrepository;

        ///<summary>
        ///</summary>
        public IEmploymentRepository EmploymentRepository
        {
            get
            {
                if (employmentrepository == null)
                    employmentrepository = RepositoryFactory.GetRepository<IEmploymentRepository>();
                return employmentrepository;
            }
        }

        private ILegalEntityRepository legalentityrepository;

        ///<summary>
        ///</summary>
        public ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (legalentityrepository == null)
                    legalentityrepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                return legalentityrepository;
            }
        }

        private ILookupRepository lookuprepository;

        ///<summary>
        ///</summary>
        public ILookupRepository LookupRepository
        {
            get
            {
                if (lookuprepository == null)
                    lookuprepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return lookuprepository;
            }
        }

        private IControlRepository controlrepository;

        ///<summary>
        ///</summary>
        public IControlRepository ControlRepository
        {
            get
            {
                if (controlrepository == null)
                    controlrepository = RepositoryFactory.GetRepository<IControlRepository>();
                return controlrepository;
            }
        }

        private ICreditMatrixRepository creditMatrixRepository;

        ///<summary>
        ///</summary>
        public ICreditMatrixRepository CreditMatrixRepository
        {
            get
            {
                if (creditMatrixRepository == null)
                    creditMatrixRepository = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

                return creditMatrixRepository;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetControlValueByDescription(string controlDescription)
        {
            try
            {
                IControl ctrl = GetControlByDescription(controlDescription);
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        // Set column values.
        // This initises the Preprospect Object with a value set for storing the object/data in the preprpspect table
        // Many of the values are assumed for the sake of capturing the lead - but these will all be verified by the sales sonsultants

        // New Origination Calculator Variables
        //------------------------------------------

        // OLD new

        #region CALCULATION METHODS

        //--------------------------------------------------------------------------------------------
        // CALCULATION METHODS

        /// <summary>
        /// Returns the present value of an investment.
        /// The present value is the total amount that a series of future payments is worth now. For example, when you borrow money, the loan amount is the present value to the lender
        /// </summary>
        /// <param name="dInstallment"></param>
        /// <param name="iTerm"></param>
        /// <param name="dLTVRate"></param>
        /// <returns></returns>
        public static double CalculatePresentValue(double dInstallment, int iTerm, double dLTVRate)
        {
            try
            {
                return (dInstallment / ((dLTVRate) / 12)) * (1 - (1 / Math.Pow(1 + (dLTVRate / 12), iTerm)));
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the Loan to Value ratio
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="homevalue"></param>
        /// <returns></returns>
        public static double CalculateLTV(double loanamount, double homevalue)
        {
            try
            {
                return Math.Round((loanamount / homevalue) * 100);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the installment valu via the Business Model
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="interestrate"></param>
        /// <param name="Term"></param>
        /// <param name="InterestOnly"></param>
        /// <returns></returns>
        public static double CalculateInstallment(double loanamount, double interestrate, double Term, bool InterestOnly)
        {
            try
            {
                double installment = Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanamount, interestrate, Term, InterestOnly);
                return installment;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the installment valu via the Business Model
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="interestrate"></param>
        /// <param name="Term"></param>
        /// <param name="InterestOnly"></param>
        /// <returns></returns>
        public static double CalculateInterestOverTerm(double loanamount, double interestrate, double Term, bool InterestOnly)
        {
            try
            {
                double interest = Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);
                return interest;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the Payment to income ratio
        /// </summary>
        /// <param name="installment"></param>
        /// <param name="earnings"></param>
        /// <returns></returns>
        public static double CalculatePTI(double installment, double earnings)
        {
            try
            {
                return Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(installment, earnings);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static CreditDisqualifications CreditDisqualifications(CreditDisqualifications cd)
        {
            try
            {
                bool calculationdone = cd.calculationdone;
                double ltv = cd.ltv;
                double pti = cd.pti;
                double householdincome = cd.householdincome;
                double loanamountrequired = cd.loanamountrequired;
                double estimatedproeprtyvalue = cd.estimatedproeprtyvalue;
                int EmploymentTypeKey = cd.EmploymentTypeKey;
                bool ifFurtherLending = cd.ifFurtherLending;
                int term = cd.term;

                IApplicationRepository apprep = RepositoryFactory.GetRepository<IApplicationRepository>();
                apprep.CreditDisqualifications(calculationdone, ltv, pti, householdincome, loanamountrequired,
                    estimatedproeprtyvalue, EmploymentTypeKey, ifFurtherLending, term, false);

                cd.calculationdone = calculationdone;
                cd.ltv = ltv;
                cd.pti = pti;
                cd.householdincome = householdincome;
                cd.loanamountrequired = loanamountrequired;
                cd.estimatedproeprtyvalue = estimatedproeprtyvalue;
                cd.EmploymentTypeKey = EmploymentTypeKey;
                cd.ifFurtherLending = ifFurtherLending;
                cd.term = term;

                return cd;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        public static double ReturnBaseRate()
        {
            try
            {
                ILookupRepository lr = RepositoryFactory.GetRepository<ILookupRepository>();
                IEventList<IMarketRate> mRates = lr.MarketRates;
                foreach (IMarketRate mR in mRates)
                {
                    if (mR.Key == (int)MarketRates.ThreeMonthJIBARRounded) //18th reset rate
                        return mR.Value;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        public static double ReturnBaseRateFixed()
        {
            try
            {
                ILookupRepository lr = RepositoryFactory.GetRepository<ILookupRepository>();
                IEventList<IMarketRate> mRates = lr.MarketRates;
                foreach (IMarketRate mR in mRates)
                {
                    if (mR.Key == (int)MarketRates.FiveYearResetFixedMortgageRate) //VF 5year reset rate
                        return mR.Value;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fees"></param>
        /// <returns></returns>
        [Obsolete("CalculateOriginationFees has been made obsolete, please use CalculateFees")]
        public static OriginationFees CalculateOriginationFees(OriginationFees fees)
        {
            fees.propertyValue = 1;
            fees.householdIncome = 1;
            fees.employmentTypeKey = 1;
            return CalculateFees(fees);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fees"></param>
        /// <returns></returns>
        public static OriginationFees CalculateFees(OriginationFees fees)
        {
            try
            {
                IApplicationRepository apprep = RepositoryFactory.GetRepository<IApplicationRepository>();

                double loanamount = fees.loanamount;
                double bondrequired = fees.bondrequired;

                OfferTypes appType;
                switch (fees.appType)
                {
                    case (int)OfferTypes.FurtherAdvance:
                        appType = OfferTypes.FurtherAdvance;
                        break;

                    case (int)OfferTypes.FurtherLoan:
                        appType = OfferTypes.FurtherLoan;
                        break;

                    case (int)OfferTypes.Life:
                        appType = OfferTypes.Life;
                        break;

                    case (int)OfferTypes.NewPurchaseLoan:
                        appType = OfferTypes.NewPurchaseLoan;
                        break;

                    case (int)OfferTypes.ReAdvance:
                        appType = OfferTypes.ReAdvance;
                        break;

                    case (int)OfferTypes.SwitchLoan:
                        appType = OfferTypes.SwitchLoan;
                        break;

                    default:
                        appType = OfferTypes.Unknown;
                        break;
                }

                double cashout = fees.cashout;
                double OverrideCancelFeeamount = fees.OverrideCancelFeeamount;
                bool capitalisefees = fees.capitalisefees;
                double propertyValue = fees.propertyValue;
                double householdIncome = fees.householdIncome;
                int employmentTypeKey = fees.employmentTypeKey;
                double initiationfee;
                double registrationfee;
                double cancellationfee;
                double interiminterest;
                double bondtoregister;
                double? InitiationFeeDiscount = null;

                apprep.CalculateOriginationFees(loanamount, bondrequired, appType, cashout, OverrideCancelFeeamount, capitalisefees, false, false, false, out InitiationFeeDiscount, out initiationfee, out registrationfee, out cancellationfee, out interiminterest, out bondtoregister, false, householdIncome, employmentTypeKey, propertyValue, 0, false, DateTime.Now, false, false);

                fees.initiationfee = initiationfee;
                fees.registrationfee = registrationfee;
                fees.cancellationfee = cancellationfee;
                fees.interiminterest = interiminterest;
                fees.bondtoregister = bondtoregister;

                return fees;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Returns the Bond Amount
        /// </summary>
        /// <param name="LoanAmount"></param>
        /// <returns></returns>
        public static double CalculateBondAmount(double LoanAmount)
        {
            try
            {
                return Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(LoanAmount);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Returns the minimum income required
        /// </summary>
        /// <param name="instalmentTotal"></param>
        /// <param name="PTI"></param>
        /// <returns></returns>
        public static double CalculateMinimumIncomeRequired(double instalmentTotal, double PTI)
        {
            try
            {
                return Common.BusinessModel.Helpers.LoanCalculator.CalculateMinimumIncomeRequired(instalmentTotal, PTI);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static IControl GetControlByDescription(string description)
        {
            try
            {
                IControlRepository controlrep = RepositoryFactory.GetRepository<IControlRepository>();
                return controlrep.GetControlByDescription(description);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMinimumLTV()
        {
            try
            {
                IControl ctrl = GetControlByDescription("Calc - webMaxLTV for Deposit");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMaximumSelfEmployedLTV()
        {
            try
            {
                IControl ctrl = GetControlByDescription("Calc - webMaxLTV for Self Employed Deposit");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMaximumPTI()
        {
            try
            {
                IControl ctrl = GetControlByDescription("Calc - maxPTICredit");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMaximumTerm()
        {
            //LogPlugin.LogError("System Error{0}{1}", Environment.NewLine, ex.ToString());
            //throw new Exception("GetMaximumTerm Test Exception");

            try
            {
                IControl ctrl = GetControlByDescription("Calc - webMaxTerm");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMinimumPurchasePrice()
        {
            try
            {
                IControl ctrl = GetControlByDescription("Calc - minBond");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        [Obsolete("Please use GetMaximumPurchasePrice(bool) instead")]
        public static double GetMaximumPurchasePrice()
        {
            try
            {
                IControl ctrl = GetControlByDescription("BondHigh");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMaximumPurchasePrice(bool IsSelfEmployed)
        {
            try
            {
                IControl ctrl = GetControlByDescription(IsSelfEmployed ? "Calc - webMaxPurchasePrice for Self Employed" : "Calc - webMaxPurchasePrice for Salaried");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetMinimumHouseholdIncome()
        {
            try
            {
                var creditMatrixRepository = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
                var creditMatrix = creditMatrixRepository.GetCreditMatrix(OriginationSources.SAHomeLoans);

                var exceptionsCreditCriteria = creditMatrix.CreditCriterias.Where(x => x.ExceptionCriteria == true);
                var minimumIncomeAmount = exceptionsCreditCriteria.Min(x => x.MinIncomeAmount);
                return minimumIncomeAmount ?? 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        ///</summary>
        public static double GetFlexiMinFixed()
        {
            try
            {
                IControl ctrl = GetControlByDescription("FlexiMinFixed");
                return ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        #endregion CALCULATION METHODS

        //*******************************************************************************************
        // NEW 2AM SAVE AND X2 CREATE CASE

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DateTime MaturityDate(DateTime fromDate, int term, int MaturityMonths)
        {
            try
            {
                DateTime maturity = fromDate.AddMonths(term + MaturityMonths);
                if (maturity.Day > 15) //get the next month
                    maturity.AddMonths(1);

                // return the last of this month
                return new DateTime(maturity.Year, maturity.Month, 1).AddMonths(1).AddDays(-1);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="ltv"></param>
        ///// <returns></returns>
        //public ISPV NewSPVFromLTV(double ltv)
        //{
        //    try
        //    {
        //        ISPVService SPVService = ServiceFactory.GetService<ISPVService>();
        //        int newSPV = SPVService.GetSPVByLTV(ltv);
        //        return LookupRepository.SPVList.ObjectDictionary[newSPV.ToString()];
        //    }
        //    catch (Exception ex)
        //    {
        //        LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
        //        throw;
        //    }
        //}

        //CREDIT CRITERIA

        public static CreditCriteria SetupCreditCriteria(int OriginationSourceKey, int ProductKey, int MortgageLoanPurpose, int EmploymentTypeKey, double loanamount, double EstimatedPropertyValue, double baserate, short Term, double HouseholdIncome)
        {
            try
            {
                SAHLSession session = new SAHLSession();
                IFinancialsService finsS = ServiceFactory.GetService<IFinancialsService>();
                CreditCriteria CreditCriteria = new CreditCriteria();

                ICreditCriteria cc = finsS.GetCreditCriteriaByLTVAndIncome(session.Messages, OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount, EstimatedPropertyValue, HouseholdIncome,CreditCriteriaAttributeTypes.NewBusiness);

                if (cc == null)
                    cc = finsS.GetCreditCriteriaException(session.Messages, OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount);

                if (cc != null)
                {
                    CreditCriteria.CreditCriteriaKey = cc.Key;
                    CreditCriteria.CreditCriteriaPTI = Convert.ToDouble(cc.PTI);
                    CreditCriteria.CreditCriteriaPTIValue = Convert.ToDouble(cc.PTI.Value);
                    CreditCriteria.CreditCriteriaLTV = Convert.ToDouble(cc.LTV);
                    CreditCriteria.CreditCriteriaLTVValue = Convert.ToDouble(cc.LTV.Value);
                    CreditCriteria.CreditCriteriaMarginValue = cc.Margin.Value;
                    CreditCriteria.CreditCriteriaMarginKey = cc.Margin.Key;
                    CreditCriteria.CreditCriteriaCreditMatrixKey = cc.CreditMatrix.Key;
                    CreditCriteria.CreditCriteriaCategoryKey = cc.Category.Key;
                }

                return CreditCriteria;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        public static CreditCriteria GetAffordabilityCreditCriteria(int OriginationSourceKey, int ProductKey, int MortgageLoanPurpose, int EmploymentTypeKey, double loanamount)
        //double loanamount, double bondrequired, int appType, double cashout, double OverrideCancelFeeamount, bool capitalisefees, out double initiationfee, out double registrationfee, out double cancellationfee, out double interiminterest, out double bondtoregister)
        {
            try
            {
                SAHLSession session = new SAHLSession();
                IFinancialsService finsS = ServiceFactory.GetService<IFinancialsService>();
                CreditCriteria CreditCriteria = new CreditCriteria();

                //TODO Get the first cc
                ICreditCriteria cc = finsS.GetCreditCriteria(session.Messages, OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount);

                CreditCriteria.CreditCriteriaKey = cc.Key;
                CreditCriteria.CreditCriteriaPTI = Convert.ToDouble(cc.PTI);
                CreditCriteria.CreditCriteriaPTIValue = Convert.ToDouble(cc.PTI.Value);
                CreditCriteria.CreditCriteriaMarginValue = cc.Margin.Value;
                CreditCriteria.CreditCriteriaMarginKey = cc.Margin.Key;
                CreditCriteria.CreditCriteriaCreditMatrixKey = cc.CreditMatrix.Key;
                CreditCriteria.CreditCriteriaCategoryKey = cc.Category.Key;

                return CreditCriteria;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }
    }
}