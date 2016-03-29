using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System;
using System.Linq;

namespace SAHL.Common.BusinessModel.Helpers
{
    public class LoanCalculator
    {
        private static IControlRepository _ctrlRepo;
        private static double _minAmountForFees; // = 7067;
        private static double _maxFees; // = 4500;
        private static double _feeBase; // = 1000;
        private static double _feePercentage; // = 0.003;
        private static double _maxFeePercentage; // = 0.15;
        private static double _flCapacity; // = 30000;

        #region old methods - VERY DUBIOUS

        public static double CalculateNewProductInterestOnlyInstallment(IMortgageLoan mortgageLoan, double ProductBalance, double InterestRate, double ActiveMarketRate, double BaseRate)
        {
            double currentBalance = GetTotalLoanValue(mortgageLoan);
            return CalculateNewProductInterestOnlyInstallment(currentBalance, ProductBalance, InterestRate, ActiveMarketRate, BaseRate);
        }

        public static double CalculateNewProductInterestOnlyInstallment(double CurrentBalance, double ProductBalance, double InterestRate, double ActiveMarketRate, double BaseRate)
        {
            if (InterestRate < 1.0e-10)
                throw new Exception("CalculateNewProductInterestOnlyInstallment: InterestRate must be bigger than 0.0");
            if (ActiveMarketRate < 1.0e-10)
                throw new Exception("CalculateNewProductInterestOnlyInstallment: ActiveMarketRate must be bigger than 0.0");
            if (BaseRate < 1.0e-10)
                throw new Exception("CalculateNewProductInterestOnlyInstallment: BaseRate must be bigger than 0.0");
            if (CurrentBalance < 1.0e-10)
                return 0.0;

            // now start the calcs
            double ProductPortion = CurrentBalance - ProductBalance;
            double ProductRate = InterestRate - BaseRate + ActiveMarketRate;
            double Installment = 0.0;
            if (ProductPortion > 1.0e-10 && Math.Abs(ProductRate - InterestRate) > 1.0e-10)
            {
                Installment = CalculateInstallmentInterestOnly(InterestRate, ProductBalance);
                if (ProductRate < 1.0e-10)
                    throw new Exception("CalculateNewProductInterestOnlyInstallment: derived ProductRate must be bigger than 0");
                Installment += CalculateInstallmentInterestOnly(ProductRate, ProductPortion);
            }
            else
            {
                Installment = CalculateInstallmentInterestOnly(InterestRate, CurrentBalance);
            }
            return Installment;
        }

        public static double CalculateNewProductInstallmentAtEndOfPeriod(double InterestRate, double ActiveMarketRate, double BaseRate, IMortgageLoan mortgageLoan, double ProductBalance, int LoanTerm, int InterestPeriods)
        {
            double currentBalance = GetTotalLoanValue(mortgageLoan);
            return CalculateNewProductInstallmentAtEndOfPeriod(InterestRate, ActiveMarketRate, BaseRate, currentBalance, ProductBalance, LoanTerm, InterestPeriods);
        }

        public static double CalculateNewProductInstallmentAtEndOfPeriod(double InterestRate, double ActiveMarketRate, double BaseRate, double CurrentBalance, double ProductBalance, int LoanTerm, int InterestPeriods)
        {
            if (InterestRate < 1.0e-10)
                throw new Exception("CalculateNewProductInstallmentAtEndOfPeriod: InterestRate must be bigger than 0.0");
            if (ActiveMarketRate < 1.0e-10)
                throw new Exception("CalculateNewProductInstallmentAtEndOfPeriod: ActiveMarketRate must be bigger than 0.0");
            if (BaseRate < 1.0e-10)
                throw new Exception("CalculateNewProductInstallmentAtEndOfPeriod: BaseRate must be bigger than 0.0");
            if (LoanTerm < 0)
                throw new Exception("CalculateNewProductInstallmentAtEndOfPeriod: LoanTerm must be bigger than 0");
            if (InterestPeriods < 0)
                throw new Exception("CalculateNewProductInstallmentAtEndOfPeriod: InterestPeriods must be bigger than 0");
            if (CurrentBalance < 1.0e-10)
                return 0.0;

            // now start the calcs
            double ProductPortion = CurrentBalance - ProductBalance;
            double ProductRate = InterestRate - BaseRate + ActiveMarketRate;
            double Installment = 0.0;
            if (ProductPortion > 1.0e-10 && Math.Abs(ProductRate - InterestRate) > 1.0e-10)
            {
                Installment = CalculateInstallmentAtEndOfPeriod(InterestRate, InterestPeriods, ProductBalance, LoanTerm);
                if (ProductRate < 1.0e-10)
                    throw new Exception("CalculateNewProductInstallmentAtEndOfPeriod: derived ProductRate must be bigger than 0");
                Installment += CalculateInstallmentAtEndOfPeriod(ProductRate, InterestPeriods, ProductPortion, LoanTerm);
            }
            else
            {
                Installment = CalculateInstallmentAtEndOfPeriod(InterestRate, InterestPeriods, CurrentBalance, LoanTerm);
            }

            return Installment;
        }

