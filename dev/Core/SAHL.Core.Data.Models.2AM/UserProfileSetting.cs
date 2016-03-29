using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserProfileSettingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserProfileSettingDataModel(int aDUserKey, string settingName, string settingValue, string settingType)
        {
            this.ADUserKey = aDUserKey;
            this.SettingName = settingName;
            this.SettingValue = settingValue;
            this.SettingType = settingType;
		
        }
		[JsonConstructor]
        public UserProfileSettingDataModel(int userProfileSettingKey, int aDUserKey, string settingName, string settingValue, string settingType)
        {
            this.UserProfileSettingKey = userProfileSettingKey;
            this.ADUserKey = aDUserKey;
            this.SettingName = settingName;
            this.SettingValue = settingValue;
            this.SettingType = settingType;
		
        }		

        public int UserProfileSettingKey { get; set; }

        public int ADUserKey { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        public string SettingType { get; set; }

        public void SetKey(int key)
        {
            this.UserProfileSettingKey =  key;
        }
    }
}