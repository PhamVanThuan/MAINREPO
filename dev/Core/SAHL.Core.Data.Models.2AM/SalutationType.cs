using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SalutationTypeDataModel :  IDataModel
    {
        public SalutationTypeDataModel(int salutationKey, string description, int translatableItemKey)
        {
            this.SalutationKey = salutationKey;
            this.Description = description;
            this.TranslatableItemKey = translatableItemKey;
		
        }		

        public int SalutationKey { get; set; }

        public string Description { get; set; }

        public int TranslatableItemKey { get; set; }
    }
}