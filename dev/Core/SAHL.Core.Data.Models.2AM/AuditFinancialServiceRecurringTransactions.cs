using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditFinancialServiceRecurringTransactionsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditFinancialServiceRecurringTransactionsDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int financialServiceRecurringTransactionKey, int? financialServiceKey, int? recurringTransactionTypeKey, DateTime? insertDate, int? frequency, int? transactionTypeNumber, string frequencyType, int? numUntilNextRun, string reference, bool? active, DateTime? startDate, int? term, int? remainingTerm, int? transactionDay, int? hourOfRun, double? amount, string statementName, DateTime? previousRunDate, string userName, string notes, int? bankAccountKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceRecurringTransactionKey = financialServiceRecurringTransactionKey;
            this.FinancialServiceKey = financialServiceKey;
            this.RecurringTransactionTypeKey = recurringTransactionTypeKey;
            this.InsertDate = insertDate;
            this.Frequency = frequency;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.FrequencyType = frequencyType;
            this.NumUntilNextRun = numUntilNextRun;
            this.Reference = reference;
            this.Active = active;
            this.StartDate = startDate;
            this.Term = term;
            this.RemainingTerm = remainingTerm;
            this.TransactionDay = transactionDay;
            this.HourOfRun = hourOfRun;
            this.Amount = amount;
            this.StatementName = statementName;
            this.PreviousRunDate = previousRunDate;
            this.UserName = userName;
            this.Notes = notes;
            this.BankAccountKey = bankAccountKey;
		
        }
		[JsonConstructor]
        public AuditFinancialServiceRecurringTransactionsDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int financialServiceRecurringTransactionKey, int? financialServiceKey, int? recurringTransactionTypeKey, DateTime? insertDate, int? frequency, int? transactionTypeNumber, string frequencyType, int? numUntilNextRun, string reference, bool? active, DateTime? startDate, int? term, int? remainingTerm, int? transactionDay, int? hourOfRun, double? amount, string statementName, DateTime? previousRunDate, string userName, string notes, int? bankAccountKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.FinancialServiceRecurringTransactionKey = financialServiceRecurringTransactionKey;
            this.FinancialServiceKey = financialServiceKey;
            this.RecurringTransactionTypeKey = recurringTransactionTypeKey;
            this.InsertDate = insertDate;
            this.Frequency = frequency;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.FrequencyType = frequencyType;
            this.NumUntilNextRun = numUntilNextRun;
            this.Reference = reference;
            this.Active = active;
            this.StartDate = startDate;
            this.Term = term;
            this.RemainingTerm = remainingTerm;
            this.TransactionDay = transactionDay;
            this.HourOfRun = hourOfRun;
            this.Amount = amount;
            this.StatementName = statementName;
            this.PreviousRunDate = previousRunDate;
            this.UserName = userName;
            this.Notes = notes;
            this.BankAccountKey = bankAccountKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int FinancialServiceRecurringTransactionKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public int? RecurringTransactionTypeKey { get; set; }

        public DateTime? InsertDate { get; set; }

        public int? Frequency { get; set; }

        public int? TransactionTypeNumber { get; set; }

        public string FrequencyType { get; set; }

        public int? NumUntilNextRun { get; set; }

        public string Reference { get; set; }

        public bool? Active { get; set; }

        public DateTime? StartDate { get; set; }

        public int? Term { get; set; }

        public int? RemainingTerm { get; set; }

        public int? TransactionDay { get; set; }

        public int? HourOfRun { get; set; }

        public double? Amount { get; set; }

        public string StatementName { get; set; }

        public DateTime? PreviousRunDate { get; set; }

        public string UserName { get; set; }

        public string Notes { get; set; }

        public int? BankAccountKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}