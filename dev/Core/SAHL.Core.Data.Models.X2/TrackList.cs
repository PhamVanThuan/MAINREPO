using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class TrackListDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TrackListDataModel(long instanceID, string aDUserName, DateTime listDate)
        {
            this.InstanceID = instanceID;
            this.ADUserName = aDUserName;
            this.ListDate = listDate;
		
        }
		[JsonConstructor]
        public TrackListDataModel(int iD, long instanceID, string aDUserName, DateTime listDate)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
            this.ADUserName = aDUserName;
            this.ListDate = listDate;
		
        }		

        public int ID { get; set; }

        public long InstanceID { get; set; }

        public string ADUserName { get; set; }

        public DateTime ListDate { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}