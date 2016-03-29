using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InternetLeadUsersDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public InternetLeadUsersDataModel(int aDUserKey, bool? flag, int? caseCount, int? lastCaseKey, int generalStatusKey)
        {
            this.ADUserKey = aDUserKey;
            this.Flag = flag;
            this.CaseCount = caseCount;
            this.LastCaseKey = lastCaseKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public InternetLeadUsersDataModel(int internetLeadUsersKey, int aDUserKey, bool? flag, int? caseCount, int? lastCaseKey, int generalStatusKey)
        {
            this.InternetLeadUsersKey = internetLeadUsersKey;
            this.ADUserKey = aDUserKey;
            this.Flag = flag;
            this.CaseCount = caseCount;
            this.LastCaseKey = lastCaseKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int InternetLeadUsersKey { get; set; }

        public int ADUserKey { get; set; }

        public bool? Flag { get; set; }

        public int? CaseCount { get; set; }

        public int? LastCaseKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.InternetLeadUsersKey =  key;
        }
    }
}