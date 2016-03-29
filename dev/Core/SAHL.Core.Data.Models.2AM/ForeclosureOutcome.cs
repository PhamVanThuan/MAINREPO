using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureOutcomeDataModel :  IDataModel
    {
        public ForeclosureOutcomeDataModel(int foreclosureOutcomeKey, string description)
        {
            this.ForeclosureOutcomeKey = foreclosureOutcomeKey;
            this.Description = description;
		
        }		

        public int ForeclosureOutcomeKey { get; set; }

        public string Description { get; set; }
    }
}