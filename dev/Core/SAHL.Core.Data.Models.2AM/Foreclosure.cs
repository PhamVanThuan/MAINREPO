using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ForeclosureDataModel(int accountKey, int generalStatusKey, int foreclosureOutcomeKey, DateTime? foreclosureDateTime)
        {
            this.AccountKey = accountKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ForeclosureOutcomeKey = foreclosureOutcomeKey;
            this.ForeclosureDateTime = foreclosureDateTime;
		
        }
		[JsonConstructor]
        public ForeclosureDataModel(int foreclosureKey, int accountKey, int generalStatusKey, int foreclosureOutcomeKey, DateTime? foreclosureDateTime)
        {
            this.ForeclosureKey = foreclosureKey;
            this.AccountKey = accountKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ForeclosureOutcomeKey = foreclosureOutcomeKey;
            this.ForeclosureDateTime = foreclosureDateTime;
		
        }		

        public int ForeclosureKey { get; set; }

        public int AccountKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int ForeclosureOutcomeKey { get; set; }

        public DateTime? ForeclosureDateTime { get; set; }

        public void SetKey(int key)
        {
            this.ForeclosureKey =  key;
        }
    }
}