using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ITDataModel :  IDataModel
    {
        public ITDataModel(long instanceID, int? key, string user)
        {
            this.InstanceID = instanceID;
            this.Key = key;
            this.User = user;
		
        }		

        public long InstanceID { get; set; }

        public int? Key { get; set; }

        public string User { get; set; }
    }
}