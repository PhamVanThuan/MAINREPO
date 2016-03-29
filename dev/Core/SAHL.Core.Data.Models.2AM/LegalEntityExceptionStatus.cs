using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityExceptionStatusDataModel :  IDataModel
    {
        public LegalEntityExceptionStatusDataModel(int legalEntityExceptionStatusKey, string description)
        {
            this.LegalEntityExceptionStatusKey = legalEntityExceptionStatusKey;
            this.Description = description;
		
        }		

        public int LegalEntityExceptionStatusKey { get; set; }

        public string Description { get; set; }
    }
}