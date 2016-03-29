using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class AccountsPaidForAttorneyInvoicesMonthlyDataModel :  IDataModel
    {
        public AccountsPaidForAttorneyInvoicesMonthlyDataModel(Guid attorneyId, int thirdPartyInvoiceKey, int accountKey)
        {
            this.AttorneyId = attorneyId;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.AccountKey = accountKey;
		
        }		

        public Guid AttorneyId { get; set; }

        public int ThirdPartyInvoiceKey { get; set; }

        public int AccountKey { get; set; }
    }
}