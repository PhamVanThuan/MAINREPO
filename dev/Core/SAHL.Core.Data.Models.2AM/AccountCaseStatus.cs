using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountCaseStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountCaseStatusDataModel(int? accountKey, int? caseStatusKey, string userName, DateTime? updateDate, string notes)
        {
            this.AccountKey = accountKey;
            this.CaseStatusKey = caseStatusKey;
            this.UserName = userName;
            this.UpdateDate = updateDate;
            this.Notes = notes;
		
        }
		[JsonConstructor]
        public AccountCaseStatusDataModel(int accountCaseStatusKey, int? accountKey, int? caseStatusKey, string userName, DateTime? updateDate, string notes)
        {
            this.AccountCaseStatusKey = accountCaseStatusKey;
            this.AccountKey = accountKey;
            this.CaseStatusKey = caseStatusKey;
            this.UserName = userName;
            this.UpdateDate = updateDate;
            this.Notes = notes;
		
        }		

        public int AccountCaseStatusKey { get; set; }

        public int? AccountKey { get; set; }

        public int? CaseStatusKey { get; set; }

        public string UserName { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Notes { get; set; }

        public void SetKey(int key)
        {
            this.AccountCaseStatusKey =  key;
        }
    }
}