using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TranslatableItemTEMPDataModel :  IDataModel
    {
        public TranslatableItemTEMPDataModel(int translatableItemKey, string description)
        {
            this.TranslatableItemKey = translatableItemKey;
            this.Description = description;
		
        }		

        public int TranslatableItemKey { get; set; }

        public string Description { get; set; }
    }
}