using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditCriteriaUnsecuredLendingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CreditCriteriaUnsecuredLendingDataModel(int creditMatrixUnsecuredLendingKey, int marginKey, double minLoanAmount, double maxLoanAmount, int term)
        {
            this.CreditMatrixUnsecuredLendingKey = creditMatrixUnsecuredLendingKey;
            this.MarginKey = marginKey;
            this.MinLoanAmount = minLoanAmount;
            this.MaxLoanAmount = maxLoanAmount;
            this.Term = term;
		
        }
		[JsonConstructor]
        public CreditCriteriaUnsecuredLendingDataModel(int creditCriteriaUnsecuredLendingKey, int creditMatrixUnsecuredLendingKey, int marginKey, double minLoanAmount, double maxLoanAmount, int term)
        {
            this.CreditCriteriaUnsecuredLendingKey = creditCriteriaUnsecuredLendingKey;
            this.CreditMatrixUnsecuredLendingKey = creditMatrixUnsecuredLendingKey;
            this.MarginKey = marginKey;
            this.MinLoanAmount = minLoanAmount;
            this.MaxLoanAmount = maxLoanAmount;
            this.Term = term;
		
        }		

        public int CreditCriteriaUnsecuredLendingKey { get; set; }

        public int CreditMatrixUnsecuredLendingKey { get; set; }

        public int MarginKey { get; set; }

        public double MinLoanAmount { get; set; }

        public double MaxLoanAmount { get; set; }

        public int Term { get; set; }

        public void SetKey(int key)
        {
            this.CreditCriteriaUnsecuredLendingKey =  key;
        }
    }
}