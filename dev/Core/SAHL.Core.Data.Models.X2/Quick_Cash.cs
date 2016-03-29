using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Quick_CashDataModel :  IDataModel
    {
        public Quick_CashDataModel(long instanceID, int? applicationKey, string previousState)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.PreviousState = previousState;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public string PreviousState { get; set; }
    }
}