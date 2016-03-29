using SAHL.Core.Data;
using SAHL.Services.Query.Models.Finance;

namespace SAHL.Services.Query.DataManagers.Statements.Finance
{
    public class GetThirdPartyInvoiceStatement : ISqlStatement<ThirdPartyInvoiceDataModel>
    {
        public string GetStatement()
        {
            return @"Select
                TPI.ThirdPartyInvoiceKey As Id,
                TP.LegalEntityKey As LegalEntityKey,
                TP.Id As ThirdPartyId,
                TP.ThirdPartyKey As ThirdPartyKey,
                TPI.AccountKey As AccountKey,
                TPI.InvoiceStatusKey As InvoiceStatusKey,
                ST.Description As InvoiceStatus,
                Coalesce(TPI.InvoiceNumber,'') As InvoiceNumber,
                TPI.InvoiceDate  As InvoiceDate,
                Coalesce(TPI.SahlReference, '') as SahlReference,
                Coalesce(TPI.ReceivedFromEmailAddress, '') As ReceivedFromEmailAddress,
                TPI.ReceivedDate  As ReceivedDate,
                TPI.CapitaliseInvoice As CapitaliseInvoice,
                Coalesce(TPI.AmountExcludingVAT, 0) As AmountExcludingVAT,
                Coalesce(TPI.VATAmount, 0) As VATAmount,
                Coalesce(TPI.TotalAmountIncludingVAT, 0) As TotalAmountIncludingVAT,
                [2am].[dbo].[AccountLegalName](TPI.AccountKey,0) as ClientName,
                TPI.PaymentReference
          From [2AM].[dbo].[ThirdPartyInvoice] TPI
            Left Join [2AM].[dbo].[ThirdParty] TP
                On TP.Id = TPI.ThirdPartyId
            Join [2AM].[dbo].[InvoiceStatus] ST
                On ST.InvoiceStatusKey = TPI.InvoiceStatusKey";
        }
    }
}