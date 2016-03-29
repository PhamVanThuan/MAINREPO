using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetCatsPaymentBatchItemInfoStatement : ISqlStatement<ThirdPartyInvoicePaymentBatchItem>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public int ThirdPartyPaymentBatchKey { get; protected set; }

        public GetCatsPaymentBatchItemInfoStatement(int thirdPartyPaymentBatchKey, int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            ThirdPartyPaymentBatchKey = thirdPartyPaymentBatchKey;
        }

        public string GetStatement()
        {
            return @"
                    SELECT
                        le.LegalEntityKey
                        ,@ThirdPartyInvoiceKey as GenericKey
                        ,gkt.GenericKeyTypeKey as GenericTypeKey
                        ,tpi.AccountKey as AccountKey
                        ,tpi.TotalAmountIncludingVat as InvoiceTotal
                        ,bacc.BankAccountKey as SourceBankAccountKey
                        ,Tbacc.BankAccountKey as TargetBankAccountKey	
                        ,tpi.SahlReference as SahlReferenceNumber
                        ,CONCAT('SAHL','      ','SPV ',sp.SPVKey) as SourceReference
                        ,@ThirdPartyPaymentBatchKey as 'ThirdPartyPaymentBatchKey'
                        ,le.TradingName as TargetName
                        ,tpi.InvoiceNumber
                        ,tpbacc.EmailAddress
                        ,tpi.PaymentReference
                    FROM [2AM].dbo.ThirdPartyInvoice as tpi
                        JOIN [2AM].dbo.Account as acc 
                            ON acc.AccountKey = tpi.AccountKey 
                        JOIN [2AM].spv.SPV as sp 
                            ON sp.SPVKey =acc.SPVKey 
                        JOIN [2AM].dbo.BankAccount as bacc 
                            ON bacc.BankAccountKey = sp.BankAccountKey 
                        JOIN [2AM].dbo.ThirdParty tp 
                            ON tp.Id = tpi.ThirdPartyId 
                        JOIN [2AM].dbo.LegalEntity le
                            ON le.LegalEntityKey = tp.LegalEntityKey
                        JOIN [2AM].dbo.GenericKeyType gkt
                            ON gkt.PrimaryKeyColumn = 'ThirdPartyInvoiceKey'
                        LEFT JOIN [2AM].dbo.ThirdPartyPaymentBankAccount tpbacc 
                            ON tpbacc.ThirdPartyKey = tp.ThirdPartyKey 
                        LEFT JOIN [2AM].dbo.BankAccount as Tbacc 
                            ON Tbacc.BankAccountKey = tpbacc.BankAccountKey 
                    WHERE tpi.ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey 
                        AND tpi.PaymentReference is not null
                        AND tpi.PaymentReference <> ''
";
        }
    }
}
