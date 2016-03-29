using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UiStatementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UiStatementDataModel(string applicationName, string statementName, DateTime modifyDate, int version, string modifyUser, string statement, int type, DateTime lastAccessedDate)
        {
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
            this.ModifyDate = modifyDate;
            this.Version = version;
            this.ModifyUser = modifyUser;
            this.Statement = statement;
            this.Type = type;
            this.LastAccessedDate = lastAccessedDate;
		
        }
		[JsonConstructor]
        public UiStatementDataModel(int statementKey, string applicationName, string statementName, DateTime modifyDate, int version, string modifyUser, string statement, int type, DateTime lastAccessedDate)
        {
            this.StatementKey = statementKey;
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
            this.ModifyDate = modifyDate;
            this.Version = version;
            this.ModifyUser = modifyUser;
            this.Statement = statement;
            this.Type = type;
            this.LastAccessedDate = lastAccessedDate;
		
        }		

        public int StatementKey { get; set; }

        public string ApplicationName { get; set; }

        public string StatementName { get; set; }

        public DateTime ModifyDate { get; set; }

        public int Version { get; set; }

        public string ModifyUser { get; set; }

        public string Statement { get; set; }

        public int Type { get; set; }

        public DateTime LastAccessedDate { get; set; }

        public void SetKey(int key)
        {
            this.StatementKey =  key;
        }
    }
}