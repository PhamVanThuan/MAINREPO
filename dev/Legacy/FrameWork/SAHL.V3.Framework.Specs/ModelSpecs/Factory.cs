using SAHL.Common.Globals;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.V3.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.ModelSpecs
{
    public static class Factory
    {
        
        public static Products GetDefaultProduct()
        {
            return Products.NewVariableLoan;
        }

        public static OfferTypes GetDefaultOfferType()
        {
            return OfferTypes.NewPurchaseLoan;
        }

        public static EmploymentTypes GetDefaultEmploymentType()
        {
            return EmploymentTypes.Salaried;
        }

        public static QualifyApplicationFor30YearLoanTermQuery GetDefaultQualifyApplicationFor30YearLoanTermQueryModel()
        {
            return new QualifyApplicationFor30YearLoanTermQuery(false, 0.5, 0.22, 50000, 0.06, 0.002, 0.023, 350000, GetDefaultOfferType(), GetDefaultProduct(), 500000, false, GetDefaultQualifyHighestIncomeContributorFor30YearLoanTermQueryModel(),false);
        }

        public static QualifyHighestIncomeContributorFor30YearLoanTermQuery GetDefaultQualifyHighestIncomeContributorFor30YearLoanTermQueryModel()
        {
            return new QualifyHighestIncomeContributorFor30YearLoanTermQuery(40, 695, "0000000000000", "Highest Income Contributor", GetDefaultEmploymentType());
        }

        public static ThirtyYearMortgageLoanEligibility_Query GetDefaultDecisionTreeThirtyYearMortgageLoanEligibility_Query()
        {
            var queryModel = GetDefaultQualifyApplicationFor30YearLoanTermQueryModel();

            return new ThirtyYearMortgageLoanEligibility_Query(queryModel.DisqualifiedByCredit,
                        queryModel.Product.ToString(),
                        queryModel.HighestIncomeContributor.SalaryType.ToString(),
                        queryModel.HighestIncomeContributor.Age,
                        queryModel.HouseholdIncome,
                        queryModel.HighestIncomeContributor.CreditScore,
                        queryModel.LTV,
                        queryModel.PTI,
                        queryModel.ApplicationType.ToString(),
                        queryModel.LoanAmount,
                        queryModel.PropertyValue,
                        queryModel.EffectiveRate,
                        queryModel.HighestIncomeContributor.FullName,
                        queryModel.HighestIncomeContributor.IdNumber,
                        queryModel.IsAlphaHousingApplication,
                        queryModel.IsInterestOnly,
                        null);
        }

        public static ThirtyYearMortgageLoanEligibility_QueryResult GetDefaultDecisionTreeThirtyYearMortgageLoanEligibility_QueryResult()
        {
            var queryResult = new ThirtyYearMortgageLoanEligibility_QueryResult();

            queryResult.InstalmentThirtyYear = 5000D;
            queryResult.InterestRateThirtyYear = 0.095;
            queryResult.LoantoValueThirtyYear = 0.6;
            queryResult.PaymenttoIncomeThirtyYear = 0.2;
            queryResult.QualifiesForThirtyYearLoanTerm = true;
            queryResult.PricingAdjustmentThirtyYear = 0.003;

            return queryResult;
        }

        public static ThirtyYearMortgageLoanEligibility_QueryResult GetFailedDecisionTreeThirtyYearMortgageLoanEligibility_QueryResult()
        {
            var queryResult = new ThirtyYearMortgageLoanEligibility_QueryResult();

            queryResult.InstalmentThirtyYear = -1;
            queryResult.InterestRateThirtyYear = -1;
            queryResult.LoantoValueThirtyYear = -1;
            queryResult.PaymenttoIncomeThirtyYear = -1;
            queryResult.PricingAdjustmentThirtyYear = -1;
            queryResult.QualifiesForThirtyYearLoanTerm = false;

            return queryResult;
        }
    }
}
