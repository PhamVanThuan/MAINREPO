using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentVerificationProcessDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public EmploymentVerificationProcessDataModel(int employmentKey, int employmentVerificationProcessTypeKey, string userID, DateTime? changeDate)
        {
            this.EmploymentKey = employmentKey;
            this.EmploymentVerificationProcessTypeKey = employmentVerificationProcessTypeKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public EmploymentVerificationProcessDataModel(int employmentVerificationProcessKey, int employmentKey, int employmentVerificationProcessTypeKey, string userID, DateTime? changeDate)
        {
            this.EmploymentVerificationProcessKey = employmentVerificationProcessKey;
            this.EmploymentKey = employmentKey;
            this.EmploymentVerificationProcessTypeKey = employmentVerificationProcessTypeKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public int EmploymentVerificationProcessKey { get; set; }

        public int EmploymentKey { get; set; }

        public int EmploymentVerificationProcessTypeKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.EmploymentVerificationProcessKey =  key;
        }
    }
}