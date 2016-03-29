using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Release_And_VariationsDataModel :  IDataModel
    {
        public Release_And_VariationsDataModel(long instanceID, int? applicationKey, bool? isFromFL, string previousState)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.IsFromFL = isFromFL;
            this.PreviousState = previousState;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public bool? IsFromFL { get; set; }

        public string PreviousState { get; set; }
    }
}