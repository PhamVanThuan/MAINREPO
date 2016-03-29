using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class CapitecCreditCriteriaDataModel :  IDataModel
    {
        public CapitecCreditCriteriaDataModel(int creditCriteriaKey, string newBusinessIndicator, double marginValue, double maxLoanAmount, double lTV, int employmentTypeKey, double minIncomeAmount, double? maxIncomeAmount, int categoryKey, int mortgageLoanPurposeKey)
        {
            this.CreditCriteriaKey = creditCriteriaKey;
            this.NewBusinessIndicator = newBusinessIndicator;
            this.MarginValue = marginValue;
            this.MaxLoanAmount = maxLoanAmount;
            this.LTV = lTV;
            this.EmploymentTypeKey = employmentTypeKey;
            this.MinIncomeAmount = minIncomeAmount;
            this.MaxIncomeAmount = maxIncomeAmount;
            this.CategoryKey = categoryKey;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
		
        }		

        public int CreditCriteriaKey { get; set; }

        public string NewBusinessIndicator { get; set; }

        public double MarginValue { get; set; }

        public double MaxLoanAmount { get; set; }

        public double LTV { get; set; }

        public int EmploymentTypeKey { get; set; }

        public double MinIncomeAmount { get; set; }

        public double? MaxIncomeAmount { get; set; }

        public int CategoryKey { get; set; }

        public int MortgageLoanPurposeKey { get; set; }
    }
}