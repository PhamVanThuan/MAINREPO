using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class FinancialServiceBankAccountModel
    {
        public int FinancialServiceBankAccountKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int? BankAccountKey { get; set; }

        public double Percentage { get; set; }

        public int? DebitOrderDay { get; set; }

        public int GeneralStatusKey { get; set; }

        public string UserID { get; set; }

        public DateTime ChangeDate { get; set; }

        public FinancialServicePaymentTypeEnum FinancialServicePaymentTypeKey { get; set; }

        public int PaymentSplitTypeKey { get; set; }

        public int ProviderKey { get; set; }

        public bool IsNaedoCompliant { get; set; }
    }
}