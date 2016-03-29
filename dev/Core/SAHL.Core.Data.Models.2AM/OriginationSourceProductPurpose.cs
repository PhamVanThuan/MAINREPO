using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductPurposeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductPurposeDataModel(int originationSourceProductKey, int mortgageLoanPurposeKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductPurposeDataModel(int originationSourceProductPurposeKey, int originationSourceProductKey, int mortgageLoanPurposeKey)
        {
            this.OriginationSourceProductPurposeKey = originationSourceProductPurposeKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
		
        }		

        public int OriginationSourceProductPurposeKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceProductPurposeKey =  key;
        }
    }
}