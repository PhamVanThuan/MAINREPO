using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class OpenThirdPartyInvoiceDataModel :  IDataModel
    {
        public OpenThirdPartyInvoiceDataModel(int thirdPartyInvoiceKey, long instanceID, string workflowState, string assignedUser, int invoiceStatusKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.InstanceID = instanceID;
            this.WorkflowState = workflowState;
            this.AssignedUser = assignedUser;
            this.InvoiceStatusKey = invoiceStatusKey;
		
        }		

        public int ThirdPartyInvoiceKey { get; set; }

        public long InstanceID { get; set; }

        public string WorkflowState { get; set; }

        public string AssignedUser { get; set; }

        public int InvoiceStatusKey { get; set; }
    }
}