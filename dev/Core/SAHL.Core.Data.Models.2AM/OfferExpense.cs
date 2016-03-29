using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferExpenseDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferExpenseDataModel(int? offerKey, int? legalEntityKey, int? expenseTypeKey, string expenseAccountNumber, string expenseAccountName, string expenseReference, double? totalOutstandingAmount, double? monthlyPayment, bool? toBeSettled, bool? overRidden)
        {
            this.OfferKey = offerKey;
            this.LegalEntityKey = legalEntityKey;
            this.ExpenseTypeKey = expenseTypeKey;
            this.ExpenseAccountNumber = expenseAccountNumber;
            this.ExpenseAccountName = expenseAccountName;
            this.ExpenseReference = expenseReference;
            this.TotalOutstandingAmount = totalOutstandingAmount;
            this.MonthlyPayment = monthlyPayment;
            this.ToBeSettled = toBeSettled;
            this.OverRidden = overRidden;
		
        }
		[JsonConstructor]
        public OfferExpenseDataModel(int offerExpenseKey, int? offerKey, int? legalEntityKey, int? expenseTypeKey, string expenseAccountNumber, string expenseAccountName, string expenseReference, double? totalOutstandingAmount, double? monthlyPayment, bool? toBeSettled, bool? overRidden)
        {
            this.OfferExpenseKey = offerExpenseKey;
            this.OfferKey = offerKey;
            this.LegalEntityKey = legalEntityKey;
            this.ExpenseTypeKey = expenseTypeKey;
            this.ExpenseAccountNumber = expenseAccountNumber;
            this.ExpenseAccountName = expenseAccountName;
            this.ExpenseReference = expenseReference;
            this.TotalOutstandingAmount = totalOutstandingAmount;
            this.MonthlyPayment = monthlyPayment;
            this.ToBeSettled = toBeSettled;
            this.OverRidden = overRidden;
		
        }		

        public int OfferExpenseKey { get; set; }

        public int? OfferKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public int? ExpenseTypeKey { get; set; }

        public string ExpenseAccountNumber { get; set; }

        public string ExpenseAccountName { get; set; }

        public string ExpenseReference { get; set; }

        public double? TotalOutstandingAmount { get; set; }

        public double? MonthlyPayment { get; set; }

        public bool? ToBeSettled { get; set; }

        public bool? OverRidden { get; set; }

        public void SetKey(int key)
        {
            this.OfferExpenseKey =  key;
        }
    }
}