using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class UpdateThirdPartyInvoiceStatusStatement : ISqlStatement<ThirdPartyInvoiceDataModel>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public int InvoiceStatusKey { get; protected set; }

        public UpdateThirdPartyInvoiceStatusStatement(int thirdPartyInvoiceKey, InvoiceStatus invoiceStatus)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.InvoiceStatusKey = (int)invoiceStatus;
        }

        public string GetStatement()
        {
            var sql = @"UPDATE [2AM].[dbo].[ThirdPartyInvoice]
                       SET
                           [InvoiceStatusKey] = @InvoiceStatusKey
                     WHERE [ThirdPartyInvoiceKey] = @ThirdPartyInvoiceKey";

            return sql;
        }
    }
}