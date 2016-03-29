using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProfileTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProfileTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ProfileTypeDataModel(int profileTypeKey, string description)
        {
            this.ProfileTypeKey = profileTypeKey;
            this.Description = description;
		
        }		

        public int ProfileTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ProfileTypeKey =  key;
        }
    }
}