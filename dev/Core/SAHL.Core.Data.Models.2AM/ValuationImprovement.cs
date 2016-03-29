using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationImprovementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValuationImprovementDataModel(int valuationKey, DateTime? improvementDate, int valuationImprovementTypeKey, double improvementValue)
        {
            this.ValuationKey = valuationKey;
            this.ImprovementDate = improvementDate;
            this.ValuationImprovementTypeKey = valuationImprovementTypeKey;
            this.ImprovementValue = improvementValue;
		
        }
		[JsonConstructor]
        public ValuationImprovementDataModel(int valuationImprovementKey, int valuationKey, DateTime? improvementDate, int valuationImprovementTypeKey, double improvementValue)
        {
            this.ValuationImprovementKey = valuationImprovementKey;
            this.ValuationKey = valuationKey;
            this.ImprovementDate = improvementDate;
            this.ValuationImprovementTypeKey = valuationImprovementTypeKey;
            this.ImprovementValue = improvementValue;
		
        }		

        public int ValuationImprovementKey { get; set; }

        public int ValuationKey { get; set; }

        public DateTime? ImprovementDate { get; set; }

        public int ValuationImprovementTypeKey { get; set; }

        public double ImprovementValue { get; set; }

        public void SetKey(int key)
        {
            this.ValuationImprovementKey =  key;
        }
    }
}