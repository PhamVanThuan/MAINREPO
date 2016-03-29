using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixCellDimensionDataModel :  IDataModel
    {
        public RiskMatrixCellDimensionDataModel(int riskMatrixCellDimensionKey, int riskMatrixCellKey, int riskMatrixDimensionKey, int riskMatrixRangeKey)
        {
            this.RiskMatrixCellDimensionKey = riskMatrixCellDimensionKey;
            this.RiskMatrixCellKey = riskMatrixCellKey;
            this.RiskMatrixDimensionKey = riskMatrixDimensionKey;
            this.RiskMatrixRangeKey = riskMatrixRangeKey;
		
        }		

        public int RiskMatrixCellDimensionKey { get; set; }

        public int RiskMatrixCellKey { get; set; }

        public int RiskMatrixDimensionKey { get; set; }

        public int RiskMatrixRangeKey { get; set; }
    }
}