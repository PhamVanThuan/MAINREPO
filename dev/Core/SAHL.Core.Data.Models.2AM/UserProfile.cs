using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserProfileDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserProfileDataModel(string aDUserName, int profileTypeKey, string value)
        {
            this.ADUserName = aDUserName;
            this.ProfileTypeKey = profileTypeKey;
            this.Value = value;
		
        }
		[JsonConstructor]
        public UserProfileDataModel(int userProfileKey, string aDUserName, int profileTypeKey, string value)
        {
            this.UserProfileKey = userProfileKey;
            this.ADUserName = aDUserName;
            this.ProfileTypeKey = profileTypeKey;
            this.Value = value;
		
        }		

        public int UserProfileKey { get; set; }

        public string ADUserName { get; set; }

        public int ProfileTypeKey { get; set; }

        public string Value { get; set; }

        public void SetKey(int key)
        {
            this.UserProfileKey =  key;
        }
    }
}