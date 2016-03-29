using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PopulationGroupDataModel :  IDataModel
    {
        public PopulationGroupDataModel(int populationGroupKey, string description)
        {
            this.PopulationGroupKey = populationGroupKey;
            this.Description = description;
		
        }		

        public int PopulationGroupKey { get; set; }

        public string Description { get; set; }
    }
}