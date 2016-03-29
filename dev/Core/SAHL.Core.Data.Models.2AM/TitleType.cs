using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TitleTypeDataModel :  IDataModel
    {
        public TitleTypeDataModel(int titleTypeKey, string description)
        {
            this.TitleTypeKey = titleTypeKey;
            this.Description = description;
		
        }		

        public int TitleTypeKey { get; set; }

        public string Description { get; set; }
    }
}