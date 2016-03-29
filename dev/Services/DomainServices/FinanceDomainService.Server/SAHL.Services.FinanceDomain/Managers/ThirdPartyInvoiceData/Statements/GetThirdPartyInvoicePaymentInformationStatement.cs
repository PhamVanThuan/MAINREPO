using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetThirdPartyInvoicePaymentInformationStatement : ISqlStatement<ThirdPartyInvoicePaymentModel>
    {
        public int[] ThirdPartyInvoiceKeys { get; protected set; }

        public GetThirdPartyInvoicePaymentInformationStatement(int[] thirdPartyInvoiceKeys)
        {
            ThirdPartyInvoiceKeys = thirdPartyInvoiceKeys;
        }

        public string GetStatement()
        {
            return @"SELECT 
                        le.TradingName as FirmName,
                        le.EmailAddress,
                        tpi.AccountKey as AccountNumber,
                        tpi.TotalAmountIncludingVAT as InvoiceAmountIncludingVat,
                        tpi.InvoiceNumber,
                        tp.ThirdPartyKey
                    FROM 
                        [2AM].dbo.ThirdPartyInvoice tpi
                        join [2AM].dbo.ThirdParty tp on tpi.ThirdPartyId = tp.Id
                        join [2AM].dbo.LegalEntity le on le.LegalEntityKey = tp.LegalEntityKey
                    where 
                        tpi.ThirdPartyInvoiceKey in @ThirdPartyInvoiceKeys
                    ORDER BY le.TradingName, tpi.ReceivedDate";
        }
    }
}
