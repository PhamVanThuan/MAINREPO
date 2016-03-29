using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CdctablelistDataModel :  IDataModel
    {
        public CdctablelistDataModel(string name, string schema)
        {
            this.Name = name;
            this.Schema = schema;
		
        }		

        public string Name { get; set; }

        public string Schema { get; set; }
    }
}