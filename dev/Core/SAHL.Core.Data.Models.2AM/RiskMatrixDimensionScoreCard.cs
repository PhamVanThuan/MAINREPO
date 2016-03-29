using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixDimensionScoreCardDataModel :  IDataModel
    {
        public RiskMatrixDimensionScoreCardDataModel(int riskMatrixDimensionScoreCardKey, int riskMatrixDimensionKey, int scoreCardKey)
        {
            this.RiskMatrixDimensionScoreCardKey = riskMatrixDimensionScoreCardKey;
            this.RiskMatrixDimensionKey = riskMatrixDimensionKey;
            this.ScoreCardKey = scoreCardKey;
		
        }		

        public int RiskMatrixDimensionScoreCardKey { get; set; }

        public int RiskMatrixDimensionKey { get; set; }

        public int ScoreCardKey { get; set; }
    }
}