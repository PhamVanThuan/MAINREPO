using System;
using System.ComponentModel;
using System.Web.Services;
using SAHL.Common.Globals;

namespace SAHL.Web.Services
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://webservices.sahomeloans.com/",
     Description = "This is the SA Homeloans Calculator Web Service.")]

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Calculators : WebService
    {
        [WebMethod(Description = "Returns the present value of an investment. The present value is the total amount that a series of future payments is worth now. For example, when you borrow money, the loan amount is the present value to the lender")]
        public double CalculatePresentValue(double dInstallment, int iTerm, double dLTVRate)
        {
            return CalculatorBase.CalculatePresentValue(dInstallment, iTerm, dLTVRate);
        }

        [WebMethod(Description = "Calculates & Returns the Loan to Value ratio")]
        public double CalculateLTV(double loanamount, double homevalue)
        {
            return CalculatorBase.CalculateLTV(loanamount, homevalue);
        }

        [WebMethod(Description = "Calculates & Returns the the Payment to income ratio")]
        public double CalculatePTI(double installment, double earnings)
        {
            return CalculatorBase.CalculatePTI(installment, earnings);
        }

        [WebMethod(Description = "Not Quite Sure what this does yet. To be continued....")]
        public CreditDisqualifications CreditDisqualifications(CreditDisqualifications cd)
        {
            return CalculatorBase.CreditDisqualifications(cd);
        }

        [WebMethod(Description = "Returns the Current Base Rate")]
        public double ReturnBaseRate()
        {
            return CalculatorBase.ReturnBaseRate();
        }

        [WebMethod(Description = "Returns the Current Fixed Base Rate")]
        public double ReturnBaseRateFixed()
        {
            return CalculatorBase.ReturnBaseRateFixed();
        }

        [WebMethod(Description = "Returns the current enumeration value based on the web calculator type")]
        public int ReturnApplicationTypeKeyByCalculatorMode(int CalculatorMode)
        {
            switch (CalculatorMode)
            {
                case 2:
                    return (int)OfferTypes.SwitchLoan;
                case 3:
                    return (int)OfferTypes.NewPurchaseLoan;
                case 4:
                    return (int)OfferTypes.RefinanceLoan;
                default:
                    return (int)OfferTypes.SwitchLoan;
            }
        }

        //// Create a new object
        //[WebMethod(Description = "Calculates the Origination Fees")]
        //public void CalculateOriginationFees(double loanamount, double bondrequired, int appType, double cashout, double OverrideCancelFeeamount, bool capitalisefees, out double initiationfee, out double registrationfee, out double cancellationfee, out double interiminterest, out double bondtoregister)
        //{
        //    ApplicationTypes applicationTypes = (ApplicationTypes) Enum.Parse(typeof (ApplicationTypes), appType.ToString());
        //    IApplicationRepository apprep = RepositoryFactory.GetRepository<IApplicationRepository>();
        //    CalculatorBase.CalculateOriginationFees(loanamount, bondrequired, applicationTypes, cashout, OverrideCancelFeeamount, capitalisefees, out initiationfee, out registrationfee, out cancellationfee, out interiminterest, out bondtoregister);
        //}

        // Create a new object
        [WebMethod(Description = "Calculates the Origination Fees")]
        [Obsolete("CalculateOriginationFees has been made obsolete, please use CalculateFees")]
        public OriginationFees CalculateOriginationFees(OriginationFees originationfees)
        {
            OriginationFees fees = new OriginationFees();
            fees.appType = originationfees.appType;
            fees.bondrequired = originationfees.bondrequired;
            fees.bondtoregister = originationfees.bondtoregister;
            fees.cancellationfee = originationfees.cancellationfee;
            fees.capitalisefees = originationfees.capitalisefees;
            fees.cashout = originationfees.cashout;
            fees.initiationfee = originationfees.initiationfee;
            fees.interiminterest = originationfees.interiminterest;
            fees.loanamount = originationfees.loanamount;
            fees.OverrideCancelFeeamount = originationfees.OverrideCancelFeeamount;
            fees.registrationfee = originationfees.registrationfee;

            fees = CalculatorBase.CalculateOriginationFees(fees);

            originationfees.initiationfee = fees.initiationfee;
            originationfees.registrationfee = fees.registrationfee;
            originationfees.cancellationfee = fees.cancellationfee;
            originationfees.interiminterest = fees.interiminterest;
            originationfees.bondtoregister = fees.bondtoregister;

            return originationfees;
        }

        [WebMethod(Description = "Calculates the Origination Fees")]
        public OriginationFees CalculateFees(OriginationFees originationfees)
        {
            OriginationFees fees = new OriginationFees();
            fees.appType = originationfees.appType;
            fees.bondrequired = originationfees.bondrequired;
            fees.bondtoregister = originationfees.bondtoregister;
            fees.cancellationfee = originationfees.cancellationfee;
            fees.capitalisefees = originationfees.capitalisefees;
            fees.cashout = originationfees.cashout;
            fees.initiationfee = originationfees.initiationfee;
            fees.interiminterest = originationfees.interiminterest;
            fees.loanamount = originationfees.loanamount;
            fees.OverrideCancelFeeamount = originationfees.OverrideCancelFeeamount;
            fees.registrationfee = originationfees.registrationfee;
            fees.householdIncome = originationfees.householdIncome;
            fees.propertyValue = originationfees.propertyValue;
            fees.employmentTypeKey = originationfees.employmentTypeKey;
            
            fees = CalculatorBase.CalculateFees(fees);

            originationfees.initiationfee = fees.initiationfee;
            originationfees.registrationfee = fees.registrationfee;
            originationfees.cancellationfee = fees.cancellationfee;
            originationfees.interiminterest = fees.interiminterest;
            originationfees.bondtoregister = fees.bondtoregister;

            return originationfees;
        }

        [WebMethod(Description = "Calculates the Minimum Income Required")]
        public double CalculateMinimumIncomeRequired(double instalmentTotal, double PTI)
        {
            return CalculatorBase.CalculateMinimumIncomeRequired(instalmentTotal, PTI);
        }

        [WebMethod(Description = "Returns the Minimum Purchase Price")]
        public double GetMinimumPurchasePrice()
        {
            return CalculatorBase.GetMinimumPurchasePrice();
        }

        [Obsolete("Please use GetMaximumPurchasePriceForSelfEmployedOrSalaried(bool) instead")]
        [WebMethod(Description = "Returns the Maximum Purchase Price")]
        public double GetMaximumPurchasePrice()
        {
            return CalculatorBase.GetMaximumPurchasePrice();
        }

        [WebMethod(Description = "Returns the Maximum Purchase Price, depending on SelfEmployed or Salaried")]
        public double GetMaximumPurchasePriceForSelfEmployedOrSalaried(bool IsSelfEmployed)
        {
            return CalculatorBase.GetMaximumPurchasePrice(IsSelfEmployed);
        }

        [WebMethod(Description = "Returns the Minimum Household Income")]
        public double GetMinimumHouseholdIncome()
        {
            return CalculatorBase.GetMinimumHouseholdIncome();
        }

        [WebMethod(Description = "Returns the Maximum LTV allowed")]
        public double GetMinimumLTV()
        {
            return CalculatorBase.GetMinimumLTV();
        }

        [WebMethod(Description = "Returns the Maximum LTV allowed for Self Employed applications")]
        public double GetMaximumSelfEmployedLTV()
        {
            return CalculatorBase.GetMaximumSelfEmployedLTV();
        }

        [WebMethod(Description = "Returns the Maximum acceptable PTI ")]
        public double GetMaximumPTI()
        {
            return CalculatorBase.GetMaximumPTI();
        }

        [WebMethod(Description = "Returns the Maximum Loan Term for web applications")]
        public double GetMaximumTerm()
        {
            return CalculatorBase.GetMaximumTerm();
        }

        [WebMethod(Description = "Returns the Minimum purchase price")]
        public double GetMinimumMarketValue()
        {
            return CalculatorBase.GetMinimumPurchasePrice();
        }

        [WebMethod(Description = "Returns the Minimum Fixed amount on a flexi bond")]
        public double GetFlexiMinFixed()
        {
            return CalculatorBase.GetFlexiMinFixed();
        }

        [WebMethod(Description = "Returns the Initial Product Key")]
        public int GetInitialProductKey()
        {
            return (int)ProductsNewPurchaseAtCreation.NewVariableLoan;  // Default to variable loan
        }

        [WebMethod(Description = "Returns the Initial Market Rate key (Web Calculators)")]
        public int GetInitialMarketRateKey()
        {
            return (int)MarketRates.ThreeMonthJIBARRounded;
        }

        [WebMethod(Description = "Calculates the PTI - Though the Domain Business model")]
        public double CalculatePTIThroughBusinessModel(double loanamount, double marginvalue, double baserate, short Term, double HouseholdIncome)
        {
            return Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanamount, (marginvalue + baserate), Term, false), HouseholdIncome);
        }

        [WebMethod(Description = "Calculates the LTV - Though the Domain Business model")]
        public double CalculateLTVThroughBusinessModel(double TotalLoanAmount, double PropertyValue)
        {
            return Common.BusinessModel.Helpers.LoanCalculator.CalculateLTV(TotalLoanAmount, PropertyValue);
        }

        //CREDIT CRITERIA

        // Create a new object
        [WebMethod(Description = "Returns a populated Credit Criteria Object")]
        public CreditCriteria SetupCreditCriteria(int OriginationSourceKey, int ProductKey, int MortgageLoanPurpose, int EmploymentTypeKey, double loanamount, double EstimatedPropertyValue, double baserate, short Term, double HouseholdIncome)
        //double loanamount, double bondrequired, int appType, double cashout, double OverrideCancelFeeamount, bool capitalisefees, out double initiationfee, out double registrationfee, out double cancellationfee, out double interiminterest, out double bondtoregister)
        {
            return CalculatorBase.SetupCreditCriteria(OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount, EstimatedPropertyValue, baserate, Term, HouseholdIncome);
        }

        // Create a new object
        [WebMethod(Description = "Returns a populated Credit Criteria Object for the Affordability Calculation")]
        public CreditCriteria GetAffordabilityCreditCriteria(int OriginationSourceKey, int ProductKey, int MortgageLoanPurpose, int EmploymentTypeKey, double loanamount)
        //double loanamount, double bondrequired, int appType, double cashout, double OverrideCancelFeeamount, bool capitalisefees, out double initiationfee, out double registrationfee, out double cancellationfee, out double interiminterest, out double bondtoregister)
        {
            return CalculatorBase.GetAffordabilityCreditCriteria(OriginationSourceKey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount);
        }

        /// <summary>
        /// Returns the Bond Amount
        /// </summary>
        /// <param name="LoanAmount"></param>
        /// <returns></returns>
        [WebMethod(Description = "Calculates and Returns the Bond Amount")]
        public double CalculateBondAmount(double LoanAmount)
        {
            return Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(LoanAmount);
        }

        [WebMethod(Description = "Calculates the installment value via the Business Model")]
        public double CalculateInstallment(double loanamount, double interestrate, double Term, bool InterestOnly)
        {
            double installment = CalculatorBase.CalculateInstallment(loanamount, interestrate, Term, InterestOnly);
            return installment;
        }

        [WebMethod(Description = "Calculates the installment value via the Business Model")]
        public double CalculateInterestOverTerm(double loanamount, double interestrate, double Term, bool InterestOnly)
        {
            double interest = CalculatorBase.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);
            return interest;
        }

        [WebMethod(Description = "Get Control Table value by Description")]
        public double GetControlValueByDescription(string description)
        {
            double value = CalculatorBase.GetControlValueByDescription(description);
            return value;
        }
    }
}