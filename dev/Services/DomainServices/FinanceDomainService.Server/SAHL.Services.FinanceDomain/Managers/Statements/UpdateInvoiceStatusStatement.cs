using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.FinanceDomain.Managers.Statements
{
    public class UpdateInvoiceStatusStatement : ISqlStatement<bool>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public int ThirdPartyInvoiceStatus { get; protected set; }

        public UpdateInvoiceStatusStatement(int thirdPartyInvoiceKey, InvoiceStatus thirdPartyInvoiceStatus)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            ThirdPartyInvoiceStatus = (int)thirdPartyInvoiceStatus;
        }

        public string GetStatement()
        {
            return @"UPDATE [2AM].dbo.[ThirdPartyInvoice] SET InvoiceStatusKey = @ThirdPartyInvoiceStatus
                    WHERE ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";
        }
    }
}
