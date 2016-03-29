using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InvertedKeysDataModel :  IDataModel
    {
        public InvertedKeysDataModel(double? f1, double? valuationKeyA, double? valuationKeyB)
        {
            this.F1 = f1;
            this.ValuationKeyA = valuationKeyA;
            this.ValuationKeyB = valuationKeyB;
		
        }		

        public double? F1 { get; set; }

        public double? ValuationKeyA { get; set; }

        public double? ValuationKeyB { get; set; }
    }
}