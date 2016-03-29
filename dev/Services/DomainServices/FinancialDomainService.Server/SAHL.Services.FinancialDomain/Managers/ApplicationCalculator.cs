using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using SAHL.Shared.BusinessModel.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers
{
    public class ApplicationCalculator : IApplicationCalculator
    {
        private IFinancialDataManager financialDataManager;
        private ILoanCalculations functionsUtils;

        public ApplicationCalculator(IFinancialDataManager FinancialDataManager, ILoanCalculations functionsUtils)
        {
            this.financialDataManager = FinancialDataManager;
            this.functionsUtils = functionsUtils;
        }

        public PricedMortgageLoanApplicationInformationModel PriceApplication(MortgageLoanApplicationInformationModel applicationInfo, OriginationFeesModel originationFees)
        {
            var loanAmountNoFees = applicationInfo.LoanAmountNoFees + originationFees.InterimInterest;
            var totalLoanAmount = loanAmountNoFees;
            if (originationFees.CapitaliseFees)
            {
                totalLoanAmount += originationFees.TotalFees();
            }

            var ltv = functionsUtils.CalculateLTV(totalLoanAmount, applicationInfo.PropertyValuation);
            var ltvPercentage = ltv * 100; 

            var creditCriteria = financialDataManager.DetermineCreditCriteria(applicationInfo.MortgageLoanPurpose,
                (applicationInfo.EmploymentType.HasValue && applicationInfo.EmploymentType.Value != EmploymentType.Unknown) 
                ? applicationInfo.EmploymentType.Value : EmploymentType.Salaried, 
                totalLoanAmount, ltvPercentage, applicationInfo.OriginationSource, 
                Product.NewVariableLoan, applicationInfo.HouseholdIncome);
            var pricedCreditCriteria = new PricedCreditCriteriaModel(creditCriteria.CreditCriteriaKey, creditCriteria.CreditMatrixKey, creditCriteria.CategoryKey);

            var rateConfiguration = financialDataManager.GetRateConfigurationValues(creditCriteria.MarginKey, (int)MarketRates.ThreeMonthJIBARRounded);
            var interestRate = rateConfiguration.MarketRateValue + rateConfiguration.MarginValue;
            var monthlyInstalment = functionsUtils.CalculateInstalment(totalLoanAmount, interestRate, applicationInfo.Term, false);

            var pti = functionsUtils.CalculatePTI(monthlyInstalment, applicationInfo.HouseholdIncome);

            return new PricedMortgageLoanApplicationInformationModel(totalLoanAmount, loanAmountNoFees, ltv, rateConfiguration, monthlyInstalment, pti, pricedCreditCriteria);
        }
    }
}