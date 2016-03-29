using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditResetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditResetDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int resetKey, DateTime resetDate, DateTime runDate, int resetConfigurationKey, double jIBARRate, double jIBARDiscountRate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.ResetKey = resetKey;
            this.ResetDate = resetDate;
            this.RunDate = runDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.JIBARRate = jIBARRate;
            this.JIBARDiscountRate = jIBARDiscountRate;
		
        }
		[JsonConstructor]
        public AuditResetDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int resetKey, DateTime resetDate, DateTime runDate, int resetConfigurationKey, double jIBARRate, double jIBARDiscountRate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.ResetKey = resetKey;
            this.ResetDate = resetDate;
            this.RunDate = runDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.JIBARRate = jIBARRate;
            this.JIBARDiscountRate = jIBARDiscountRate;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int ResetKey { get; set; }

        public DateTime ResetDate { get; set; }

        public DateTime RunDate { get; set; }

        public int ResetConfigurationKey { get; set; }

        public double JIBARRate { get; set; }

        public double JIBARDiscountRate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}