using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureAttorneyDetailTypeMappingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ForeclosureAttorneyDetailTypeMappingDataModel(int? legalEntityKey, int? detailTypeKey, int? generalStatusKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.DetailTypeKey = detailTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public ForeclosureAttorneyDetailTypeMappingDataModel(int foreclosureAttorneyDetailTypeMappingKey, int? legalEntityKey, int? detailTypeKey, int? generalStatusKey)
        {
            this.ForeclosureAttorneyDetailTypeMappingKey = foreclosureAttorneyDetailTypeMappingKey;
            this.LegalEntityKey = legalEntityKey;
            this.DetailTypeKey = detailTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int ForeclosureAttorneyDetailTypeMappingKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public int? DetailTypeKey { get; set; }

        public int? GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.ForeclosureAttorneyDetailTypeMappingKey =  key;
        }
    }
}