using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TokenTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TokenTypeDataModel(string description, string userID, bool runStatement)
        {
            this.Description = description;
            this.UserID = userID;
            this.RunStatement = runStatement;
		
        }
		[JsonConstructor]
        public TokenTypeDataModel(int tokenTypeKey, string description, string userID, bool runStatement)
        {
            this.TokenTypeKey = tokenTypeKey;
            this.Description = description;
            this.UserID = userID;
            this.RunStatement = runStatement;
		
        }		

        public int TokenTypeKey { get; set; }

        public string Description { get; set; }

        public string UserID { get; set; }

        public bool RunStatement { get; set; }

        public void SetKey(int key)
        {
            this.TokenTypeKey =  key;
        }
    }
}