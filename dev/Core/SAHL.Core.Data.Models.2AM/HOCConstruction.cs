using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCConstructionDataModel :  IDataModel
    {
        public HOCConstructionDataModel(int hOCConstructionKey, string description)
        {
            this.HOCConstructionKey = hOCConstructionKey;
            this.Description = description;
		
        }		

        public int HOCConstructionKey { get; set; }

        public string Description { get; set; }
    }
}