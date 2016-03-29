using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Third_Party_InvoicesDataModel :  IDataModel
    {
        public Third_Party_InvoicesDataModel(long instanceID, int thirdPartyInvoiceKey, int accountKey, int thirdPartyTypeKey, int genericKey)
        {
            this.InstanceID = instanceID;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.AccountKey = accountKey;
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int ThirdPartyInvoiceKey { get; set; }

        public int AccountKey { get; set; }

        public int ThirdPartyTypeKey { get; set; }

        public int GenericKey { get; set; }
    }
}