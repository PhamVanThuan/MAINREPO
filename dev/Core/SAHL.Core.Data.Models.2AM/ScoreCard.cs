using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ScoreCardDataModel :  IDataModel
    {
        public ScoreCardDataModel(int scoreCardKey, string description, double? basePoints, DateTime revisionDate, int generalStatusKey)
        {
            this.ScoreCardKey = scoreCardKey;
            this.Description = description;
            this.BasePoints = basePoints;
            this.RevisionDate = revisionDate;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int ScoreCardKey { get; set; }

        public string Description { get; set; }

        public double? BasePoints { get; set; }

        public DateTime RevisionDate { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}