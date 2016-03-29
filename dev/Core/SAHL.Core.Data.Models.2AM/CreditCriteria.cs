using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditCriteriaDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CreditCriteriaDataModel(int creditMatrixKey, int marginKey, int categoryKey, int employmentTypeKey, int mortgageLoanPurposeKey, double? minLoanAmount, double? maxLoanAmount, double? minPropertyValue, double? maxPropertyValue, double? lTV, double? pTI, double? minIncomeAmount, bool? exceptionCriteria, double? maxIncomeAmount, int? minEmpiricaScore)
        {
            this.CreditMatrixKey = creditMatrixKey;
            this.MarginKey = marginKey;
            this.CategoryKey = categoryKey;
            this.EmploymentTypeKey = employmentTypeKey;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.MinLoanAmount = minLoanAmount;
            this.MaxLoanAmount = maxLoanAmount;
            this.MinPropertyValue = minPropertyValue;
            this.MaxPropertyValue = maxPropertyValue;
            this.LTV = lTV;
            this.PTI = pTI;
            this.MinIncomeAmount = minIncomeAmount;
            this.ExceptionCriteria = exceptionCriteria;
            this.MaxIncomeAmount = maxIncomeAmount;
            this.MinEmpiricaScore = minEmpiricaScore;
		
        }
		[JsonConstructor]
        public CreditCriteriaDataModel(int creditCriteriaKey, int creditMatrixKey, int marginKey, int categoryKey, int employmentTypeKey, int mortgageLoanPurposeKey, double? minLoanAmount, double? maxLoanAmount, double? minPropertyValue, double? maxPropertyValue, double? lTV, double? pTI, double? minIncomeAmount, bool? exceptionCriteria, double? maxIncomeAmount, int? minEmpiricaScore)
        {
            this.CreditCriteriaKey = creditCriteriaKey;
            this.CreditMatrixKey = creditMatrixKey;
            this.MarginKey = marginKey;
            this.CategoryKey = categoryKey;
            this.EmploymentTypeKey = employmentTypeKey;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.MinLoanAmount = minLoanAmount;
            this.MaxLoanAmount = maxLoanAmount;
            this.MinPropertyValue = minPropertyValue;
            this.MaxPropertyValue = maxPropertyValue;
            this.LTV = lTV;
            this.PTI = pTI;
            this.MinIncomeAmount = minIncomeAmount;
            this.ExceptionCriteria = exceptionCriteria;
            this.MaxIncomeAmount = maxIncomeAmount;
            this.MinEmpiricaScore = minEmpiricaScore;
		
        }		

        public int CreditCriteriaKey { get; set; }

        public int CreditMatrixKey { get; set; }

        public int MarginKey { get; set; }

        public int CategoryKey { get; set; }

        public int EmploymentTypeKey { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public double? MinLoanAmount { get; set; }

        public double? MaxLoanAmount { get; set; }

        public double? MinPropertyValue { get; set; }

        public double? MaxPropertyValue { get; set; }

        public double? LTV { get; set; }

        public double? PTI { get; set; }

        public double? MinIncomeAmount { get; set; }

        public bool? ExceptionCriteria { get; set; }

        public double? MaxIncomeAmount { get; set; }

        public int? MinEmpiricaScore { get; set; }

        public void SetKey(int key)
        {
            this.CreditCriteriaKey =  key;
        }
    }
}