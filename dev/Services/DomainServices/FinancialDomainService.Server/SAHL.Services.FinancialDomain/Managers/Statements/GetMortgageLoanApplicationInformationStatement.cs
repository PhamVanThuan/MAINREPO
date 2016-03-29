using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetMortgageLoanApplicationInformationStatement : ISqlStatement<MortgageLoanApplicationInformationModel>
    {
        public int ApplicationInformationKey { get; protected set; }

        public GetMortgageLoanApplicationInformationStatement(int applicationInformationKey)
        {
            this.ApplicationInformationKey = applicationInformationKey;
        }

        public string GetStatement()
        {
            return @" SELECT
                   Cast(oivl.LoanAmountNoFees as decimal(22, 10)) as LoanAmountNoFees
                 , Cast(oivl.BondToRegister as decimal(22, 10)) as BondToRegister
                 , o.OfferTypeKey as OfferTypeKey                 
                 , Cast(oivl.RequestedCashAmount as decimal(22, 10)) as RequestedCashAmount
                 , Cast(oivl.HouseholdIncome as decimal(22, 10)) as HouseholdIncome
                 , oivl.EmploymentTypeKey as EmploymentTypeKey
                 , Cast(oivl.PropertyValuation as decimal(22, 10)) as PropertyValuation                 
                 , oml.MortgageLoanPurposeKey as MortgageLoanPurposeKey
                 , o.OriginationSourceKey as OriginationSourceKey
                 , oivl.Term as Term
            FROM [2AM].dbo.Offer o
            JOIN [2AM].dbo.OfferMortgageLoan oml on oml.OfferKey = o.OfferKey
            JOIN [2AM].dbo.OfferInformation oi on o.OfferKey = oi.OfferKey
            JOIN [2AM].dbo.OfferInformationVariableLoan oivl on oivl.OfferInformationKey = oi.OfferInformationKey
            WHERE oi.OfferInformationKey = @ApplicationInformationKey";
        }
    }
}