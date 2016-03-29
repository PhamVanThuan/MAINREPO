using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TextStatementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TextStatementDataModel(int textStatementTypeKey, string statementTitle, string statement)
        {
            this.TextStatementTypeKey = textStatementTypeKey;
            this.StatementTitle = statementTitle;
            this.Statement = statement;
		
        }
		[JsonConstructor]
        public TextStatementDataModel(int textStatementKey, int textStatementTypeKey, string statementTitle, string statement)
        {
            this.TextStatementKey = textStatementKey;
            this.TextStatementTypeKey = textStatementTypeKey;
            this.StatementTitle = statementTitle;
            this.Statement = statement;
		
        }		

        public int TextStatementKey { get; set; }

        public int TextStatementTypeKey { get; set; }

        public string StatementTitle { get; set; }

        public string Statement { get; set; }

        public void SetKey(int key)
        {
            this.TextStatementKey =  key;
        }
    }
}