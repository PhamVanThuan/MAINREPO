using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductCreditMatrixDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductCreditMatrixDataModel(int creditMatrixKey, int originationSourceProductKey)
        {
            this.CreditMatrixKey = creditMatrixKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductCreditMatrixDataModel(int originationSourceProductCreditMatrixKey, int creditMatrixKey, int originationSourceProductKey)
        {
            this.OriginationSourceProductCreditMatrixKey = originationSourceProductCreditMatrixKey;
            this.CreditMatrixKey = creditMatrixKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
		
        }		

        public int OriginationSourceProductCreditMatrixKey { get; set; }

        public int CreditMatrixKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceProductCreditMatrixKey =  key;
        }
    }
}