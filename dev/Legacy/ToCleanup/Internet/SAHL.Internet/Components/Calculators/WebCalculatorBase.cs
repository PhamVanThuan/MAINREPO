using System.Net;
using SAHL.Internet.SAHL.Web.Services.Application;
using SAHL.Internet.SAHL.Web.Services.Calculators;
using SAHL.Internet.SAHL.Web.Services.Globals;



namespace SAHL.Internet.Components.Calculators
{
    /// <summary>
    ///  This Class is used as an interim method for storing preprospects in SAHLDB
    /// This Class will need to be modified to add the Pre-Prospect to the leads workflow
    /// in 2AM when HALO goes live
    /// </summary>
    public class WebCalculatorBase
    {
        // Set up The Web Services Layer
        private readonly WebServiceBase _webServiceBase;
        
        /// <summary>
        /// Gets the <see cref="PreProspect"/> object associated with the calculator.
        /// </summary>
        public PreProspect PreProspect
        {
            get;
            private set;
        }

        /// <summary>
        /// Preprospect Constructor
        /// </summary>
        public WebCalculatorBase()
        {
            _webServiceBase = new WebServiceBase();
            CreatePreProspect();
        }

        /// <summary>
        /// Creates and initializes the PreProspect object.
        /// </summary>
        public bool CreatePreProspect()
        {
            try
            {
                PreProspect = new PreProspect
                                  {
                                      EmploymentType = _webServiceBase.Globals.Salaried(),
                                      ProductKey = GetInitialProductKey(),
                                      marketrateKey = GetInitialMarketRateKey(),
                                  };
                return true;
            }
            catch
            {
                PreProspect = null;
                return false;
            }
        }

        private int GetInitialMarketRateKey()
        {
            return _webServiceBase.Calculators.GetInitialMarketRateKey();
        }

        private int GetInitialProductKey()
        {
            return _webServiceBase.Calculators.GetInitialProductKey();
        }


