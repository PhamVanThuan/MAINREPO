using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ITCDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ITCDataModel(int legalEntityKey, int? accountKey, DateTime? changeDate, string responseXML, string responseStatus, string userID, string requestXML)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.ChangeDate = changeDate;
            this.ResponseXML = responseXML;
            this.ResponseStatus = responseStatus;
            this.UserID = userID;
            this.RequestXML = requestXML;
		
        }
		[JsonConstructor]
        public ITCDataModel(int iTCKey, int legalEntityKey, int? accountKey, DateTime? changeDate, string responseXML, string responseStatus, string userID, string requestXML)
        {
            this.ITCKey = iTCKey;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.ChangeDate = changeDate;
            this.ResponseXML = responseXML;
            this.ResponseStatus = responseStatus;
            this.UserID = userID;
            this.RequestXML = requestXML;
		
        }		

        public int ITCKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int? AccountKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ResponseXML { get; set; }

        public string ResponseStatus { get; set; }

        public string UserID { get; set; }

        public string RequestXML { get; set; }

        public void SetKey(int key)
        {
            this.ITCKey =  key;
        }
    }
}