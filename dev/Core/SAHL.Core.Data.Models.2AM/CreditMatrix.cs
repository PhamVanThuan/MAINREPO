using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditMatrixDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CreditMatrixDataModel(string newBusinessIndicator, DateTime? implementationDate)
        {
            this.NewBusinessIndicator = newBusinessIndicator;
            this.ImplementationDate = implementationDate;
		
        }
		[JsonConstructor]
        public CreditMatrixDataModel(int creditMatrixKey, string newBusinessIndicator, DateTime? implementationDate)
        {
            this.CreditMatrixKey = creditMatrixKey;
            this.NewBusinessIndicator = newBusinessIndicator;
            this.ImplementationDate = implementationDate;
		
        }		

        public int CreditMatrixKey { get; set; }

        public string NewBusinessIndicator { get; set; }

        public DateTime? ImplementationDate { get; set; }

        public void SetKey(int key)
        {
            this.CreditMatrixKey =  key;
        }
    }
}