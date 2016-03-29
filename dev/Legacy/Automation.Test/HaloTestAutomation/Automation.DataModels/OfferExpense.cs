using Common.Enums;

namespace Automation.DataModels
{
    public sealed class OfferExpense
    {
        public int OfferExpenseKey { get; set; }

        public int OfferKey { get; set; }

        public int LegalEntityKey { get; set; }

        public ExpenseTypeEnum ExpenseTypeKey { get; set; }

        public string ExpenseAccountNumber { get; set; }

        public string ExpenseAccountName { get; set; }

        public string ExpenseReference { get; set; }

        public double TotalOutstandingAmount { get; set; }

        public double MonthlyPayment { get; set; }

        public bool ToBeSettled { get; set; }

        public bool OverRidden { get; set; }
    }
}