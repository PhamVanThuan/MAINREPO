using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCStatusDataModel :  IDataModel
    {
        public HOCStatusDataModel(int hOCStatusKey, string description)
        {
            this.HOCStatusKey = hOCStatusKey;
            this.Description = description;
		
        }		

        public int HOCStatusKey { get; set; }

        public string Description { get; set; }
    }
}