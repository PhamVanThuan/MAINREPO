using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RateAdjustmentElementTypeDataModel :  IDataModel
    {
        public RateAdjustmentElementTypeDataModel(int rateAdjustmentElementTypeKey, string description, string statementName)
        {
            this.RateAdjustmentElementTypeKey = rateAdjustmentElementTypeKey;
            this.Description = description;
            this.StatementName = statementName;
		
        }		

        public int RateAdjustmentElementTypeKey { get; set; }

        public string Description { get; set; }

        public string StatementName { get; set; }
    }
}