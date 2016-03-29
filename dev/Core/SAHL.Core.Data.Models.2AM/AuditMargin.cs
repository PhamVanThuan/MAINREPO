using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditMarginDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditMarginDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime? auditDate, string auditAddUpdateDelete, int marginKey, double value, string description)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MarginKey = marginKey;
            this.Value = value;
            this.Description = description;
		
        }
		[JsonConstructor]
        public AuditMarginDataModel(int auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime? auditDate, string auditAddUpdateDelete, int marginKey, double value, string description)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MarginKey = marginKey;
            this.Value = value;
            this.Description = description;
		
        }		

        public int AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime? AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int MarginKey { get; set; }

        public double Value { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.AuditNumber =  key;
        }
    }
}