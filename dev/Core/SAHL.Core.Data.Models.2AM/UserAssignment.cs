using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserAssignmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserAssignmentDataModel(int financialServiceKey, int originationSourceProductKey, DateTime assignmentDate, string assigningUser, string assignedUser)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.AssignmentDate = assignmentDate;
            this.AssigningUser = assigningUser;
            this.AssignedUser = assignedUser;
		
        }
		[JsonConstructor]
        public UserAssignmentDataModel(int userAssignmentKey, int financialServiceKey, int originationSourceProductKey, DateTime assignmentDate, string assigningUser, string assignedUser)
        {
            this.UserAssignmentKey = userAssignmentKey;
            this.FinancialServiceKey = financialServiceKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.AssignmentDate = assignmentDate;
            this.AssigningUser = assigningUser;
            this.AssignedUser = assignedUser;
		
        }		

        public int UserAssignmentKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public DateTime AssignmentDate { get; set; }

        public string AssigningUser { get; set; }

        public string AssignedUser { get; set; }

        public void SetKey(int key)
        {
            this.UserAssignmentKey =  key;
        }
    }
}