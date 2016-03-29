using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityExceptionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityExceptionDataModel(int legalEntityKey, int legalEntityExceptionReasonKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.LegalEntityExceptionReasonKey = legalEntityExceptionReasonKey;
		
        }
		[JsonConstructor]
        public LegalEntityExceptionDataModel(int legalEntityExceptionKey, int legalEntityKey, int legalEntityExceptionReasonKey)
        {
            this.LegalEntityExceptionKey = legalEntityExceptionKey;
            this.LegalEntityKey = legalEntityKey;
            this.LegalEntityExceptionReasonKey = legalEntityExceptionReasonKey;
		
        }		

        public int LegalEntityExceptionKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int LegalEntityExceptionReasonKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityExceptionKey =  key;
        }
    }
}