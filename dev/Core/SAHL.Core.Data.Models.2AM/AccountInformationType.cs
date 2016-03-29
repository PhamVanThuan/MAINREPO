using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountInformationTypeDataModel :  IDataModel
    {
        public AccountInformationTypeDataModel(int accountInformationTypeKey, string description)
        {
            this.AccountInformationTypeKey = accountInformationTypeKey;
            this.Description = description;
		
        }		

        public int AccountInformationTypeKey { get; set; }

        public string Description { get; set; }
    }
}