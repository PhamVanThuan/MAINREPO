using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountIndicationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountIndicationDataModel(int accountIndicator, int accountIndicationTypeKey, string indicator, string userID, DateTime dateChange)
        {
            this.AccountIndicator = accountIndicator;
            this.AccountIndicationTypeKey = accountIndicationTypeKey;
            this.Indicator = indicator;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }
		[JsonConstructor]
        public AccountIndicationDataModel(int accountIndicationKey, int accountIndicator, int accountIndicationTypeKey, string indicator, string userID, DateTime dateChange)
        {
            this.AccountIndicationKey = accountIndicationKey;
            this.AccountIndicator = accountIndicator;
            this.AccountIndicationTypeKey = accountIndicationTypeKey;
            this.Indicator = indicator;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }		

        public int AccountIndicationKey { get; set; }

        public int AccountIndicator { get; set; }

        public int AccountIndicationTypeKey { get; set; }

        public string Indicator { get; set; }

        public string UserID { get; set; }

        public DateTime DateChange { get; set; }

        public void SetKey(int key)
        {
            this.AccountIndicationKey =  key;
        }
    }
}