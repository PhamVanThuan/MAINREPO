using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    internal class when_calculating_origination_fees : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static decimal loanAmount;
        private static decimal bondRequired;
        private static OfferType offerType;
        private static decimal cashOut;
        private static decimal overRideCancelFee;
        private static bool capitaliseFees;
        private static bool isQuickPay;
        private static decimal householdIncome;
        private static EmploymentType employmentType;
        private static decimal ltv;
        private static bool isStaffLoan;
        private static bool isGEPF;
        private static bool isDiscountedInitiationFee;
        private static bool capitaliseInitiationFee;
        private static DateTime offerStartDate;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            loanAmount = 1000000;
            bondRequired = 1100000;
            offerType = OfferType.NewPurchaseLoan;
            cashOut = 0;
            overRideCancelFee = 0;
            capitaliseFees = false;
            isQuickPay = false;
            householdIncome = 100000;
            employmentType = EmploymentType.Salaried;
            ltv = 0.9m;
            isStaffLoan = false;
            isGEPF = false;
            isDiscountedInitiationFee = false;
            capitaliseInitiationFee = false;
            offerStartDate = DateTime.Now;
        };

        private Because of = () =>
        {
            financialDataManager.CalculateOriginationFees(loanAmount, bondRequired, offerType, cashOut, overRideCancelFee, capitaliseFees, isQuickPay, householdIncome, employmentType, ltv, isStaffLoan, isDiscountedInitiationFee, offerStartDate, capitaliseInitiationFee, isGEPF);
        };

        private It should_calculate_origination_fees = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<OriginationFeesModel>(Arg.Is<CalculateOriginationFeesStatement>(
                y => y.LoanAmount == loanAmount
                    && y.BondRequired == bondRequired
                    && y.CapitaliseFees == capitaliseFees
                    && y.CashOut == cashOut
                    && y.EmploymentTypeKey == (int)employmentType
                    && y.HouseholdIncome == householdIncome
                    && y.IsDiscountedInitiationFee == isDiscountedInitiationFee
                    && y.IsStaffLoan == isStaffLoan
                    && y.LoanType == (int)offerType
                    && y.LTV == ltv
                    && y.OverRideCancelFee == overRideCancelFee
                    && y.QuickPay == isQuickPay
            )));
        };
    }
}