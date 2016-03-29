using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Readvance_PaymentsDataModel :  IDataModel
    {
        public Readvance_PaymentsDataModel(long instanceID, int? applicationKey, string previousState, int? genericKey, int? entryPath)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.PreviousState = previousState;
            this.GenericKey = genericKey;
            this.EntryPath = entryPath;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public string PreviousState { get; set; }

        public int? GenericKey { get; set; }

        public int? EntryPath { get; set; }
    }
}