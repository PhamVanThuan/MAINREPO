using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Personal_LoansDataModel :  IDataModel
    {
        public Personal_LoansDataModel(long instanceID, int? applicationKey, string previousState, int genericKey)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.PreviousState = previousState;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public string PreviousState { get; set; }

        public int GenericKey { get; set; }
    }
}