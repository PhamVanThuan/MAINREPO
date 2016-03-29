using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetOfferInformationVariableLoanStatement : ISqlStatement<OfferInformationVariableLoanDataModel>
    {
        public int ApplicationInformationKey { get; protected set; }

        public GetOfferInformationVariableLoanStatement(int applicationInformationKey)
        {
            this.ApplicationInformationKey = applicationInformationKey;
        }

        public string GetStatement()
        {
            return @"SELECT oivl.[OfferInformationKey]
      ,oivl.[CategoryKey]
      ,oivl.[Term]
      ,oivl.[ExistingLoan]
      ,oivl.[CashDeposit]
      ,oivl.[PropertyValuation]
      ,oivl.[HouseholdIncome]
      ,oivl.[FeesTotal]
      ,oivl.[InterimInterest]
      ,oivl.[MonthlyInstalment]
      ,oivl.[LifePremium]
      ,oivl.[HOCPremium]
      ,oivl.[MinLoanRequired]
      ,oivl.[MinBondRequired]
      ,oivl.[PreApprovedAmount]
      ,oivl.[MinCashAllowed]
      ,oivl.[MaxCashAllowed]
      ,oivl.[LoanAmountNoFees]
      ,oivl.[RequestedCashAmount]
      ,oivl.[LoanAgreementAmount]
      ,oivl.[BondToRegister]
      ,oivl.[LTV]
      ,oivl.[PTI]
      ,oivl.[MarketRate]
      ,oivl.[SPVKey]
      ,oivl.[EmploymentTypeKey]
      ,oivl.[RateConfigurationKey]
      ,oivl.[CreditMatrixKey]
      ,oivl.[CreditCriteriaKey]
      ,oivl.[AppliedInitiationFeeDiscount]
  FROM [2AM].dbo.OfferInformationVariableLoan oivl
WHERE oivl.OfferInformationKey = @ApplicationInformationKey";
        }
    }
}