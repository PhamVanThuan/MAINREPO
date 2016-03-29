using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class FinancialService : IComparable
    {
        public int DebitOrderDay { get; set; }

        public int FinancialServicePaymentTypeKey { get; set; }

        public GeneralStatusEnum FinancialServiceBankAccountGeneralStatusKey { get; set; }

        public string AccountNumber { get; set; }

        public string ACBBankDescription { get; set; }

        public int FinancialServiceKey { get; set; }

        public string ACBBranchCode { get; set; }

        public string ACBBranchDescription { get; set; }

        public string ACBTypeDescription { get; set; }

        public OriginationSourceEnum OriginationSourceKey { get; set; }

        public GeneralStatusEnum AccountStatusKey { get; set; }

        public AccountStatusEnum FinancialServiceStatusKey { get; set; }

        public Account Account { get; set; }

        public int CompareTo(object obj)
        {
            var comparingObj = obj as FinancialService;

            if (comparingObj.Account.AccountKey != this.Account.AccountKey)
                return 0;
            if (comparingObj.DebitOrderDay != this.DebitOrderDay)
                return 0;
            if (comparingObj.FinancialServiceBankAccountGeneralStatusKey != this.FinancialServiceBankAccountGeneralStatusKey)
                return 0;
            return 1;
        }

        public int AccountKey { get; set; }
    }

    public class Balance : IDataModel
    {
        public int FinancialServiceKey { get; set; }

        public int BalanceTypeKey { get; set; }

        public decimal Amount { get; set; }
    }
}