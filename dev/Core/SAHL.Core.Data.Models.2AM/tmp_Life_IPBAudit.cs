using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class Tmp_Life_IPBAuditDataModel :  IDataModel
    {
        public Tmp_Life_IPBAuditDataModel(int financialServiceKey, int accountKey, int legalEntityKey, DateTime? dateOfAcceptance, int policyStatusKey, DateTime? dateAddedToPolicy, string sourceTable, int? sourceTablePrimaryKey, int? ageNext, int? ageAtAcceptance, int? ageAtDateAdded)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.AccountKey = accountKey;
            this.LegalEntityKey = legalEntityKey;
            this.DateOfAcceptance = dateOfAcceptance;
            this.PolicyStatusKey = policyStatusKey;
            this.DateAddedToPolicy = dateAddedToPolicy;
            this.SourceTable = sourceTable;
            this.SourceTablePrimaryKey = sourceTablePrimaryKey;
            this.AgeNext = ageNext;
            this.AgeAtAcceptance = ageAtAcceptance;
            this.AgeAtDateAdded = ageAtDateAdded;
		
        }		

        public int FinancialServiceKey { get; set; }

        public int AccountKey { get; set; }

        public int LegalEntityKey { get; set; }

        public DateTime? DateOfAcceptance { get; set; }

        public int PolicyStatusKey { get; set; }

        public DateTime? DateAddedToPolicy { get; set; }

        public string SourceTable { get; set; }

        public int? SourceTablePrimaryKey { get; set; }

        public int? AgeNext { get; set; }

        public int? AgeAtAcceptance { get; set; }

        public int? AgeAtDateAdded { get; set; }
    }
}