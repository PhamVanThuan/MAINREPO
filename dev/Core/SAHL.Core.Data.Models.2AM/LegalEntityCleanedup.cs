using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityCleanedupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityCleanedupDataModel(int legalEntityKey, int legalEntityExceptionReasonKey, string description, string surname, string firstnames, string iDNumber, string accounts)
        {
            this.LegalEntityKey = legalEntityKey;
            this.LegalEntityExceptionReasonKey = legalEntityExceptionReasonKey;
            this.Description = description;
            this.Surname = surname;
            this.Firstnames = firstnames;
            this.IDNumber = iDNumber;
            this.Accounts = accounts;
		
        }
		[JsonConstructor]
        public LegalEntityCleanedupDataModel(int cleanupkey, int legalEntityKey, int legalEntityExceptionReasonKey, string description, string surname, string firstnames, string iDNumber, string accounts)
        {
            this.cleanupkey = cleanupkey;
            this.LegalEntityKey = legalEntityKey;
            this.LegalEntityExceptionReasonKey = legalEntityExceptionReasonKey;
            this.Description = description;
            this.Surname = surname;
            this.Firstnames = firstnames;
            this.IDNumber = iDNumber;
            this.Accounts = accounts;
		
        }		

        public int cleanupkey { get; set; }

        public int LegalEntityKey { get; set; }

        public int LegalEntityExceptionReasonKey { get; set; }

        public string Description { get; set; }

        public string Surname { get; set; }

        public string Firstnames { get; set; }

        public string IDNumber { get; set; }

        public string Accounts { get; set; }

        public void SetKey(int key)
        {
            this.cleanupkey =  key;
        }
    }
}