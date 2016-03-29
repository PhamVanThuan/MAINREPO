using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RoundRobinPointerDataModel :  IDataModel
    {
        public RoundRobinPointerDataModel(int roundRobinPointerKey, int? roundRobinPointerIndexID, string description, int generalStatusKey)
        {
            this.RoundRobinPointerKey = roundRobinPointerKey;
            this.RoundRobinPointerIndexID = roundRobinPointerIndexID;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int RoundRobinPointerKey { get; set; }

        public int? RoundRobinPointerIndexID { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}