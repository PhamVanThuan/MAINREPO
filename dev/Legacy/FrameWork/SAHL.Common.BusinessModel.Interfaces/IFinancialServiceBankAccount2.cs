using System;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IFinancialServiceBankAccount
    {
        int DebitOrderDay { get; set; }

        void SetDebitOrderDay(DateTime effectiveDate, int DebitOrderDay);
    }
}