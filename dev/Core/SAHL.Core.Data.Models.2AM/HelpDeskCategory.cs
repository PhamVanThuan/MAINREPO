using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HelpDeskCategoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HelpDeskCategoryDataModel(string description, int generalStatusKey)
        {
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public HelpDeskCategoryDataModel(int helpDeskCategoryKey, string description, int generalStatusKey)
        {
            this.HelpDeskCategoryKey = helpDeskCategoryKey;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int HelpDeskCategoryKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.HelpDeskCategoryKey =  key;
        }
    }
}