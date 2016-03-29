using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ScoreCardAttributeDataModel :  IDataModel
    {
        public ScoreCardAttributeDataModel(int scoreCardAttributeKey, int scoreCardKey, string code, string description)
        {
            this.ScoreCardAttributeKey = scoreCardAttributeKey;
            this.ScoreCardKey = scoreCardKey;
            this.Code = code;
            this.Description = description;
		
        }		

        public int ScoreCardAttributeKey { get; set; }

        public int ScoreCardKey { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}