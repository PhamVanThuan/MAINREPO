using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditHOCDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditHOCDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime? auditDate, string auditAddUpdateDelete, int financialServiceKey, int? hOCInsurerKey, string hOCPolicyNumber, double? hOCProrataPremium, double? hOCMonthlyPremium, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, double? hOCTotalSumInsured, int? hOCSubsidenceKey, int? hOCConstructionKey, int? hOCRoofKey, int? hOCStatusID, bool? hOCSBICFlag, double? capitalizedMonthlyBalance, DateTime? commencementDate, DateTime? anniversaryDate, string userID, DateTime? changeDate, int hOCStatusKey, bool ceded, string sAHLPolicyNumber, DateTime? cancellationDate, int? hOCHistoryKey, double? hOCAdministrationFee, double? hOCBasePremium, double? sASRIAAmount, int? hOCRatesKey, double? hOCBaseConventional, double? hOCBaseThatch, double? hOCBaseShingle)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceKey = financialServiceKey;
            this.HOCInsurerKey = hOCInsurerKey;
            this.HOCPolicyNumber = hOCPolicyNumber;
            this.HOCProrataPremium = hOCProrataPremium;
            this.HOCMonthlyPremium = hOCMonthlyPremium;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.HOCTotalSumInsured = hOCTotalSumInsured;
            this.HOCSubsidenceKey = hOCSubsidenceKey;
            this.HOCConstructionKey = hOCConstructionKey;
            this.HOCRoofKey = hOCRoofKey;
            this.HOCStatusID = hOCStatusID;
            this.HOCSBICFlag = hOCSBICFlag;
            this.CapitalizedMonthlyBalance = capitalizedMonthlyBalance;
            this.CommencementDate = commencementDate;
            this.AnniversaryDate = anniversaryDate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.HOCStatusKey = hOCStatusKey;
            this.Ceded = ceded;
            this.SAHLPolicyNumber = sAHLPolicyNumber;
            this.CancellationDate = cancellationDate;
            this.HOCHistoryKey = hOCHistoryKey;
            this.HOCAdministrationFee = hOCAdministrationFee;
            this.HOCBasePremium = hOCBasePremium;
            this.SASRIAAmount = sASRIAAmount;
            this.HOCRatesKey = hOCRatesKey;
            this.HOCBaseConventional = hOCBaseConventional;
            this.HOCBaseThatch = hOCBaseThatch;
            this.HOCBaseShingle = hOCBaseShingle;
		
        }
		[JsonConstructor]
        public AuditHOCDataModel(int auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime? auditDate, string auditAddUpdateDelete, int financialServiceKey, int? hOCInsurerKey, string hOCPolicyNumber, double? hOCProrataPremium, double? hOCMonthlyPremium, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, double? hOCTotalSumInsured, int? hOCSubsidenceKey, int? hOCConstructionKey, int? hOCRoofKey, int? hOCStatusID, bool? hOCSBICFlag, double? capitalizedMonthlyBalance, DateTime? commencementDate, DateTime? anniversaryDate, string userID, DateTime? changeDate, int hOCStatusKey, bool ceded, string sAHLPolicyNumber, DateTime? cancellationDate, int? hOCHistoryKey, double? hOCAdministrationFee, double? hOCBasePremium, double? sASRIAAmount, int? hOCRatesKey, double? hOCBaseConventional, double? hOCBaseThatch, double? hOCBaseShingle)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceKey = financialServiceKey;
            this.HOCInsurerKey = hOCInsurerKey;
            this.HOCPolicyNumber = hOCPolicyNumber;
            this.HOCProrataPremium = hOCProrataPremium;
            this.HOCMonthlyPremium = hOCMonthlyPremium;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.HOCTotalSumInsured = hOCTotalSumInsured;
            this.HOCSubsidenceKey = hOCSubsidenceKey;
            this.HOCConstructionKey = hOCConstructionKey;
            this.HOCRoofKey = hOCRoofKey;
            this.HOCStatusID = hOCStatusID;
            this.HOCSBICFlag = hOCSBICFlag;
            this.CapitalizedMonthlyBalance = capitalizedMonthlyBalance;
            this.CommencementDate = commencementDate;
            this.AnniversaryDate = anniversaryDate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.HOCStatusKey = hOCStatusKey;
            this.Ceded = ceded;
            this.SAHLPolicyNumber = sAHLPolicyNumber;
            this.CancellationDate = cancellationDate;
            this.HOCHistoryKey = hOCHistoryKey;
            this.HOCAdministrationFee = hOCAdministrationFee;
            this.HOCBasePremium = hOCBasePremium;
            this.SASRIAAmount = sASRIAAmount;
            this.HOCRatesKey = hOCRatesKey;
            this.HOCBaseConventional = hOCBaseConventional;
            this.HOCBaseThatch = hOCBaseThatch;
            this.HOCBaseShingle = hOCBaseShingle;
		
        }		

        public int AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime? AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int FinancialServiceKey { get; set; }

        public int? HOCInsurerKey { get; set; }

        public string HOCPolicyNumber { get; set; }

        public double? HOCProrataPremium { get; set; }

        public double? HOCMonthlyPremium { get; set; }

        public double? HOCThatchAmount { get; set; }

        public double? HOCConventionalAmount { get; set; }

        public double? HOCShingleAmount { get; set; }

        public double? HOCTotalSumInsured { get; set; }

        public int? HOCSubsidenceKey { get; set; }

        public int? HOCConstructionKey { get; set; }

        public int? HOCRoofKey { get; set; }

        public int? HOCStatusID { get; set; }

        public bool? HOCSBICFlag { get; set; }

        public double? CapitalizedMonthlyBalance { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? AnniversaryDate { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int HOCStatusKey { get; set; }

        public bool Ceded { get; set; }

        public string SAHLPolicyNumber { get; set; }

        public DateTime? CancellationDate { get; set; }

        public int? HOCHistoryKey { get; set; }

        public double? HOCAdministrationFee { get; set; }

        public double? HOCBasePremium { get; set; }

        public double? SASRIAAmount { get; set; }

        public int? HOCRatesKey { get; set; }

        public double? HOCBaseConventional { get; set; }

        public double? HOCBaseThatch { get; set; }

        public double? HOCBaseShingle { get; set; }

        public void SetKey(int key)
        {
            this.AuditNumber =  key;
        }
    }
}