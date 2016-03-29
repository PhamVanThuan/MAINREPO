using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReasonDescriptionDataModel :  IDataModel
    {
        public ReasonDescriptionDataModel(int reasonDescriptionKey, string description, int? translatableItemKey)
        {
            this.ReasonDescriptionKey = reasonDescriptionKey;
            this.Description = description;
            this.TranslatableItemKey = translatableItemKey;
		
        }		

        public int ReasonDescriptionKey { get; set; }

        public string Description { get; set; }

        public int? TranslatableItemKey { get; set; }
    }
}