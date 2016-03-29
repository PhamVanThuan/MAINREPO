using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OSPFinancialAdjustmentTypeSourceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OSPFinancialAdjustmentTypeSourceDataModel(int financialAdjustmentTypeSourceKey, int originationSourceProductKey)
        {
            this.FinancialAdjustmentTypeSourceKey = financialAdjustmentTypeSourceKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
		
        }
		[JsonConstructor]
        public OSPFinancialAdjustmentTypeSourceDataModel(int oSPFinancialAdjustmentTypeSourceKey, int financialAdjustmentTypeSourceKey, int originationSourceProductKey)
        {
            this.OSPFinancialAdjustmentTypeSourceKey = oSPFinancialAdjustmentTypeSourceKey;
            this.FinancialAdjustmentTypeSourceKey = financialAdjustmentTypeSourceKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
		
        }		

        public int OSPFinancialAdjustmentTypeSourceKey { get; set; }

        public int FinancialAdjustmentTypeSourceKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public void SetKey(int key)
        {
            this.OSPFinancialAdjustmentTypeSourceKey =  key;
        }
    }
}