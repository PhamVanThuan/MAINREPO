using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Delete_Debit_OrderDataModel :  IDataModel
    {
        public Delete_Debit_OrderDataModel(long instanceID, int? debitOrderKey, string requestUser, int? accountKey, string processUser, bool? requestApproved)
        {
            this.InstanceID = instanceID;
            this.DebitOrderKey = debitOrderKey;
            this.RequestUser = requestUser;
            this.AccountKey = accountKey;
            this.ProcessUser = processUser;
            this.RequestApproved = requestApproved;
		
        }		

        public long InstanceID { get; set; }

        public int? DebitOrderKey { get; set; }

        public string RequestUser { get; set; }

        public int? AccountKey { get; set; }

        public string ProcessUser { get; set; }

        public bool? RequestApproved { get; set; }
    }
}