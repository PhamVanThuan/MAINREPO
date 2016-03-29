using SAHL.Common.Globals;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Interfaces.DecisionTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Model
{
    public class QualifyApplicationFor30YearLoanTermQuery
    {
        public QualifyApplicationFor30YearLoanTermQuery(bool disqualifiedByCredit, double ltv, double pti, double householdIncome, double marketRate, double totalRateAdjustments, double linkRate, double loanAmount, OfferTypes applicationType, Products product, double propertyValue, bool isAlphaHousingApplication, QualifyHighestIncomeContributorFor30YearLoanTermQuery highestIncomeContributor, bool isInterestOnly)
        {
            this.DisqualifiedByCredit = disqualifiedByCredit;
            this.LTV = ltv;
            this.PTI = pti;
            this.HouseholdIncome = householdIncome;
            this.MarketRate = marketRate;
            this.TotalRateAdjustments = totalRateAdjustments;
            this.LinkRate = linkRate;
            this.LoanAmount = loanAmount;
            this.ApplicationType = GetDecisionTreeEnum(applicationType);
            this.Product = GetDecisionTreeEnum(product);
            this.PropertyValue = propertyValue;
            this.IsAlphaHousingApplication = isAlphaHousingApplication;
            this.HighestIncomeContributor = highestIncomeContributor;
            this.EffectiveRate = MarketRate + LinkRate + TotalRateAdjustments;
            this.IsInterestOnly = isInterestOnly;
            this.Messages = SystemMessageCollection.Empty();
        }

        private string GetDecisionTreeEnum(OfferTypes applicationType)
        {
            string result = String.Empty;
            switch (applicationType)
            {
                case OfferTypes.SwitchLoan:
                    result = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().Switch;
                    break;
                case OfferTypes.NewPurchaseLoan:
                    result = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().NewPurchase;
                    break;
                default:
                    result = String.Empty;
                    break;
            }

            return result;
        }

        private string GetDecisionTreeEnum(Products product)
        {
            string result = String.Empty;

            switch (product)
            {
                case Products.NewVariableLoan:
                    result = new Enumerations.SAHomeLoans.MortgageLoanProductType().NewVariable;
                    break;
                case Products.VariableLoan:
                    result = new Enumerations.SAHomeLoans.MortgageLoanProductType().Variable;
                    break;
                case Products.Edge:
                    result = new Enumerations.SAHomeLoans.MortgageLoanProductType().Edge;
                    break;
                case Products.SuperLo:
                    result = new Enumerations.SAHomeLoans.MortgageLoanProductType().SuperLo;
                    break;
                default:
                    result = String.Empty;
                    break;
            }

            return result;
        }

        public bool DisqualifiedByCredit { get; private set; }
        public double LTV { get; private set; }
        public double PTI { get; private set; }
        public double HouseholdIncome { get; private set; }
        public double MarketRate { get; private set; }
        public double TotalRateAdjustments { get; private set; }
        public double LinkRate { get; private set; }
        public double EffectiveRate { get; private set; }
        public double LoanAmount { get; private set; }
        public bool IsAlphaHousingApplication { get; set; }
        public string ApplicationType { get; private set; }
        public string Product { get; private set; }
        public double PropertyValue { get; private set; }
        public bool IsInterestOnly { get; private set; }

        public QualifyHighestIncomeContributorFor30YearLoanTermQuery HighestIncomeContributor { get; private set; }

        public ISystemMessageCollection Messages { get; private set; }
        public ThirtyYearMortgageLoanEligibility_QueryResult Result { get; private set; }

        public void SetTreeResult(ThirtyYearMortgageLoanEligibility_QueryResult result, ISystemMessageCollection messages)
        {
            this.Messages = messages;
            this.Result = result;
        }
    }
}
