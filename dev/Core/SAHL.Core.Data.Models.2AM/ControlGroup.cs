using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ControlGroupDataModel :  IDataModel
    {
        public ControlGroupDataModel(int controlGroupKey, string description)
        {
            this.ControlGroupKey = controlGroupKey;
            this.Description = description;
		
        }		

        public int ControlGroupKey { get; set; }

        public string Description { get; set; }
    }
}