using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixDataModel :  IDataModel
    {
        public RiskMatrixDataModel(int riskMatrixKey, string description)
        {
            this.RiskMatrixKey = riskMatrixKey;
            this.Description = description;
		
        }		

        public int RiskMatrixKey { get; set; }

        public string Description { get; set; }
    }
}