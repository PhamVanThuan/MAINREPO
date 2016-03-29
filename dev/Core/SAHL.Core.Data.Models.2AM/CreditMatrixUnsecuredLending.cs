using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditMatrixUnsecuredLendingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CreditMatrixUnsecuredLendingDataModel(string newBusinessIndicator, DateTime? implementationDate)
        {
            this.NewBusinessIndicator = newBusinessIndicator;
            this.ImplementationDate = implementationDate;
		
        }
		[JsonConstructor]
        public CreditMatrixUnsecuredLendingDataModel(int creditMatrixUnsecuredLendingKey, string newBusinessIndicator, DateTime? implementationDate)
        {
            this.CreditMatrixUnsecuredLendingKey = creditMatrixUnsecuredLendingKey;
            this.NewBusinessIndicator = newBusinessIndicator;
            this.ImplementationDate = implementationDate;
		
        }		

        public int CreditMatrixUnsecuredLendingKey { get; set; }

        public string NewBusinessIndicator { get; set; }

        public DateTime? ImplementationDate { get; set; }

        public void SetKey(int key)
        {
            this.CreditMatrixUnsecuredLendingKey =  key;
        }
    }
}