using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountIndicationTypeDataModel :  IDataModel
    {
        public AccountIndicationTypeDataModel(int accountIndicationTypeKey, string description)
        {
            this.AccountIndicationTypeKey = accountIndicationTypeKey;
            this.Description = description;
		
        }		

        public int AccountIndicationTypeKey { get; set; }

        public string Description { get; set; }
    }
}