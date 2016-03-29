using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class EmptyThirdPartyInvoicesDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public EmptyThirdPartyInvoicesDataModel(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
		
        }
		[JsonConstructor]
        public EmptyThirdPartyInvoicesDataModel(int id, int thirdPartyInvoiceKey)
        {
            this.Id = id;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
		
        }		

        public int Id { get; set; }

        public int ThirdPartyInvoiceKey { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}