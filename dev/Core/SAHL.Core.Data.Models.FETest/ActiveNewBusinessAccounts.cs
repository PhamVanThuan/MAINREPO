using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ActiveNewBusinessAccountsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ActiveNewBusinessAccountsDataModel(int accountKey, int productKey, bool hasThirdPartyInvoice)
        {
            this.AccountKey = accountKey;
            this.ProductKey = productKey;
            this.HasThirdPartyInvoice = hasThirdPartyInvoice;
		
        }
		[JsonConstructor]
        public ActiveNewBusinessAccountsDataModel(int id, int accountKey, int productKey, bool hasThirdPartyInvoice)
        {
            this.Id = id;
            this.AccountKey = accountKey;
            this.ProductKey = productKey;
            this.HasThirdPartyInvoice = hasThirdPartyInvoice;
		
        }		

        public int Id { get; set; }

        public int AccountKey { get; set; }

        public int ProductKey { get; set; }

        public bool HasThirdPartyInvoice { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}