        public static double CalculateInstallmentAtEndOfPeriod(double interestRate, int interestPeriods, double loanAmount, int loanTerm)
        {
            return System.Math.Round((interestRate / interestPeriods) * (loanAmount * System.Math.Pow(1 + interestRate / interestPeriods, loanTerm)) / ((1 + interestRate / interestPeriods * 0) * (1 - System.Math.Pow(1 + interestRate / interestPeriods, loanTerm))) * -1, 2);
        }

        #endregion old methods - VERY DUBIOUS

        public static double CalculateLTV(double TotalLoanAmount, double PropertyValue)
        {
            if (PropertyValue <= 0)
                throw new Exception("PropertyValue must be greater than zero.");

            return TotalLoanAmount / PropertyValue;
        }

        public static double CalculatePTI(double Installment, double HouseHoldIncome)
        {
            if (HouseHoldIncome <= 0)
                return 1; // from IvorJ 100% is deemed far enough outside business scope to return instead of an error //throw new Exception("HouseHoldIncome must be greater than zero.");

            return Installment / HouseHoldIncome;
        }

        public static double CalculateInstallment(IMortgageLoan mortgageLoan, double AnnualInterestRate, double RemainingTerm, bool InterestOnly)
        {
            double currentBalance = GetTotalLoanValue(mortgageLoan);
            return CalculateInstallment(currentBalance, AnnualInterestRate, RemainingTerm, InterestOnly);
        }

        public static double CalculateInstallment(double TotalLoanValue, double AnnualInterestRate, double RemainingTerm, bool InterestOnly)
        {
            if (InterestOnly)
                return CalculateInstallmentInterestOnly(AnnualInterestRate, TotalLoanValue);
            else
                return CalculateInstallment(TotalLoanValue, AnnualInterestRate, RemainingTerm);
        }

        //public static double CalculateInstallment(double AnnualInterestRate, double RemainingTerm, int MortgageLoanPurposeKey, double PurchasePrice, double CurrentLoanValue, double CashRequired, double CashDeposit, double PropertyValue, bool CapitalizeFees, bool NaturalPersonTransferType)
        //{
        //    double TotalLoanValue = 0;

        //    try
        //    {
        //        TotalLoanValue = CalculateTotalLoanRequired(MortgageLoanPurposeKey, PurchasePrice, CurrentLoanValue, CashRequired, CashDeposit, PropertyValue, CapitalizeFees, NaturalPersonTransferType);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return CalculateInstallment(TotalLoanValue, AnnualInterestRate, RemainingTerm);
        //}

        private static double GetTotalLoanValue(IMortgageLoan mortgageLoan)
        {
            double currentBalance = mortgageLoan.CurrentBalance;
            double lifeBalance = 0;
            double hocBalance = 0;

            IFinancialService lifeFinancialService = mortgageLoan.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)SAHL.Common.Globals.FinancialServiceTypes.LifePolicy
                && x.AccountStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active).SingleOrDefault();

            if (lifeFinancialService != null)
            {
                lifeBalance = lifeFinancialService.Balance.Amount;
            }

