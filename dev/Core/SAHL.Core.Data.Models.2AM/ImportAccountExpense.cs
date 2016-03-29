using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportAccountExpenseDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportAccountExpenseDataModel(int offerKey, string expenseTypeKey, string expenseAccountNumber, string expenseAccountName, string expenseReference, double? totalOutstandingAmount, double? monthlyPayment, bool? toBeSettled)
        {
            this.OfferKey = offerKey;
            this.ExpenseTypeKey = expenseTypeKey;
            this.ExpenseAccountNumber = expenseAccountNumber;
            this.ExpenseAccountName = expenseAccountName;
            this.ExpenseReference = expenseReference;
            this.TotalOutstandingAmount = totalOutstandingAmount;
            this.MonthlyPayment = monthlyPayment;
            this.ToBeSettled = toBeSettled;
		
        }
		[JsonConstructor]
        public ImportAccountExpenseDataModel(int accountExpenseKey, int offerKey, string expenseTypeKey, string expenseAccountNumber, string expenseAccountName, string expenseReference, double? totalOutstandingAmount, double? monthlyPayment, bool? toBeSettled)
        {
            this.AccountExpenseKey = accountExpenseKey;
            this.OfferKey = offerKey;
            this.ExpenseTypeKey = expenseTypeKey;
            this.ExpenseAccountNumber = expenseAccountNumber;
            this.ExpenseAccountName = expenseAccountName;
            this.ExpenseReference = expenseReference;
            this.TotalOutstandingAmount = totalOutstandingAmount;
            this.MonthlyPayment = monthlyPayment;
            this.ToBeSettled = toBeSettled;
		
        }		

        public int AccountExpenseKey { get; set; }

        public int OfferKey { get; set; }

        public string ExpenseTypeKey { get; set; }

        public string ExpenseAccountNumber { get; set; }

        public string ExpenseAccountName { get; set; }

        public string ExpenseReference { get; set; }

        public double? TotalOutstandingAmount { get; set; }

        public double? MonthlyPayment { get; set; }

        public bool? ToBeSettled { get; set; }

        public void SetKey(int key)
        {
            this.AccountExpenseKey =  key;
        }
    }
}