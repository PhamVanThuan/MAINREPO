using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityExceptionReasonDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityExceptionReasonDataModel(string description, byte priority)
        {
            this.Description = description;
            this.Priority = priority;
		
        }
		[JsonConstructor]
        public LegalEntityExceptionReasonDataModel(int legalEntityExceptionReasonKey, string description, byte priority)
        {
            this.LegalEntityExceptionReasonKey = legalEntityExceptionReasonKey;
            this.Description = description;
            this.Priority = priority;
		
        }		

        public int LegalEntityExceptionReasonKey { get; set; }

        public string Description { get; set; }

        public byte Priority { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityExceptionReasonKey =  key;
        }
    }
}