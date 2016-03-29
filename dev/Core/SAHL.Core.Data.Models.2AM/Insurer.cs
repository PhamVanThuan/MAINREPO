using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InsurerDataModel :  IDataModel
    {
        public InsurerDataModel(int insurerKey, string description)
        {
            this.InsurerKey = insurerKey;
            this.Description = description;
		
        }		

        public int InsurerKey { get; set; }

        public string Description { get; set; }
    }
}