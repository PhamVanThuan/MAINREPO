using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DataServiceDataModel :  IDataModel
    {
        public DataServiceDataModel(int dataServiceKey, string description)
        {
            this.DataServiceKey = dataServiceKey;
            this.Description = description;
		
        }		

        public int DataServiceKey { get; set; }

        public string Description { get; set; }
    }
}