        //CATEGORIES
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCategory1()
        {
            return _webServiceBase.Globals.GetCategory1();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCategory2()
        {
            return _webServiceBase.Globals.GetCategory2();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCategory3()
        {
            return _webServiceBase.Globals.GetCategory3();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCategory4()
        {
            return _webServiceBase.Globals.GetCategory4();
        }


        //SALARY TYPES
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int EmploymentTypeSalaried()
        {
            return _webServiceBase.Globals.Salaried();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int EmploymentTypeSelfEmployed()
        {
            return _webServiceBase.Globals.SelfEmployed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int EmploymentTypeSubsidised()
        {

            return _webServiceBase.Globals.Subsidised();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int EmploymentTypeUnemployed()
        {
            return _webServiceBase.Globals.Unemployed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int EmploymentTypeUnknown()
        {
            return _webServiceBase.Globals.Unknown();
        }

        //------------------------------------------

        /// <summary>
        /// Returns The Currency Format
        /// </summary>
        /// <returns></returns>
        public string CurrencyFormat()
        {
            return _webServiceBase.Globals.CurrencyFormat();
        }

        /// <summary>
        /// Returns The Currency Format
        /// </summary>
        /// <returns></returns>
        public string CurrencyFormatNoCents()
        {
            return _webServiceBase.Globals.CurrencyFormatNoCents();
        }

        /// <summary>
        /// Returns The Rate Format
        /// </summary>
        /// <returns></returns>
        public string RateFormat()
        {
            return _webServiceBase.Globals.RateFormat();
        }


        // RETURN THE DIFFERENT TYPES OF BASIC PRODUCT KEYS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int VariableLoan()
        {
            return _webServiceBase.Globals.VariableLoan();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int VariFixLoan()
        {
            return _webServiceBase.Globals.VariFixLoan();
        }

        // RETURN THE DIFFERENT TYPES OF PRODUCT KEYS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsSwitchLoanNewVariableLoan()
        {
            return _webServiceBase.Globals.ProductsSwitchLoanNewVariableLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsSwitchLoanVariFixLoan()
        {
            return _webServiceBase.Globals.ProductsSwitchLoanVariFixLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsSwitchLoanSuperLo()
        {
            return _webServiceBase.Globals.ProductsSwitchLoanSuperLo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsNewPurchaseNewVariableLoan()
        {
            return _webServiceBase.Globals.ProductsNewPurchaseNewVariableLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsNewPurchaseVariFixLoan()
        {
            return _webServiceBase.Globals.ProductsNewPurchaseVariFixLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsNewPurchaseSuperLo()
        {
            return _webServiceBase.Globals.ProductsNewPurchaseSuperLo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsRefinanceNewVariableLoan()
        {
            return _webServiceBase.Globals.ProductsRefinanceNewVariableLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsRefinanceVariFixLoan()
        {
            return _webServiceBase.Globals.ProductsRefinanceVariFixLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ProductsRefinanceSuperLo()
        {
            return _webServiceBase.Globals.ProductsRefinanceSuperLo();
        }


        // MORTGAGE LOAN PURPOSES 

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int MortgageLoanPurposesSwitchloan()
        {
            return _webServiceBase.Globals.MortgageLoanPurposesSwitchloan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int MortgageLoanPurposesNewpurchase()
        {
            return _webServiceBase.Globals.MortgageLoanPurposesNewpurchase();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int MortgageLoanPurposesRefinance()
        {
            return _webServiceBase.Globals.MortgageLoanPurposesRefinance();
        }


        // ORIGINATION SOURCES

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int OriginationSourcesSAHomeLoans()
        {
            return _webServiceBase.Globals.OriginationSourcesSAHomeLoans();
        }

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
        public double CalculatePresentValue(double dInstallment, int iTerm, double dLTVRate)
        {
            return _webServiceBase.Calculators.CalculatePresentValue(dInstallment, iTerm, dLTVRate);
        }

        /// <summary>
        /// Calculates the Loan to Value ratio
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="homevalue"></param>
        /// <returns></returns>
        public double CalculateLTV(double loanamount, double homevalue)
        {
            return _webServiceBase.Calculators.CalculateLTV(loanamount, homevalue);

        }

        /// <summary>
        /// Calculates the Payment to income ratio
        /// </summary>
        /// <param name="installment"></param>
        /// <param name="earnings"></param>
        /// <returns></returns>
        public double CalculatePTI(double installment, double earnings)
        {
            return _webServiceBase.Calculators.CalculatePTI(installment, earnings);

        }

        /// <summary>
        /// Returns the instalment value via the web service 
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="interestrate"></param>
        /// <param name="Term"></param>
        /// <param name="InterestOnly"></param>
        /// <returns></returns>
        public double CalculateInstallment(double loanamount, double interestrate, double Term, bool InterestOnly)
        {
            return _webServiceBase.Calculators.CalculateInstallment(loanamount, interestrate, Term, InterestOnly);
        }

        /// <summary>
        /// Returns the interest over the term via the web service 
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="interestrate"></param>
        /// <param name="Term"></param>
        /// <param name="InterestOnly"></param>
        /// <returns></returns>
        public double CalculateInterestOverTerm(double loanamount, double interestrate, double Term, bool InterestOnly)
        {
            return _webServiceBase.Calculators.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public CreditDisqualifications CreditDisqualifications(CreditDisqualifications cd)
        {
            return _webServiceBase.Calculators.CreditDisqualifications(cd);
        }


		public double GetControlValueByDescription(string ControlDescription)
		{
            return _webServiceBase.Calculators.GetControlValueByDescription(ControlDescription);
		}


        /// <summary>
        /// Returns the Maximum Loan Term for web applications
        /// </summary>
        /// <returns></returns>
        public double GetMaximumTerm()
        {
            return _webServiceBase.Calculators.GetMaximumTerm();

        }

        /// <summary>
        /// Returns the Maximum acceptable PTI 
        /// </summary>
        /// <returns></returns>
        public double GetMaximumPTI()
        {
            return _webServiceBase.Calculators.GetMaximumPTI();

        }


        /// <summary>
        /// Returns the Minimum LTV required to not pay a deposit
        /// </summary>
        /// <returns></returns>
        public double GetMinimumLTV()
        {
            return _webServiceBase.Calculators.GetMinimumLTV();

        }

        ///<summary>
        ///Returns the Maximum LTV allowed for Self Employed applications
        ///</summary>
        /// <returns></returns>
        public double GetMaximumSelfEmployedLTV()
        {
            return _webServiceBase.Calculators.GetMaximumSelfEmployedLTV();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ReturnBaseRate()
        {
            return _webServiceBase.Calculators.ReturnBaseRate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ReturnBaseRateFixed()
        {
            return _webServiceBase.Calculators.ReturnBaseRateFixed();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CalculatorMode"></param>
        /// <returns></returns>
        public int ReturnApplicationTypeByCalculatorMode(int CalculatorMode)
        {
            return _webServiceBase.Calculators.ReturnApplicationTypeKeyByCalculatorMode(CalculatorMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fees"></param>
        /// <returns></returns>
        public OriginationFees CalculateOriginationFees(OriginationFees fees)
        {
            //SAHL.Web.Services.Calculators._OriginationFees originationfees = new SAHL.Web.Services.Calculators._OriginationFees();

            //originationfees.appType = fees.appType;
            //originationfees.bondrequired = fees.bondrequired;
            //originationfees.bondtoregister = fees.bondtoregister;
            //originationfees.cancellationfee = fees.cancellationfee;
            //originationfees.capitalisefees = fees.capitalisefees;
            //originationfees.cashout = fees.cashout;
            //originationfees.initiationfee = fees.initiationfee;
            //originationfees.interiminterest = fees.interiminterest;
            //originationfees.loanamount = fees.loanamount;
            //originationfees.OverrideCancelFeeamount = fees.OverrideCancelFeeamount;
            //originationfees.registrationfee = fees.registrationfee;

            return _webServiceBase.Calculators.CalculateOriginationFees(fees);


            //fees.appType = originationfees.appType;
            //fees.bondrequired = originationfees.bondrequired;
            //fees.bondtoregister = originationfees.bondtoregister;
            //fees.cancellationfee = originationfees.cancellationfee;
            //fees.capitalisefees = originationfees.capitalisefees;
            //fees.cashout = originationfees.cashout;
            //fees.initiationfee = originationfees.initiationfee;
            //fees.interiminterest = originationfees.interiminterest;
            //fees.loanamount = originationfees.loanamount;
            //fees.OverrideCancelFeeamount = originationfees.OverrideCancelFeeamount;
            //fees.registrationfee = originationfees.registrationfee;


            //return fees;
        }

        /// <summary>
        /// Returns the minimum income required
        /// </summary>
        /// <param name="instalmentTotal"></param>
        /// <param name="PTI"></param>
        /// <returns></returns>
        public double CalculateMinimumIncomeRequired(double instalmentTotal, double PTI)
        {
            return _webServiceBase.Calculators.CalculateMinimumIncomeRequired(instalmentTotal, PTI);

        }

        ///<summary>
        /// Returns the Minimum Allowed Purchase Price"
        ///</summary>
        public double GetMinimumPurchasePrice()
        {
            return _webServiceBase.Calculators.GetMinimumPurchasePrice(); 

        }

        ///<summary>
        /// Returns the Maximum Allowed Purchase Price
        ///</summary>
        public double GetMaximumPurchasePrice()
        {
            return _webServiceBase.Calculators.GetMaximumPurchasePrice();

        }

        ///<summary>
        ///</summary>
        public double GetMinimumHouseholdIncome()
        {
            return _webServiceBase.Calculators.GetMinimumHouseholdIncome();

        }

        ///<summary>
        ///</summary>
        public double GetMinimumMarketValue()
        {
            return _webServiceBase.Calculators.GetMinimumMarketValue();

        }

        ///<summary>
        ///</summary>
        public double GetFlexiMinFixed()
        {
            return _webServiceBase.Calculators.GetFlexiMinFixed();

        }


        #endregion

        //*********************************************************************************************
        // This section the new Replacement for HALO /2AM
        /// <summary>
        /// creates a New 2AM lead
        /// </summary>
        /// <returns>True if a lead was created and false if not</returns>
        public int CreateLead(PreProspect preProspect)
        {
            return _webServiceBase.Application.CreateLead(preProspect);
            //return 0;
        }


        /// <summary>
        /// Returns the Bond Amount
        /// </summary>
        /// <param name="LoanAmount"></param>
        /// <returns></returns>
        public double CalculateBondAmount(double LoanAmount)
        {
            return _webServiceBase.Calculators.CalculateBondAmount(LoanAmount);

        }



        /// <summary>
        /// Calculates the PTI via the Business Model
        /// </summary>
        /// <param name="loanamount"></param>
        /// <param name="marginvalue"></param>
        /// <param name="baserate"></param>
        /// <param name="Term"></param>
        /// <param name="HouseholdIncome"></param>
        /// <returns></returns>
        public double CalculatePTIThroughBusinessModel(double loanamount, double marginvalue, double baserate, short Term, double HouseholdIncome)
        {
            return _webServiceBase.Calculators.CalculatePTIThroughBusinessModel(loanamount, marginvalue, baserate, Term, HouseholdIncome);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TotalLoanAmount"></param>
        /// <param name="PropertyValue"></param>
        /// <returns></returns>
        public double CalculateLTVThroughBusinessModel(double TotalLoanAmount, double PropertyValue)
        {
            return _webServiceBase.Calculators.CalculateLTVThroughBusinessModel(TotalLoanAmount, PropertyValue);

        }

        /// <summary>
        /// This method sets up The Credit Criteria for the Application
        /// </summary>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurpose"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="loanamount"></param>
        /// <param name="EstimatedPropertyValue"></param>
        /// <param name="baserate"></param>
        /// <param name="Term"></param>
        /// <param name="HouseholdIncome"></param>
        /// <returns></returns>
        public CreditCriteria SetupCreditCriteria(int OriginationSourceKey, int ProductKey, int MortgageLoanPurpose, int EmploymentTypeKey, double loanamount, double EstimatedPropertyValue, double baserate, short Term, double HouseholdIncome)
        {
            //CreditCriteria retval = new CreditCriteria();
            //SAHL.Web.Services.Calculators.CreditCriteria cc = calculators.SetupCreditCriteria(OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount, EstimatedPropertyValue, baserate, Term, HouseholdIncome);
            //retval.CreditCriteriaCategoryKey = cc.CreditCriteriaCategoryKey;
            //retval.CreditCriteriaCreditMatrixKey = cc.CreditCriteriaCreditMatrixKey;
            //retval.CreditCriteriaKey = cc.CreditCriteriaKey;
            //retval.CreditCriteriaMarginKey = cc.CreditCriteriaMarginKey;
            //retval.CreditCriteriaMarginValue = cc.CreditCriteriaMarginValue;
            //retval.CreditCriteriaPTI = cc.CreditCriteriaPTI;
            //retval.CreditCriteriaPTIValue = cc.CreditCriteriaPTIValue;
            //return retval;

            return _webServiceBase.Calculators.SetupCreditCriteria(OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount, EstimatedPropertyValue, baserate, Term, HouseholdIncome);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurpose"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="loanamount"></param>
        /// <returns></returns>
        public CreditCriteria GetAffordabilityCreditCriteria(int OriginationSourceKey, int ProductKey, int MortgageLoanPurpose, int EmploymentTypeKey, double loanamount)
        {
            //CreditCriteria retval = new CreditCriteria();
            //CreditCriteria cc = calculators.GetAffordabilityCreditCriteria(OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount);
            //retval.CreditCriteriaCategoryKey = cc.CreditCriteriaCategoryKey;
            //retval.CreditCriteriaCreditMatrixKey = cc.CreditCriteriaCreditMatrixKey;
            //retval.CreditCriteriaKey = cc.CreditCriteriaKey;
            //retval.CreditCriteriaMarginKey = cc.CreditCriteriaMarginKey;
            //retval.CreditCriteriaMarginValue = cc.CreditCriteriaMarginValue;
            //retval.CreditCriteriaPTI = cc.CreditCriteriaPTI;
            //retval.CreditCriteriaPTIValue = cc.CreditCriteriaPTIValue;
            //return retval;
            return _webServiceBase.Calculators.GetAffordabilityCreditCriteria(OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount);
        }

        //*******************************************************************************************
        // NEW 2AM SAVE AND X2 CREATE CASE






    }
}