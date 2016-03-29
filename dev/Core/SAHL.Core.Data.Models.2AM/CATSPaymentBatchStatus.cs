using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CATSPaymentBatchStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CATSPaymentBatchStatusDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public CATSPaymentBatchStatusDataModel(int cATSPaymentBatchStatusKey, string description)
        {
            this.CATSPaymentBatchStatusKey = cATSPaymentBatchStatusKey;
            this.Description = description;
		
        }		

        public int CATSPaymentBatchStatusKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.CATSPaymentBatchStatusKey =  key;
        }
    }
}