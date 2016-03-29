using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UiStatementTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UiStatementTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public UiStatementTypeDataModel(int statementTypeKey, string description)
        {
            this.StatementTypeKey = statementTypeKey;
            this.Description = description;
		
        }		

        public int StatementTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.StatementTypeKey =  key;
        }
    }
}