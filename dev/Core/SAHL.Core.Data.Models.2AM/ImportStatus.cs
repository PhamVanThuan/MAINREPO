using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportStatusDataModel :  IDataModel
    {
        public ImportStatusDataModel(int importStatusKey, string description)
        {
            this.ImportStatusKey = importStatusKey;
            this.Description = description;
		
        }		

        public int ImportStatusKey { get; set; }

        public string Description { get; set; }
    }
}