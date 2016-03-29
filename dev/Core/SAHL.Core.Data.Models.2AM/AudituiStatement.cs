using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AudituiStatementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AudituiStatementDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int statementKey, string applicationName, string statementName, DateTime modifyDate, int version, string modifyUser, string statement, int type, DateTime? lastAccessedDate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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
		[JsonConstructor]
        public AudituiStatementDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int statementKey, string applicationName, string statementName, DateTime modifyDate, int version, string modifyUser, string statement, int type, DateTime? lastAccessedDate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int StatementKey { get; set; }

        public string ApplicationName { get; set; }

        public string StatementName { get; set; }

        public DateTime ModifyDate { get; set; }

        public int Version { get; set; }

        public string ModifyUser { get; set; }

        public string Statement { get; set; }

        public int Type { get; set; }

        public DateTime? LastAccessedDate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}