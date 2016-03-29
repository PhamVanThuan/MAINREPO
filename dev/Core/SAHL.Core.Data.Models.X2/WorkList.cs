using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkListDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkListDataModel(long instanceID, string aDUserName, DateTime listDate, string message)
        {
            this.InstanceID = instanceID;
            this.ADUserName = aDUserName;
            this.ListDate = listDate;
            this.Message = message;
		
        }
		[JsonConstructor]
        public WorkListDataModel(int iD, long instanceID, string aDUserName, DateTime listDate, string message)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
            this.ADUserName = aDUserName;
            this.ListDate = listDate;
            this.Message = message;
		
        }		

        public int ID { get; set; }

        public long InstanceID { get; set; }

        public string ADUserName { get; set; }

        public DateTime ListDate { get; set; }

        public string Message { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}