using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GeneralStatusDataModel :  IDataModel
    {
        public GeneralStatusDataModel(int generalStatusKey, string description)
        {
            this.GeneralStatusKey = generalStatusKey;
            this.Description = description;
		
        }		

        public int GeneralStatusKey { get; set; }

        public string Description { get; set; }
    }
}