            IFinancialService hocFinancialService = mortgageLoan.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover
                && x.AccountStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active).SingleOrDefault();

            if (hocFinancialService != null)
            {
                hocBalance = hocFinancialService.Balance.Amount;
            }
            return currentBalance - (lifeBalance + hocBalance);
        }

        private static double CalculateInstallment(double TotalLoanValue, double AnnualInterestRate, double RemainingTerm)
        {
            if (TotalLoanValue < 0.01)
                return 0;

            if (AnnualInterestRate <= 0 || RemainingTerm <= 0)
                throw new Exception("AnnualInterestRate and RemainingTerm must be greater than zero.");

            double MonthlyInterestRate = AnnualInterestRate / 12; // Adjust to monthly rate
            return (MonthlyInterestRate + (MonthlyInterestRate / (Math.Pow((1 + MonthlyInterestRate), RemainingTerm) - 1))) * TotalLoanValue;
        }

        private static double CalculateInstallmentInterestOnly(double AnnualInterestRate, double TotalLoanValue)
        {
            if (TotalLoanValue < 0.01)
                return 0;

            if (AnnualInterestRate <= 0)
            {
                throw new Exception("The parameters AnnualInterestRate and TotalLoanValue must be greater than zero.");
            }

            return ((TotalLoanValue * (AnnualInterestRate / 365)) * 31); // IO instalment always calculated at 31 days to make sure we always collect at a minimum the interest accrued in the month
        }

        //public static double CalculateTotalLoanRequired(int MortgageLoanPurposeKey, double PurchasePrice, double CurrentLoanValue, double CashRequired, double CashDeposit, double PropertyValue, bool CapitalizeFees, bool NaturalPersonTransferType)
        //{
        //    double TotalLoanRequired = 0;

        //    if (MortgageLoanPurposeKey < 2 || MortgageLoanPurposeKey > 4)
        //        throw new Exception("The parameter MortgageLoanPurposeKey refers to a value that is either unknown or not implemented.");

        //    switch (MortgageLoanPurposeKey)
        //    {
        //        case 2: // Switch Loan
        //            TotalLoanRequired = CurrentLoanValue;
        //            break;

        //        case 3: // New Purchase
        //            TotalLoanRequired = PurchasePrice;
        //            break;

        //        case 4: // Refinance
        //            TotalLoanRequired = 0;
        //            break;
        //    }

        //    TotalLoanRequired += (CashRequired - CashDeposit);

        //    // Calculate the fees if we need to capitalize these
        //    if (CapitalizeFees)
        //    {
        //        // uses the NaturalPersonTransferType parameter to calculate the fees
        //        // TODO: Need to finalise teh new fees structure before we imoplement anything meaningful

        //    }

        //    return TotalLoanRequired;
        //}

        public static double CalculateInterimInterest(double ProspectExistingLoan, double BankMortgageRate, double InterimInterestWeeks)
        {
            if (ProspectExistingLoan <= 0 || BankMortgageRate <= 0 || InterimInterestWeeks <= 0)
            {
                throw new Exception("The parameters ProspectExistingLoan, BanksMortgageRate, InterimInterestWeeks must be greater than zero.");
            }

            return (ProspectExistingLoan * (BankMortgageRate * 0.01)) / 52.0 * InterimInterestWeeks;
        }

        //public static double CalculateInterimInterest(double LoanAmount)
        //{
        //    ILookupRepository Lookups = TypeFactory.CreateType<ILookupRepository>();

        //    //Lookups.Controls.ObjectDictionary["

        //    return 0; // (LoanAmount * BankMortgageRate / 100) / 52.0 * InterimInterestWeeks;
        //}

        public static double CalculateInterestOverTerm(double TotalLoanValue, double AnnualInterestRate, double RemainingTerm, bool InterestOnly)
        {
            if (InterestOnly)
                return CalculateInterestOverTermInterestOnly(TotalLoanValue, AnnualInterestRate, RemainingTerm);
            else
                return CalculateInterestOverTerm(TotalLoanValue, AnnualInterestRate, RemainingTerm);
        }

        private static double CalculateInterestOverTerm(double TotalLoanValue, double AnnualInterestRate, double RemainingTerm)
        {
            if (AnnualInterestRate <= 0 || RemainingTerm <= 0 || TotalLoanValue <= 0)
            {
                throw new Exception("The parameters: InterestRate, TotalLoanValue and RemainingTerm, may not be zero or less.");
            }

            double Installment = 0;

            try
            {
                Installment = CalculateInstallment(TotalLoanValue, AnnualInterestRate, RemainingTerm);
            }
            catch (Exception)
            {
                throw;
            }

            return (Installment * RemainingTerm) - TotalLoanValue;
        }

        private static double CalculateInterestOverTermInterestOnly(double TotalLoanValue, double AnnualInterestRate, double RemainingTerm)
        {
            if (AnnualInterestRate <= 0 || RemainingTerm <= 0)
            {
                throw new Exception("The parameters: InterestRate and RemainingTerm, may not be zero or less.");
            }

            double Installment = 0;

            try
            {
                Installment = CalculateInstallmentInterestOnly(AnnualInterestRate, TotalLoanValue);
            }
            catch (Exception)
            {
                throw;
            }

            return (Installment * RemainingTerm);
        }

        //private static double CalculateInterestOverTermInterestOnly(double TotalLoanValue, double AnnualInterestRate, double InterestOnlyTerm, double RemainingTerm)
        //{
        //    double Installment = 0;

        //    if (AnnualInterestRate <= 0 || RemainingTerm <= 0 || TotalLoanValue <= 0)
        //    {
        //        throw new Exception("The parameters InterestRate, TotalLoanValue and RemainingTerm must be greater than zero.");
        //    }

        //    double InterestOverTerm = 0;

        //    try
        //    {
        //        Installment = CalculateInstallmentInterestOnly(AnnualInterestRate, TotalLoanValue);
        //        InterestOverTerm = (Installment * InterestOnlyTerm);

        //        // reduce the RemainingTerm by the Interest-only Term
        //        RemainingTerm -= InterestOnlyTerm;

        //        Installment = CalculateInstallment(TotalLoanValue, AnnualInterestRate, RemainingTerm);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return InterestOverTerm + (Installment * RemainingTerm) - TotalLoanValue;
        //}

        public static double CalculateMinimumIncomeRequired(double Installment, double PTI)
        {
            if (PTI <= 0)
            {
                throw new Exception("The parameter PTI must be greater than zero.");
            }

            return Installment / PTI * 100;
        }

        public static double CalculateLoanTerm(double TotalLoanValue, double AnnualInterestRate, double TotalMonthlyInstallment)
        {
            if (AnnualInterestRate <= 0 || TotalMonthlyInstallment <= 0)
            {
                throw new Exception("The parameters InterestRate and TotalMonthlyInstallment must be greater than zero.");
            }

            double monthlyInterest = TotalLoanValue * AnnualInterestRate / 12;

            if (TotalMonthlyInstallment <= monthlyInterest)
            {
                throw new Exception(String.Format("The TotalMonthlyInstallment ({0}) must cover at least the monthly interest ({1}).", TotalMonthlyInstallment, monthlyInterest));
            }

            return (Math.Log(1 - (TotalLoanValue * AnnualInterestRate / TotalMonthlyInstallment / 12), 1 / (1 + AnnualInterestRate / 12)));
        }

        //public static double CalculateSavingsOnExtraMonthlyPayments(double TotalLoanValue, double AnnualInterestRate, double RemainingTerm, double ExtraMonthlyPayments)
        //{
        //    // Might need to refine later to calculate the figure in one pass (single formula)
        //    double InterestOverTermNormalPayments = 0;
        //    double InterestOverTermExtraPayments = 0;

        //    try
        //    {
        //        // Calculate the total interest over term - assuming the normal monthly installment is paid
        //        InterestOverTermNormalPayments = CalculateInterestOverTerm(TotalLoanValue, AnnualInterestRate, RemainingTerm);

        //        // Calculate the total interest over term - taking into account the extra payments
        //        double MonthlyInstallment = CalculateInstallment(TotalLoanValue, AnnualInterestRate, RemainingTerm);
        //        double term = CalculateLoanTerm(TotalLoanValue, AnnualInterestRate, MonthlyInstallment + ExtraMonthlyPayments);
        //        InterestOverTermExtraPayments = CalculateInterestOverTerm(TotalLoanValue, AnnualInterestRate, term);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return InterestOverTermNormalPayments - InterestOverTermExtraPayments;
        //}

        public static double CalculateFurtherLendingInstallment(double TotalLoanValue, double AnnualVariableInterestRate, double RemainingTerm, double FixedLoanValue, double AnnualFixedInterestRate, bool InterestOnly)
        {
            double fixedInstalment = 0;
            double variableInstalment = 0;

            try
            {
                // Fixed Instalment
                if (FixedLoanValue > 0)
                {
                    if (InterestOnly)
                        fixedInstalment = CalculateInstallmentInterestOnly(AnnualFixedInterestRate, FixedLoanValue);
                    else
                        fixedInstalment = CalculateInstallment(FixedLoanValue, AnnualFixedInterestRate, RemainingTerm);
                }

                // Variable Instalment
                if (InterestOnly)
                    variableInstalment = CalculateInstallmentInterestOnly(AnnualVariableInterestRate, TotalLoanValue - FixedLoanValue);
                else
                    variableInstalment = CalculateInstallment(TotalLoanValue - FixedLoanValue, AnnualVariableInterestRate, RemainingTerm);
            }
            catch (Exception)
            {
                throw;
            }

            return fixedInstalment + variableInstalment;
        }

        public static double CalculateInitiationFees(double LoanAmount, bool NCACompliant)
        {
            if (NCACompliant)
                return 0D;

            double fees;

            if (LoanAmount > MinAmountForFees)
                fees = FeeBase + (LoanAmount * FeePercentage);
            else
                fees = LoanAmount * MaxFeePercentage;

            if (fees > MaxFees)
                fees = MaxFees;

            if (fees > LoanAmount * MaxFeePercentage)
                fees = LoanAmount * MaxFeePercentage;

            return fees;
        }

        public static double CalculateCancellationFee(double FeeCancelDuty, double FeeCancelConveyancing, double FeeCancelVAT)
        {
            //SELECT  top 1 *
            //FROM FEES
            //WHERE FeeRange >= LoanAmount
            //order by FeeRange
            return FeeCancelDuty + FeeCancelConveyancing + FeeCancelVAT;
        }

        public static double CalculateRegistrationFee(double FeeBondStamps, double FeeBondConveyancing, double FeeBondVAT)
        {
            //SELECT  top 1 *
            //FROM FEES
            //WHERE FeeRange >= LoanAmount
            //order by FeeRange

            return FeeBondStamps + FeeBondConveyancing + FeeBondVAT;
        }

        public static double CalculateBondAmount(double LoanAmount)
        {
            double lVal;
            double dVal;

            if (LoanAmount > 0)
                LoanAmount += FLCapacity;

            if (LoanAmount > 0)
                lVal = Math.Round(LoanAmount / 1000, 0);
            else
                lVal = 0;

            dVal = lVal * 1000;

            if ((LoanAmount - dVal) <= 0)
                return dVal;
            else
                return (lVal + 1) * 1000;
        }

        /// <summary>
        /// This method calculates the maximum percentage that may be fixed, for the given parameters.
        /// </summary>
        /// <param name="MaximumIncome"></param>
        /// <param name="TotalLoanAmount">Total loan amount (Fixed and Variable financial services).</param>
        /// <param name="AnnualFixedInterestRate">Total Interest rate (base + link) for the Fixed leg.</param>
        /// <param name="AnnualVariableInterestRate">Total Interest rate (base + link) for the Variable leg.</param>
        /// <returns></returns>
        public static double CalculateMaximumFixedPercentage(double MaximumIncome, double TotalLoanAmount, double AnnualFixedInterestRate, double AnnualVariableInterestRate)
        {
            double maximumFixedPercentage;

            if (AnnualFixedInterestRate != AnnualVariableInterestRate)
                maximumFixedPercentage = (MaximumIncome - TotalLoanAmount * AnnualVariableInterestRate / 12) / (TotalLoanAmount * (AnnualFixedInterestRate - AnnualVariableInterestRate) / 12);
            else
                throw new Exception("The AnnualFixedInterestRate and the AnnualVariableInterestRate values may not be the same.");

            return maximumFixedPercentage > 1 ? 1.0 : maximumFixedPercentage;
        }

        public static ICalculated20YearLoanDetailsFor30YearTermLoan Calculate20YearLoanDetailsFor30YearTermLoan(double totalPricingAdjustment, double thirtyYearPricingAdjustment, double totalLoanValue, double interestRate, double term, double householdIncome)
        {
            ICalculated20YearLoanDetailsFor30YearTermLoan calculated20YearLoanDetailsFor30YearTermLoan = new Calculated20YearLoanDetailsFor30YearTermLoan();
            calculated20YearLoanDetailsFor30YearTermLoan.PricingAdjustment = totalPricingAdjustment - thirtyYearPricingAdjustment;
            calculated20YearLoanDetailsFor30YearTermLoan.EffectiveRate = interestRate - thirtyYearPricingAdjustment;
            calculated20YearLoanDetailsFor30YearTermLoan.Instalment = LoanCalculator.CalculateFurtherLendingInstallment(totalLoanValue, calculated20YearLoanDetailsFor30YearTermLoan.EffectiveRate, term, 0, 0, false);
            calculated20YearLoanDetailsFor30YearTermLoan.PTI = LoanCalculator.CalculatePTI(calculated20YearLoanDetailsFor30YearTermLoan.Instalment, householdIncome);
            return calculated20YearLoanDetailsFor30YearTermLoan;
        }

        private static double MinAmountForFees
        {
            get
            {
                if (_minAmountForFees == 0)
                    _minAmountForFees = GetControlValue("Calc - MinAmountForFees");

                return _minAmountForFees;
            }
        }

        private static double MaxFees
        {
            get
            {
                if (_maxFees == 0)
                    _maxFees = GetControlValue("Calc - MaxFees");

                return _maxFees;
            }
        }

        private static double FeeBase
        {
            get
            {
                if (_feeBase == 0)
                    _feeBase = GetControlValue("Calc - FeeBase");

                return _feeBase;
            }
        }

        private static double FeePercentage
        {
            get
            {
                if (_feePercentage == 0)
                    _feePercentage = GetControlValue("Calc - FeePercentage");

                return _feePercentage;
            }
        }

        private static double MaxFeePercentage
        {
            get
            {
                if (_maxFeePercentage == 0)
                    _maxFeePercentage = GetControlValue("Calc - MaxFeePercentage");

                return _maxFeePercentage;
            }
        }

        private static double FLCapacity
        {
            get
            {
                if (_flCapacity == 0)
                    _flCapacity = GetControlValue("Calc - FurtherLoanCapacity");

                return _flCapacity;
            }
        }

        #region LoanCalc - WhatIf's

        /// <summary>
        ///
        /// </summary>
        /// <param name="CurrentBalance"></param>
        /// <param name="Rate"></param>
        /// <param name="Instalment"></param>
        /// <param name="CapitalisedLife"></param>
        /// <param name="Interest"></param>
        /// <param name="Capital"></param>
        public static void CalculateInstalmentPortion(double CurrentBalance, double Rate, double Instalment, double CapitalisedLife, out double Interest, out double Capital)
        {
            Interest = Math.Round(((Rate / 12) * (CurrentBalance - CapitalisedLife)), 2);
            Capital = Instalment - Interest;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="PMT"></param>
        /// <param name="nper"></param>
        /// <param name="CurBal"></param>
        /// <returns></returns>
        public static double CalculateTotalInterest(double PMT, int nper, double CurBal)
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

        //public static double CalculateRemainingTerm(double LV, double PMT, double i)
        //{
        //    // Calculate the remaining term.
        //    // LV - Current Balance
        //    // i - interest
        //    // PMT - Remaining Amount
        //    // nper - New term
        //    //interest must be monthly for this calc
        //    i = i / 12;
        //    double nper;
        //    double top = System.Math.Log(1 - ((LV * i) / PMT));
        //    double bottom = System.Math.Log(1 / (1 + i));

        //    nper = top / bottom;
        //    return nper;
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="PMT"></param>
        /// <param name="nper"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double CalculateCurrentBalance(double PMT, int nper, double i)
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

        #endregion LoanCalc - WhatIf's

        private static double GetControlValue(string ValueDescription)
        {
            IControl ctrl = CtrlRepo.GetControlByDescription(ValueDescription);
            if (ctrl == null || ctrl.ControlNumeric.HasValue == false)
                throw new Exception("The control defininition for a required calculation value is missing: " + ValueDescription);

            return Convert.ToDouble(ctrl.ControlNumeric.Value);
        }

        private static IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctrlRepo;
            }
        }
    }
}