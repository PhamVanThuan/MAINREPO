using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class SPVsDataModel :  IDataModel
    {
        public SPVsDataModel(int sPVKey, string description, string reportDescription, int parentSPVKey, string parentSPVDescription, string parentSPVReportDescription, int sPVCompanyKey, string sPVCompanyDescription, int generalStatusKey, int bankAccountKey, string bankName, string branchCode, string branchName, string accountName, string accountNumber, string accountType)
        {
            this.SPVKey = sPVKey;
            this.Description = description;
            this.ReportDescription = reportDescription;
            this.ParentSPVKey = parentSPVKey;
            this.ParentSPVDescription = parentSPVDescription;
            this.ParentSPVReportDescription = parentSPVReportDescription;
            this.SPVCompanyKey = sPVCompanyKey;
            this.SPVCompanyDescription = sPVCompanyDescription;
            this.GeneralStatusKey = generalStatusKey;
            this.BankAccountKey = bankAccountKey;
            this.BankName = bankName;
            this.BranchCode = branchCode;
            this.BranchName = branchName;
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
            this.AccountType = accountType;
		
        }		

        public int SPVKey { get; set; }

        public string Description { get; set; }

        public string ReportDescription { get; set; }

        public int ParentSPVKey { get; set; }

        public string ParentSPVDescription { get; set; }

        public string ParentSPVReportDescription { get; set; }

        public int SPVCompanyKey { get; set; }

        public string SPVCompanyDescription { get; set; }

        public int GeneralStatusKey { get; set; }

        public int BankAccountKey { get; set; }

        public string BankName { get; set; }

        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountType { get; set; }
    }
}