using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ErrorRepositoryDataModel :  IDataModel
    {
        public ErrorRepositoryDataModel(int errorRepositoryKey, string description, bool active)
        {
            this.ErrorRepositoryKey = errorRepositoryKey;
            this.Description = description;
            this.Active = active;
		
        }		

        public int ErrorRepositoryKey { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }
    }
}