using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ScoreCardAttributeRangeDataModel :  IDataModel
    {
        public ScoreCardAttributeRangeDataModel(int scoreCardAttributeRangeKey, int scoreCardAttributeKey, double? min, double? max, double? points)
        {
            this.ScoreCardAttributeRangeKey = scoreCardAttributeRangeKey;
            this.ScoreCardAttributeKey = scoreCardAttributeKey;
            this.Min = min;
            this.Max = max;
            this.Points = points;
		
        }		

        public int ScoreCardAttributeRangeKey { get; set; }

        public int ScoreCardAttributeKey { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public double? Points { get; set; }
    }
}