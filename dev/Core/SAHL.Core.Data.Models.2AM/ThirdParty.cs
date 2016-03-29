using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ThirdPartyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ThirdPartyDataModel(int legalEntityKey, bool isPanel, int generalStatusKey, int? genericKey, int? thirdPartyTypeKey, int? genericKeyTypeKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.IsPanel = isPanel;
            this.GeneralStatusKey = generalStatusKey;
            this.GenericKey = genericKey;
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public ThirdPartyDataModel(int thirdPartyKey, Guid id, int legalEntityKey, bool isPanel, int generalStatusKey, int? genericKey, int? thirdPartyTypeKey, int? genericKeyTypeKey)
        {
            this.ThirdPartyKey = thirdPartyKey;
            this.Id = id;
            this.LegalEntityKey = legalEntityKey;
            this.IsPanel = isPanel;
            this.GeneralStatusKey = generalStatusKey;
            this.GenericKey = genericKey;
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int ThirdPartyKey { get; set; }

        public Guid Id { get; set; }

        public int LegalEntityKey { get; set; }

        public bool IsPanel { get; set; }

        public int GeneralStatusKey { get; set; }

        public int? GenericKey { get; set; }

        public int? ThirdPartyTypeKey { get; set; }

        public int? GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.ThirdPartyKey =  key;
        }
    }
}