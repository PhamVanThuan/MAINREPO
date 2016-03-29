using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BehaviouralScoreCategoryDataModel :  IDataModel
    {
        public BehaviouralScoreCategoryDataModel(int behaviouralScoreCategoryKey, string description, double behaviouralScore, string thresholdColour)
        {
            this.BehaviouralScoreCategoryKey = behaviouralScoreCategoryKey;
            this.Description = description;
            this.BehaviouralScore = behaviouralScore;
            this.ThresholdColour = thresholdColour;
		
        }		

        public int BehaviouralScoreCategoryKey { get; set; }

        public string Description { get; set; }

        public double BehaviouralScore { get; set; }

        public string ThresholdColour { get; set; }
    }
}