using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountStatusDataModel :  IDataModel
    {
        public AccountStatusDataModel(int accountStatusKey, string description)
        {
            this.AccountStatusKey = accountStatusKey;
            this.Description = description;
		
        }		

        public int AccountStatusKey { get; set; }

        public string Description { get; set; }
    }
}