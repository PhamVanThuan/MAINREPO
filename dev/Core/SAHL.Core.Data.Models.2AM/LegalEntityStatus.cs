using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityStatusDataModel :  IDataModel
    {
        public LegalEntityStatusDataModel(int legalEntityStatusKey, string description)
        {
            this.LegalEntityStatusKey = legalEntityStatusKey;
            this.Description = description;
		
        }		

        public int LegalEntityStatusKey { get; set; }

        public string Description { get; set; }
    }
}