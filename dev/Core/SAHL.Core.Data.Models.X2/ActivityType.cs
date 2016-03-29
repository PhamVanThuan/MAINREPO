using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ActivityTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ActivityTypeDataModel(string name)
        {
            this.Name = name;
		
        }
		[JsonConstructor]
        public ActivityTypeDataModel(int iD, string name)
        {
            this.ID = iD;
            this.Name = name;
		
        }		

        public int ID { get; set; }

        public string Name { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}