using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapStatusDataModel :  IDataModel
    {
        public CapStatusDataModel(int capStatusKey, string description)
        {
            this.CapStatusKey = capStatusKey;
            this.Description = description;
		
        }		

        public int CapStatusKey { get; set; }

        public string Description { get; set; }
    }
}