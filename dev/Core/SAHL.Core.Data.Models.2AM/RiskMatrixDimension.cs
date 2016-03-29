using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixDimensionDataModel :  IDataModel
    {
        public RiskMatrixDimensionDataModel(int riskMatrixDimensionKey, string description)
        {
            this.RiskMatrixDimensionKey = riskMatrixDimensionKey;
            this.Description = description;
		
        }		

        public int RiskMatrixDimensionKey { get; set; }

        public string Description { get; set; }
    }
}