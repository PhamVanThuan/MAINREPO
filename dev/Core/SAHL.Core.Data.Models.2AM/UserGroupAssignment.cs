using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserGroupAssignmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserGroupAssignmentDataModel(int userGroupMappingKey, int aDUserKey, int genericKey, DateTime? changeDate, DateTime insertedDate)
        {
            this.UserGroupMappingKey = userGroupMappingKey;
            this.ADUserKey = aDUserKey;
            this.GenericKey = genericKey;
            this.ChangeDate = changeDate;
            this.InsertedDate = insertedDate;
		
        }
		[JsonConstructor]
        public UserGroupAssignmentDataModel(int userGroupAssignmentKey, int userGroupMappingKey, int aDUserKey, int genericKey, DateTime? changeDate, DateTime insertedDate)
        {
            this.UserGroupAssignmentKey = userGroupAssignmentKey;
            this.UserGroupMappingKey = userGroupMappingKey;
            this.ADUserKey = aDUserKey;
            this.GenericKey = genericKey;
            this.ChangeDate = changeDate;
            this.InsertedDate = insertedDate;
		
        }		

        public int UserGroupAssignmentKey { get; set; }

        public int UserGroupMappingKey { get; set; }

        public int ADUserKey { get; set; }

        public int GenericKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public void SetKey(int key)
        {
            this.UserGroupAssignmentKey =  key;
        }
    }
}