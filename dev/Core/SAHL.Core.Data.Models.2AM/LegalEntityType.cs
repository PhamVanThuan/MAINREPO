using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityTypeDataModel :  IDataModel
    {
        public LegalEntityTypeDataModel(int legalEntityTypeKey, string description)
        {
            this.LegalEntityTypeKey = legalEntityTypeKey;
            this.Description = description;
		
        }		

        public int LegalEntityTypeKey { get; set; }

        public string Description { get; set; }
    }
}