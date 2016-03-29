using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ApplicationCalculateMortgageLoanHelperTest : TestBase
    {
        private void CalculateOriginationFeesTestHelper(double LoanAmount, double BondRequired, OfferTypes appType, double CashOut, double OverrideCancelFee, bool CapitaliseFees, bool NCACompliant, bool IsBondExceptionAction, out double InitiationFee, out double RegFee, out double CancelFee, out double InterimInterest, out double BondToRegister, out double LoanExFees, out double MinFeeRange, bool IsQuickPayLoan, double HouseholdIncome, int EmploymentTypeKey, double PropertyValue, bool IsStaffLoan, bool isGEPF)
        {
            #region OutPrms

            InterimInterest = 0;
            CancelFee = 0;
            InitiationFee = 0;
            BondToRegister = 0;
            RegFee = 0;
            LoanExFees = 0;
            MinFeeRange = 0;

            #endregion OutPrms

            #region vars

            double amount = 0;

            #endregion vars

            bool isAlphaHousing = false;
            if (appType != OfferTypes.FurtherLoan &&
                PropertyValue > 0 &&
                LoanAmount > 0)
            {
                double ltv = LoanAmount / PropertyValue;
                isAlphaHousing = IsAlphaHousing(ltv, EmploymentTypeKey, HouseholdIncome, IsStaffLoan, isGEPF);
            }

            if (appType == OfferTypes.SwitchLoan) //only switch gets interest
                InterimInterest = ((((LoanAmount - CashOut)) * (BankRate * 0.01) / 52) * InterimInterestWeeks);

            LoanAmount += InterimInterest;

            amount = Math.Round(LoanAmount, 0);

            DataRow tdr = GetFeeRow(amount);

            if (appType == OfferTypes.SwitchLoan) // only switch gets cancel fee
            {
                if (OverrideCancelFee > 0)
                    CancelFee = OverrideCancelFee;
                else
                    CancelFee = Convert.ToDouble(tdr[FeeColumns.FeeCancelDuty.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeCancelConveyancing.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeCancelVAT.ToString()].ToString());
            }

            if (NCACompliant == false)
            {
                InitiationFee = 5700D;
            }

            //QuickPayFee is applied if applicable
            if (IsQuickPayLoan == true)
                InitiationFee = (amount * QuickPayFeePCT);

            if (CapitaliseFees == true)
            {
                if (isAlphaHousing)
                {
                    RegFee = Convert.ToDouble(tdr[FeeColumns.FeeBondStamps.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondConveyancingNoFICA.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondNoFICAVAT.ToString()].ToString());
                }
                else
                {
                    if (appType == OfferTypes.FurtherLoan || appType == OfferTypes.NewPurchaseLoan) // 80% fee for Further Loan & New Purchase
                        RegFee = Convert.ToDouble(tdr[FeeColumns.FeeBondStamps.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondConveyancing80Pct.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondVAT80Pct.ToString()].ToString());
                    else
                        RegFee = Convert.ToDouble(tdr[FeeColumns.FeeBondStamps.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondConveyancing.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondVAT.ToString()].ToString());
                }

                amount = amount + RegFee + InitiationFee + CancelFee;
            }

            //Calc Bond To Register
            double lval = 0;
            double dval = 0;
            if (amount > 0 && IsBondExceptionAction == false)
            {
                amount = amount + Buffer;
                lval = Math.Round((amount / 1000), 0);
                dval = lval * 1000;
                if (dval > amount)
                    BondToRegister = dval;
                else
                    BondToRegister = (lval + 1) * 1000;

                BondToRegister = GetFeeRowValue(BondToRegister, FeeColumns.FeeRange);
            }
            else
            {
                //BondToRegister must be at least the Loan Amount
                if (BondRequired < amount)
                    BondToRegister = amount;
                else
                    BondToRegister = BondRequired;
            }

            //if the calculated bondtoreg < what the client wants, reset
            if (BondToRegister < BondRequired)
                BondToRegister = BondRequired;

            amount = Math.Round(BondToRegister, 0);

            //2nd pass for registration fees
            tdr = GetFeeRow(amount);

            if (isAlphaHousing && appType != OfferTypes.FurtherLoan)
            {
                RegFee = Convert.ToDouble(tdr[FeeColumns.FeeBondStamps.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondConveyancingNoFICA.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondNoFICAVAT.ToString()].ToString());
            }
            else
            {
                if (appType == OfferTypes.FurtherLoan || appType == OfferTypes.NewPurchaseLoan) // 80% fee for Further Loan & New Purchase
                    RegFee = Convert.ToDouble(tdr[FeeColumns.FeeBondStamps.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondConveyancing80Pct.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondVAT80Pct.ToString()].ToString());
                else
                    RegFee = Convert.ToDouble(tdr[FeeColumns.FeeBondStamps.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondConveyancing.ToString()].ToString()) + Convert.ToDouble(tdr[FeeColumns.FeeBondVAT.ToString()].ToString());
            }

            LoanExFees = LoanAmount;
            MinFeeRange = (LoanAmount * (FeePCT + 1)) + (FeeBase + Buffer);

            return;
        }

        [Test]
        public void CalculateOriginationFees()
        {
            // Test values that can change
            double loanAmount = 0;
            double bondRequired = 0;
            double CashOut = 0;
            double OverrideCancelFee = 0;

            bool capitaliseFees = true;
            bool capitaliseInitiationFee = false;
            bool NCACompliant = true;
            bool IsBondExceptionAction = true;
            bool IsQuickPayLoan = true;

            double HouseholdIncome;
            int EmploymentTypeKey = 1;
            double PropertyValue;
            int ApplicationParentAccountKey = 0;
            bool IsStaffLoan = false;
            bool isGEPF = false;

            foreach (DataRow dr in FeeData.Rows)
            {
                foreach (double d in RangeCheck)
                {
                    loanAmount = Convert.ToDouble((dr[(int)FeeColumns.FeeRange].ToString())) + d;
                    if (loanAmount > 50 && loanAmount < 9950000D)
                    {
                        foreach (OfferTypes ot in listOfferTypes)
                        {
                            bondRequired = loanAmount - 50;
                            CashOut = 0;
                            OverrideCancelFee = 0;

                            capitaliseFees = false;
                            NCACompliant = false;
                            IsBondExceptionAction = false;
                            IsQuickPayLoan = false;

                            PropertyValue = loanAmount * 1.15;

                            HouseholdIncome = 10000;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);

                            HouseholdIncome = 20000;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);

                            capitaliseFees = true;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);

                            OverrideCancelFee = 2500D;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);
                            OverrideCancelFee = 0;

                            NCACompliant = true;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);

                            IsBondExceptionAction = true;
                            bondRequired = loanAmount;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);
                            IsBondExceptionAction = false;
                            bondRequired += 50;
                            capitaliseFees = false;
                            IsQuickPayLoan = true;
                            RunTest(loanAmount, bondRequired, ot, CashOut, OverrideCancelFee, capitaliseFees, NCACompliant, IsBondExceptionAction, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, capitaliseInitiationFee, isGEPF);
                        }
                    }
                }
            }
        }

        [Test, Description("Tests that the Control table values for the Fees Calculation are configured correctly.")]
        public void CalculateOriginationFeesControlTableData()
        {
            double expectedMaxFees = 5472;
            Assert.AreEqual(expectedMaxFees, MaxFees);
            double expectedMinAmountForFees = 7067;
            Assert.AreEqual(expectedMinAmountForFees, MinAmountForFees);
            double expectedFeesBase = 1140;
            Assert.AreEqual(expectedFeesBase, FeeBase);
            double expectedFeePCT = 0.00513;
            Assert.AreEqual(expectedFeePCT, FeePCT);
            double expectedMaxFeePCT = 0.15;
            Assert.AreEqual(expectedMaxFeePCT, MaxFeePCT);
            double expectedQuickPayFeePCT = 0.025;
            Assert.AreEqual(expectedQuickPayFeePCT, QuickPayFeePCT);
            double expectedBuffer = 30000;
            Assert.AreEqual(expectedBuffer, Buffer);
            double expectedInterimInterestWeeks = 12;
            Assert.AreEqual(expectedInterimInterestWeeks, InterimInterestWeeks);
            double expectedBankRate = 9.25;
            Assert.AreEqual(expectedBankRate, BankRate);
        }

        private void RunTest(double LoanAmount, double BondRequired, OfferTypes appType, double CashOut, double OverrideCancelFee, bool CapitaliseFees, bool NCACompliant, bool IsBondExceptionAction, bool IsQuickPayLoan, double HouseholdIncome, int EmploymentTypeKey, double PropertyValue, int ApplicationParentAccountKey, bool IsStaffLoan, bool CapitaliseInitiationFee, bool isGEPF)
        {
            string message = String.Format(@"OfferType: {0}, BondRequired = {1}, LoanAmount = {2}, CashOut = {3}, OverrideCancelFee = {4}, CapitaliseFees = {5}, NCACompliant = {6}, IsBondExceptionAction = {7}, IsQuickPayLoan = {8}, HouseholdIncome = {9}, PropertyValue = {10}", appType.ToString(), BondRequired.ToString(), LoanAmount.ToString(), CashOut.ToString(), OverrideCancelFee.ToString(), CapitaliseFees.ToString(), NCACompliant.ToString(), IsBondExceptionAction.ToString(), IsQuickPayLoan.ToString(), HouseholdIncome.ToString(), PropertyValue.ToString());

            //Output values to check
            double InitiationFee = 0;
            double RegistrationFee = 0;
            double CancelFee = 0;
            double InterimInterest = 0;
            double BondToRegister = 0;

            double InitiationFeeTestVal = 0;
            double RegistrationFeeTestVal = 0;
            double LoanExFeesTestVal = 0;
            double MinFeeRangeTestVal = 0;
            double CancelFeeTestVal = 0;
            double InterimInterestTestVal = 0;
            double BondToRegisterTestVal = 0;
            double? InitiationFeeDiscount;

            CalculateOriginationFeesTestHelper(LoanAmount, BondRequired, appType, CashOut, OverrideCancelFee, CapitaliseFees, NCACompliant, IsBondExceptionAction, out InitiationFeeTestVal, out RegistrationFeeTestVal, out CancelFeeTestVal, out InterimInterestTestVal, out BondToRegisterTestVal, out LoanExFeesTestVal, out MinFeeRangeTestVal, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, IsStaffLoan, isGEPF);
            ApplicationCalculateMortgageLoanHelper.CalculateOriginationFees(LoanAmount, BondRequired, appType, CashOut, OverrideCancelFee, CapitaliseFees, NCACompliant, IsBondExceptionAction, false, out InitiationFeeDiscount, out InitiationFee, out RegistrationFee, out CancelFee, out InterimInterest, out BondToRegister, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, DateTime.Now, CapitaliseInitiationFee, false);

            Assert.AreEqual(InitiationFeeTestVal, InitiationFee, 0.01, "Initiation Fee mismatch for test: " + message);
            Assert.AreEqual(RegistrationFeeTestVal, RegistrationFee, 0.01, "Registration Fee mismatch for test: " + message);
            Assert.AreEqual(CancelFeeTestVal, CancelFee, 0.01, "Cancel Fee mismatch for test: " + message);
            Assert.AreEqual(InterimInterestTestVal, InterimInterest, 0.01, "InterimInterest mismatch for test: " + message);
            Assert.IsTrue(CheckVals(BondToRegisterTestVal, BondToRegister, 1), "BondToRegister Fee mismatch for test: " + message);
        }

        private bool CheckVals(double test, double sp, double fudge)
        {
            if (test <= (sp + fudge) && test >= (sp - fudge))
                return true;

            return false;
        }

        #region FeeData

        private static string GetFeesSql = @"select distinct
                                                [FeeRange]
                                                ,[FeeBondStamps]
                                                ,[FeeBondConveyancing]
                                                ,[FeeBondConveyancing80Pct]
                                                ,[FeeBondVAT]
                                                ,[FeeBondVAT80Pct]
                                                ,[FeeCancelDuty]
                                                ,[FeeCancelConveyancing]
                                                ,[FeeCancelVAT]
                                                ,[FeeBondConveyancingNoFICA]
                                                ,[FeeBondNoFICAVAT]
                                                from Fees (nolock)
                                                order by FeeRange";

        private DataTable dt;

        private DataTable FeeData
        {
            get
            {
                if (dt == null)
                    dt = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(GetFeesSql, typeof(GeneralStatus_DAO), null).Tables[0];

                return dt;
            }
        }

        private DataRow[] dr;

        private DataRow GetFeeRow(double feeRange)
        {
            dr = FeeData.Select(String.Format(@"FeeRange >= {0}", feeRange.ToString()));
            return dr[0];
        }

        private double GetFeeRowValue(double feeRange, FeeColumns fc)
        {
            //int i = (int)Enum.Parse(enumValue.GetType(), enumValue.ToString());

            return Convert.ToDouble(GetFeeRow(feeRange)[(int)fc].ToString());
        }

        #endregion FeeData

        #region AlphaHousing

        private bool IsAlphaHousing(double LTV, int EmploymentTypeKey, double HouseholdIncome, bool IsStaffLoan, bool isGEPF)
        {
            bool result = false;

            string IsAlphaHousingSql = string.Format(@"select count(1) FROM GetAlphaHousingOfferAttributes({0}, {1}, {2}, {3}, {4}, 0) WHERE OfferAttributeTypeKey = 26 AND Remove = 0", LTV, EmploymentTypeKey, HouseholdIncome, Convert.ToInt32(IsStaffLoan), Convert.ToInt32(isGEPF));
            DataTable dt = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(IsAlphaHousingSql, typeof(GeneralStatus_DAO), null).Tables[0];
            DataRow[] dr = dt.Select();

            if (dr.Length > 0 &&
                dr[0].ItemArray.Length > 0)
                result = (dr[0].ItemArray[0].ToString() == "1");

            return result;
        }

        #endregion AlphaHousing

        private enum FeeColumns
        {
            FeeRange = 0,
            FeeBondStamps,
            FeeBondConveyancing,
            FeeBondConveyancing80Pct,
            FeeBondVAT,
            FeeBondVAT80Pct,
            FeeCancelDuty,
            FeeCancelConveyancing,
            FeeCancelVAT,
            FeeBondConveyancingNoFICA,
            FeeBondNoFICAVAT
        }

        #region ControlValues

        private List<OfferTypes> lOT;

        private List<OfferTypes> listOfferTypes
        {
            get
            {
                if (lOT == null)
                {
                    lOT = new List<OfferTypes>();
                    lOT.Add(OfferTypes.FurtherAdvance);
                    lOT.Add(OfferTypes.FurtherLoan);
                    lOT.Add(OfferTypes.NewPurchaseLoan);
                    lOT.Add(OfferTypes.ReAdvance);
                    lOT.Add(OfferTypes.RefinanceLoan);
                    lOT.Add(OfferTypes.SwitchLoan);
                }

                return lOT;
            }
        }

        private List<double> rc;

        private List<double> RangeCheck
        {
            get
            {
                if (rc == null)
                {
                    rc = new List<double>();
                    rc.Add(-5);
                    rc.Add(0);
                    rc.Add(5);
                }

                return rc;
            }
        }

        private double br = 0;

        private double BankRate
        {
            get
            {
                if (br == 0)
                    br = ctrlRepo.GetControlByDescription("Banks Mortgage Rate").ControlNumeric ?? 0;

                return br;
            }
        }

        private double iiw = 0;

        private double InterimInterestWeeks
        {
            get
            {
                if (iiw == 0)
                    iiw = ctrlRepo.GetControlByDescription("Interim Interest Weeks").ControlNumeric ?? 0;

                return iiw;
            }
        }

        private double maff = 0;

        private double MinAmountForFees
        {
            get
            {
                if (maff == 0)
                    maff = ctrlRepo.GetControlByDescription("Calc - MinAmountForFees").ControlNumeric ?? 0;

                return maff;
            }
        }

        private double fb = 0;

        private double FeeBase
        {
            get
            {
                if (fb == 0)
                    fb = ctrlRepo.GetControlByDescription("Calc - FeeBase").ControlNumeric ?? 0;

                return fb;
            }
        }

        private double fPct = 0;

        private double FeePCT
        {
            get
            {
                if (fPct == 0)
                    fPct = ctrlRepo.GetControlByDescription("Calc - FeePercentage").ControlNumeric ?? 0;

                return fPct;
            }
        }

        private double mF = 0;

        private double MaxFees
        {
            get
            {
                if (mF == 0)
                    mF = ctrlRepo.GetControlByDescription("Calc - MaxFees").ControlNumeric ?? 0;

                return mF;
            }
        }

        private double mfPct = 0;

        private double MaxFeePCT
        {
            get
            {
                if (mfPct == 0)
                    mfPct = ctrlRepo.GetControlByDescription("Calc - MaxFeePercentage").ControlNumeric ?? 0;

                return mfPct;
            }
        }

        private double buf = 0;

        private double Buffer
        {
            get
            {
                if (buf == 0)
                    buf = ctrlRepo.GetControlByDescription("Calc - FurtherLoanCapacity").ControlNumeric ?? 0;

                return buf;
            }
        }

        private double qpfPct = 0;

        private double QuickPayFeePCT
        {
            get
            {
                if (qpfPct == 0)
                    qpfPct = ctrlRepo.GetControlByDescription("Calc - QuickPayFeePercentage").ControlNumeric ?? 0;
                return qpfPct;
            }
        }

        private IControlRepository cR;

        private IControlRepository ctrlRepo
        {
            get
            {
                if (cR == null)
                    cR = RepositoryFactory.GetRepository<IControlRepository>();

                return cR;
            }
        }

        #endregion ControlValues

        //[Test]
        public void Perc80_Fee_R_150_Fica_as_at_1_Jan_2013_v2_xlsx_Test()
        {
            var feesTestData = new List<FeesData>();

            #region TestData

            var FeesData0 = new FeesData { FeeRange = 0, FeeLegalConveyancing = 0, FeeBondStamps = 0, FeeBondConveyancing = 0, FeeBondVAT = 0 }; feesTestData.Add(FeesData0);
            var FeesData20000 = new FeesData { FeeRange = 20000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData20000);
            var FeesData25000 = new FeesData { FeeRange = 25000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData25000);
            var FeesData30000 = new FeesData { FeeRange = 30000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData30000);
            var FeesData35000 = new FeesData { FeeRange = 35000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData35000);
            var FeesData40000 = new FeesData { FeeRange = 40000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData40000);
            var FeesData45000 = new FeesData { FeeRange = 45000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData45000);
            var FeesData50000 = new FeesData { FeeRange = 50000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData50000);
            var FeesData60000 = new FeesData { FeeRange = 60000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData60000);
            var FeesData70000 = new FeesData { FeeRange = 70000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData70000);
            var FeesData80000 = new FeesData { FeeRange = 80000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData80000);
            var FeesData90000 = new FeesData { FeeRange = 90000, FeeLegalConveyancing = 2500, FeeBondStamps = 1163.45, FeeBondConveyancing = 1550, FeeBondVAT = 217 }; feesTestData.Add(FeesData90000);
            var FeesData100000 = new FeesData { FeeRange = 100000, FeeLegalConveyancing = 2600, FeeBondStamps = 1163.45, FeeBondConveyancing = 1600, FeeBondVAT = 224 }; feesTestData.Add(FeesData100000);
            var FeesData110000 = new FeesData { FeeRange = 110000, FeeLegalConveyancing = 2600, FeeBondStamps = 1163.45, FeeBondConveyancing = 1600, FeeBondVAT = 224 }; feesTestData.Add(FeesData110000);
            var FeesData120000 = new FeesData { FeeRange = 120000, FeeLegalConveyancing = 2600, FeeBondStamps = 1163.45, FeeBondConveyancing = 1600, FeeBondVAT = 224 }; feesTestData.Add(FeesData120000);
            var FeesData125000 = new FeesData { FeeRange = 125000, FeeLegalConveyancing = 2700, FeeBondStamps = 1163.45, FeeBondConveyancing = 1650, FeeBondVAT = 231 }; feesTestData.Add(FeesData125000);
            var FeesData130000 = new FeesData { FeeRange = 130000, FeeLegalConveyancing = 2700, FeeBondStamps = 1163.45, FeeBondConveyancing = 1650, FeeBondVAT = 231 }; feesTestData.Add(FeesData130000);
            var FeesData140000 = new FeesData { FeeRange = 140000, FeeLegalConveyancing = 2700, FeeBondStamps = 1163.45, FeeBondConveyancing = 1650, FeeBondVAT = 231 }; feesTestData.Add(FeesData140000);
            var FeesData150000 = new FeesData { FeeRange = 150000, FeeLegalConveyancing = 2900, FeeBondStamps = 1203.45, FeeBondConveyancing = 1750, FeeBondVAT = 245 }; feesTestData.Add(FeesData150000);
            var FeesData160000 = new FeesData { FeeRange = 160000, FeeLegalConveyancing = 2900, FeeBondStamps = 1203.45, FeeBondConveyancing = 1750, FeeBondVAT = 245 }; feesTestData.Add(FeesData160000);
            var FeesData170000 = new FeesData { FeeRange = 170000, FeeLegalConveyancing = 2900, FeeBondStamps = 1203.45, FeeBondConveyancing = 1750, FeeBondVAT = 245 }; feesTestData.Add(FeesData170000);
            var FeesData175000 = new FeesData { FeeRange = 175000, FeeLegalConveyancing = 3000, FeeBondStamps = 1203.45, FeeBondConveyancing = 1800, FeeBondVAT = 252 }; feesTestData.Add(FeesData175000);
            var FeesData180000 = new FeesData { FeeRange = 180000, FeeLegalConveyancing = 3000, FeeBondStamps = 1203.45, FeeBondConveyancing = 1800, FeeBondVAT = 252 }; feesTestData.Add(FeesData180000);
            var FeesData190000 = new FeesData { FeeRange = 190000, FeeLegalConveyancing = 3000, FeeBondStamps = 1203.45, FeeBondConveyancing = 1800, FeeBondVAT = 252 }; feesTestData.Add(FeesData190000);
            var FeesData200000 = new FeesData { FeeRange = 200000, FeeLegalConveyancing = 3000, FeeBondStamps = 1203.45, FeeBondConveyancing = 1800, FeeBondVAT = 252 }; feesTestData.Add(FeesData200000);
            var FeesData210000 = new FeesData { FeeRange = 210000, FeeLegalConveyancing = 3300, FeeBondStamps = 1203.45, FeeBondConveyancing = 1950, FeeBondVAT = 273 }; feesTestData.Add(FeesData210000);
            var FeesData220000 = new FeesData { FeeRange = 220000, FeeLegalConveyancing = 3300, FeeBondStamps = 1203.45, FeeBondConveyancing = 1950, FeeBondVAT = 273 }; feesTestData.Add(FeesData220000);
            var FeesData230000 = new FeesData { FeeRange = 230000, FeeLegalConveyancing = 3300, FeeBondStamps = 1203.45, FeeBondConveyancing = 1950, FeeBondVAT = 273 }; feesTestData.Add(FeesData230000);
            var FeesData240000 = new FeesData { FeeRange = 240000, FeeLegalConveyancing = 3300, FeeBondStamps = 1203.45, FeeBondConveyancing = 1950, FeeBondVAT = 273 }; feesTestData.Add(FeesData240000);
            var FeesData250000 = new FeesData { FeeRange = 250000, FeeLegalConveyancing = 3700, FeeBondStamps = 1203.45, FeeBondConveyancing = 2150, FeeBondVAT = 301 }; feesTestData.Add(FeesData250000);
            var FeesData260000 = new FeesData { FeeRange = 260000, FeeLegalConveyancing = 3700, FeeBondStamps = 1203.45, FeeBondConveyancing = 2150, FeeBondVAT = 301 }; feesTestData.Add(FeesData260000);
            var FeesData270000 = new FeesData { FeeRange = 270000, FeeLegalConveyancing = 3700, FeeBondStamps = 1203.45, FeeBondConveyancing = 2150, FeeBondVAT = 301 }; feesTestData.Add(FeesData270000);
            var FeesData280000 = new FeesData { FeeRange = 280000, FeeLegalConveyancing = 3700, FeeBondStamps = 1203.45, FeeBondConveyancing = 2150, FeeBondVAT = 301 }; feesTestData.Add(FeesData280000);
            var FeesData290000 = new FeesData { FeeRange = 290000, FeeLegalConveyancing = 3700, FeeBondStamps = 1203.45, FeeBondConveyancing = 2150, FeeBondVAT = 301 }; feesTestData.Add(FeesData290000);
            var FeesData300000 = new FeesData { FeeRange = 300000, FeeLegalConveyancing = 3700, FeeBondStamps = 1303.45, FeeBondConveyancing = 2150, FeeBondVAT = 301 }; feesTestData.Add(FeesData300000);
            var FeesData310000 = new FeesData { FeeRange = 310000, FeeLegalConveyancing = 4100, FeeBondStamps = 1303.45, FeeBondConveyancing = 2350, FeeBondVAT = 329 }; feesTestData.Add(FeesData310000);
            var FeesData320000 = new FeesData { FeeRange = 320000, FeeLegalConveyancing = 4100, FeeBondStamps = 1303.45, FeeBondConveyancing = 2350, FeeBondVAT = 329 }; feesTestData.Add(FeesData320000);
            var FeesData330000 = new FeesData { FeeRange = 330000, FeeLegalConveyancing = 4100, FeeBondStamps = 1303.45, FeeBondConveyancing = 2350, FeeBondVAT = 329 }; feesTestData.Add(FeesData330000);
            var FeesData340000 = new FeesData { FeeRange = 340000, FeeLegalConveyancing = 4100, FeeBondStamps = 1303.45, FeeBondConveyancing = 2350, FeeBondVAT = 329 }; feesTestData.Add(FeesData340000);
            var FeesData350000 = new FeesData { FeeRange = 350000, FeeLegalConveyancing = 4600, FeeBondStamps = 1303.45, FeeBondConveyancing = 2600, FeeBondVAT = 364 }; feesTestData.Add(FeesData350000);
            var FeesData360000 = new FeesData { FeeRange = 360000, FeeLegalConveyancing = 4600, FeeBondStamps = 1303.45, FeeBondConveyancing = 2600, FeeBondVAT = 364 }; feesTestData.Add(FeesData360000);
            var FeesData370000 = new FeesData { FeeRange = 370000, FeeLegalConveyancing = 4600, FeeBondStamps = 1303.45, FeeBondConveyancing = 2600, FeeBondVAT = 364 }; feesTestData.Add(FeesData370000);
            var FeesData380000 = new FeesData { FeeRange = 380000, FeeLegalConveyancing = 4600, FeeBondStamps = 1303.45, FeeBondConveyancing = 2600, FeeBondVAT = 364 }; feesTestData.Add(FeesData380000);
            var FeesData390000 = new FeesData { FeeRange = 390000, FeeLegalConveyancing = 4600, FeeBondStamps = 1303.45, FeeBondConveyancing = 2600, FeeBondVAT = 364 }; feesTestData.Add(FeesData390000);
            var FeesData400000 = new FeesData { FeeRange = 400000, FeeLegalConveyancing = 4600, FeeBondStamps = 1303.45, FeeBondConveyancing = 2600, FeeBondVAT = 364 }; feesTestData.Add(FeesData400000);
            var FeesData410000 = new FeesData { FeeRange = 410000, FeeLegalConveyancing = 5000, FeeBondStamps = 1303.45, FeeBondConveyancing = 2800, FeeBondVAT = 392 }; feesTestData.Add(FeesData410000);
            var FeesData420000 = new FeesData { FeeRange = 420000, FeeLegalConveyancing = 5000, FeeBondStamps = 1303.45, FeeBondConveyancing = 2800, FeeBondVAT = 392 }; feesTestData.Add(FeesData420000);
            var FeesData430000 = new FeesData { FeeRange = 430000, FeeLegalConveyancing = 5000, FeeBondStamps = 1303.45, FeeBondConveyancing = 2800, FeeBondVAT = 392 }; feesTestData.Add(FeesData430000);
            var FeesData440000 = new FeesData { FeeRange = 440000, FeeLegalConveyancing = 5000, FeeBondStamps = 1303.45, FeeBondConveyancing = 2800, FeeBondVAT = 392 }; feesTestData.Add(FeesData440000);
            var FeesData450000 = new FeesData { FeeRange = 450000, FeeLegalConveyancing = 5400, FeeBondStamps = 1303.45, FeeBondConveyancing = 3000, FeeBondVAT = 420 }; feesTestData.Add(FeesData450000);
            var FeesData460000 = new FeesData { FeeRange = 460000, FeeLegalConveyancing = 5400, FeeBondStamps = 1303.45, FeeBondConveyancing = 3000, FeeBondVAT = 420 }; feesTestData.Add(FeesData460000);
            var FeesData470000 = new FeesData { FeeRange = 470000, FeeLegalConveyancing = 5400, FeeBondStamps = 1303.45, FeeBondConveyancing = 3000, FeeBondVAT = 420 }; feesTestData.Add(FeesData470000);
            var FeesData480000 = new FeesData { FeeRange = 480000, FeeLegalConveyancing = 5400, FeeBondStamps = 1303.45, FeeBondConveyancing = 3000, FeeBondVAT = 420 }; feesTestData.Add(FeesData480000);
            var FeesData490000 = new FeesData { FeeRange = 490000, FeeLegalConveyancing = 5400, FeeBondStamps = 1303.45, FeeBondConveyancing = 3000, FeeBondVAT = 420 }; feesTestData.Add(FeesData490000);
            var FeesData500000 = new FeesData { FeeRange = 500000, FeeLegalConveyancing = 5400, FeeBondStamps = 1303.45, FeeBondConveyancing = 3000, FeeBondVAT = 420 }; feesTestData.Add(FeesData500000);
            var FeesData510000 = new FeesData { FeeRange = 510000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData510000);
            var FeesData520000 = new FeesData { FeeRange = 520000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData520000);
            var FeesData530000 = new FeesData { FeeRange = 530000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData530000);
            var FeesData540000 = new FeesData { FeeRange = 540000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData540000);
            var FeesData550000 = new FeesData { FeeRange = 550000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData550000);
            var FeesData560000 = new FeesData { FeeRange = 560000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData560000);
            var FeesData570000 = new FeesData { FeeRange = 570000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData570000);
            var FeesData580000 = new FeesData { FeeRange = 580000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData580000);
            var FeesData590000 = new FeesData { FeeRange = 590000, FeeLegalConveyancing = 6200, FeeBondStamps = 1303.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData590000);
            var FeesData600000 = new FeesData { FeeRange = 600000, FeeLegalConveyancing = 6200, FeeBondStamps = 1503.45, FeeBondConveyancing = 3400, FeeBondVAT = 476 }; feesTestData.Add(FeesData600000);
            var FeesData610000 = new FeesData { FeeRange = 610000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData610000);
            var FeesData620000 = new FeesData { FeeRange = 620000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData620000);
            var FeesData630000 = new FeesData { FeeRange = 630000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData630000);
            var FeesData640000 = new FeesData { FeeRange = 640000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData640000);
            var FeesData650000 = new FeesData { FeeRange = 650000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData650000);
            var FeesData660000 = new FeesData { FeeRange = 660000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData660000);
            var FeesData670000 = new FeesData { FeeRange = 670000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData670000);
            var FeesData680000 = new FeesData { FeeRange = 680000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData680000);
            var FeesData690000 = new FeesData { FeeRange = 690000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData690000);
            var FeesData700000 = new FeesData { FeeRange = 700000, FeeLegalConveyancing = 7000, FeeBondStamps = 1503.45, FeeBondConveyancing = 3800, FeeBondVAT = 532 }; feesTestData.Add(FeesData700000);
            var FeesData710000 = new FeesData { FeeRange = 710000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData710000);
            var FeesData720000 = new FeesData { FeeRange = 720000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData720000);
            var FeesData730000 = new FeesData { FeeRange = 730000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData730000);
            var FeesData740000 = new FeesData { FeeRange = 740000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData740000);
            var FeesData750000 = new FeesData { FeeRange = 750000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData750000);
            var FeesData760000 = new FeesData { FeeRange = 760000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData760000);
            var FeesData770000 = new FeesData { FeeRange = 770000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData770000);
            var FeesData780000 = new FeesData { FeeRange = 780000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData780000);
            var FeesData790000 = new FeesData { FeeRange = 790000, FeeLegalConveyancing = 7800, FeeBondStamps = 1503.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData790000);
            var FeesData800000 = new FeesData { FeeRange = 800000, FeeLegalConveyancing = 7800, FeeBondStamps = 1603.45, FeeBondConveyancing = 4200, FeeBondVAT = 588 }; feesTestData.Add(FeesData800000);
            var FeesData810000 = new FeesData { FeeRange = 810000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData810000);
            var FeesData820000 = new FeesData { FeeRange = 820000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData820000);
            var FeesData830000 = new FeesData { FeeRange = 830000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData830000);
            var FeesData840000 = new FeesData { FeeRange = 840000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData840000);
            var FeesData850000 = new FeesData { FeeRange = 850000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData850000);
            var FeesData860000 = new FeesData { FeeRange = 860000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData860000);
            var FeesData870000 = new FeesData { FeeRange = 870000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData870000);
            var FeesData880000 = new FeesData { FeeRange = 880000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData880000);
            var FeesData890000 = new FeesData { FeeRange = 890000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData890000);
            var FeesData900000 = new FeesData { FeeRange = 900000, FeeLegalConveyancing = 8600, FeeBondStamps = 1603.45, FeeBondConveyancing = 4600, FeeBondVAT = 644 }; feesTestData.Add(FeesData900000);
            var FeesData910000 = new FeesData { FeeRange = 910000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData910000);
            var FeesData920000 = new FeesData { FeeRange = 920000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData920000);
            var FeesData930000 = new FeesData { FeeRange = 930000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData930000);
            var FeesData940000 = new FeesData { FeeRange = 940000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData940000);
            var FeesData950000 = new FeesData { FeeRange = 950000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData950000);
            var FeesData960000 = new FeesData { FeeRange = 960000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData960000);
            var FeesData970000 = new FeesData { FeeRange = 970000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData970000);
            var FeesData980000 = new FeesData { FeeRange = 980000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData980000);
            var FeesData990000 = new FeesData { FeeRange = 990000, FeeLegalConveyancing = 9400, FeeBondStamps = 1603.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData990000);
            var FeesData1000000 = new FeesData { FeeRange = 1000000, FeeLegalConveyancing = 9400, FeeBondStamps = 1703.45, FeeBondConveyancing = 5000, FeeBondVAT = 700 }; feesTestData.Add(FeesData1000000);
            var FeesData1100000 = new FeesData { FeeRange = 1100000, FeeLegalConveyancing = 9800, FeeBondStamps = 1703.45, FeeBondConveyancing = 5200, FeeBondVAT = 728 }; feesTestData.Add(FeesData1100000);
            var FeesData1200000 = new FeesData { FeeRange = 1200000, FeeLegalConveyancing = 10200, FeeBondStamps = 1703.45, FeeBondConveyancing = 5400, FeeBondVAT = 756 }; feesTestData.Add(FeesData1200000);
            var FeesData1300000 = new FeesData { FeeRange = 1300000, FeeLegalConveyancing = 10600, FeeBondStamps = 1703.45, FeeBondConveyancing = 5600, FeeBondVAT = 784 }; feesTestData.Add(FeesData1300000);
            var FeesData1400000 = new FeesData { FeeRange = 1400000, FeeLegalConveyancing = 11000, FeeBondStamps = 1703.45, FeeBondConveyancing = 5800, FeeBondVAT = 812 }; feesTestData.Add(FeesData1400000);
            var FeesData1500000 = new FeesData { FeeRange = 1500000, FeeLegalConveyancing = 11400, FeeBondStamps = 1703.45, FeeBondConveyancing = 6000, FeeBondVAT = 840 }; feesTestData.Add(FeesData1500000);
            var FeesData1600000 = new FeesData { FeeRange = 1600000, FeeLegalConveyancing = 11800, FeeBondStamps = 1703.45, FeeBondConveyancing = 6200, FeeBondVAT = 868 }; feesTestData.Add(FeesData1600000);
            var FeesData1700000 = new FeesData { FeeRange = 1700000, FeeLegalConveyancing = 12200, FeeBondStamps = 1703.45, FeeBondConveyancing = 6400, FeeBondVAT = 896 }; feesTestData.Add(FeesData1700000);
            var FeesData1800000 = new FeesData { FeeRange = 1800000, FeeLegalConveyancing = 12600, FeeBondStamps = 1703.45, FeeBondConveyancing = 6600, FeeBondVAT = 924 }; feesTestData.Add(FeesData1800000);
            var FeesData1900000 = new FeesData { FeeRange = 1900000, FeeLegalConveyancing = 13000, FeeBondStamps = 1703.45, FeeBondConveyancing = 6800, FeeBondVAT = 952 }; feesTestData.Add(FeesData1900000);
            var FeesData2000000 = new FeesData { FeeRange = 2000000, FeeLegalConveyancing = 13400, FeeBondStamps = 1803.45, FeeBondConveyancing = 7000, FeeBondVAT = 980 }; feesTestData.Add(FeesData2000000);
            var FeesData2100000 = new FeesData { FeeRange = 2100000, FeeLegalConveyancing = 13800, FeeBondStamps = 1803.45, FeeBondConveyancing = 7200, FeeBondVAT = 1008 }; feesTestData.Add(FeesData2100000);
            var FeesData2200000 = new FeesData { FeeRange = 2200000, FeeLegalConveyancing = 14200, FeeBondStamps = 1803.45, FeeBondConveyancing = 7400, FeeBondVAT = 1036 }; feesTestData.Add(FeesData2200000);
            var FeesData2300000 = new FeesData { FeeRange = 2300000, FeeLegalConveyancing = 14600, FeeBondStamps = 1803.45, FeeBondConveyancing = 7600, FeeBondVAT = 1064 }; feesTestData.Add(FeesData2300000);
            var FeesData2400000 = new FeesData { FeeRange = 2400000, FeeLegalConveyancing = 15000, FeeBondStamps = 1803.45, FeeBondConveyancing = 7800, FeeBondVAT = 1092 }; feesTestData.Add(FeesData2400000);
            var FeesData2500000 = new FeesData { FeeRange = 2500000, FeeLegalConveyancing = 15400, FeeBondStamps = 1803.45, FeeBondConveyancing = 8000, FeeBondVAT = 1120 }; feesTestData.Add(FeesData2500000);
            var FeesData2600000 = new FeesData { FeeRange = 2600000, FeeLegalConveyancing = 15800, FeeBondStamps = 1803.45, FeeBondConveyancing = 8200, FeeBondVAT = 1148 }; feesTestData.Add(FeesData2600000);
            var FeesData2700000 = new FeesData { FeeRange = 2700000, FeeLegalConveyancing = 16200, FeeBondStamps = 1803.45, FeeBondConveyancing = 8400, FeeBondVAT = 1176 }; feesTestData.Add(FeesData2700000);
            var FeesData2800000 = new FeesData { FeeRange = 2800000, FeeLegalConveyancing = 16600, FeeBondStamps = 1803.45, FeeBondConveyancing = 8600, FeeBondVAT = 1204 }; feesTestData.Add(FeesData2800000);
            var FeesData2900000 = new FeesData { FeeRange = 2900000, FeeLegalConveyancing = 17000, FeeBondStamps = 1803.45, FeeBondConveyancing = 8800, FeeBondVAT = 1232 }; feesTestData.Add(FeesData2900000);
            var FeesData3000000 = new FeesData { FeeRange = 3000000, FeeLegalConveyancing = 17400, FeeBondStamps = 1803.45, FeeBondConveyancing = 9000, FeeBondVAT = 1260 }; feesTestData.Add(FeesData3000000);
            var FeesData3500000 = new FeesData { FeeRange = 3500000, FeeLegalConveyancing = 17800, FeeBondStamps = 1803.45, FeeBondConveyancing = 9200, FeeBondVAT = 1288 }; feesTestData.Add(FeesData3500000);
            var FeesData4000000 = new FeesData { FeeRange = 4000000, FeeLegalConveyancing = 18200, FeeBondStamps = 1953.45, FeeBondConveyancing = 9400, FeeBondVAT = 1316 }; feesTestData.Add(FeesData4000000);
            var FeesData4500000 = new FeesData { FeeRange = 4500000, FeeLegalConveyancing = 18600, FeeBondStamps = 1953.45, FeeBondConveyancing = 9600, FeeBondVAT = 1344 }; feesTestData.Add(FeesData4500000);
            var FeesData5000000 = new FeesData { FeeRange = 5000000, FeeLegalConveyancing = 19000, FeeBondStamps = 1953.45, FeeBondConveyancing = 9800, FeeBondVAT = 1372 }; feesTestData.Add(FeesData5000000);
            var FeesData5500000 = new FeesData { FeeRange = 5500000, FeeLegalConveyancing = 20000, FeeBondStamps = 1953.45, FeeBondConveyancing = 10300, FeeBondVAT = 1442 }; feesTestData.Add(FeesData5500000);
            var FeesData6000000 = new FeesData { FeeRange = 6000000, FeeLegalConveyancing = 21000, FeeBondStamps = 2353.45, FeeBondConveyancing = 10800, FeeBondVAT = 1512 }; feesTestData.Add(FeesData6000000);
            var FeesData6500000 = new FeesData { FeeRange = 6500000, FeeLegalConveyancing = 22000, FeeBondStamps = 2353.45, FeeBondConveyancing = 11300, FeeBondVAT = 1582 }; feesTestData.Add(FeesData6500000);
            var FeesData7000000 = new FeesData { FeeRange = 7000000, FeeLegalConveyancing = 23000, FeeBondStamps = 2353.45, FeeBondConveyancing = 11800, FeeBondVAT = 1652 }; feesTestData.Add(FeesData7000000);
            var FeesData7500000 = new FeesData { FeeRange = 7500000, FeeLegalConveyancing = 24000, FeeBondStamps = 2353.45, FeeBondConveyancing = 12300, FeeBondVAT = 1722 }; feesTestData.Add(FeesData7500000);
            var FeesData8000000 = new FeesData { FeeRange = 8000000, FeeLegalConveyancing = 25000, FeeBondStamps = 2353.45, FeeBondConveyancing = 12800, FeeBondVAT = 1792 }; feesTestData.Add(FeesData8000000);
            var FeesData8500000 = new FeesData { FeeRange = 8500000, FeeLegalConveyancing = 26000, FeeBondStamps = 2353.45, FeeBondConveyancing = 13300, FeeBondVAT = 1862 }; feesTestData.Add(FeesData8500000);
            var FeesData9000000 = new FeesData { FeeRange = 9000000, FeeLegalConveyancing = 27000, FeeBondStamps = 2353.45, FeeBondConveyancing = 13800, FeeBondVAT = 1932 }; feesTestData.Add(FeesData9000000);
            var FeesData9500000 = new FeesData { FeeRange = 9500000, FeeLegalConveyancing = 28000, FeeBondStamps = 2353.45, FeeBondConveyancing = 14300, FeeBondVAT = 2002 }; feesTestData.Add(FeesData9500000);
            var FeesData10000000 = new FeesData { FeeRange = 10000000, FeeLegalConveyancing = 29000, FeeBondStamps = 2853.45, FeeBondConveyancing = 14800, FeeBondVAT = 2072 }; feesTestData.Add(FeesData10000000);

            #endregion TestData

            foreach (var feesTest in feesTestData)
            {
                DataRow dr = GetFeeRow(feesTest.FeeRange);
                Assert.AreEqual(feesTest.FeeRange, dr["FeeRange"]);
                Assert.AreEqual(feesTest.FeeLegalConveyancing, dr["FeeLegalConveyancing"]);
                Assert.AreEqual(feesTest.FeeBondStamps, dr["FeeBondStamps"]);
                Assert.AreEqual(feesTest.FeeBondConveyancing, dr["FeeBondConveyancing"]);
                Assert.AreEqual(feesTest.FeeBondVAT, dr["FeeBondVAT"]);
            }
        }

        public class FeesData
        {
            public int FeeRange { get; set; }

            public double FeeLegalConveyancing { get; set; }

            public double FeeBondStamps { get; set; }

            public double FeeBondConveyancing80Pct { get; set; }

            public double FeeBondVAT80Pct { get; set; }

            public double FeeBondConveyancing { get; set; }

            public double FeeBondVAT { get; set; }
        }
    }
}