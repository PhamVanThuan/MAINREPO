
using SAHL.Core.BusinessModel.Enums;
namespace SAHL.Services.FinanceDomain.Managers
{
    public interface IFinanceDataManager
    {
        int? GetVariableLoanFinancialServiceKeyByAccount(int accountNumber);

        int GetFinancialTransactionKeyByReference(string reference);

        void UpdateInvoiceStatus(int thirdPartyInvoiceKey, InvoiceStatus invoiceStatus);
    }
}
