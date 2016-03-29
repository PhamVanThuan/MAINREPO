using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetThirdPartyInvoiceByAccountKeyQueryStatement : IServiceQuerySqlStatement<GetThirdPartyInvoiceByAccountKeyQuery, GetThirdPartyInvoiceByAccountKeyQueryResult>
    {
        public int AccountKey { get; protected set; }
        public GetThirdPartyInvoiceByAccountKeyQueryStatement(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public string GetStatement()
        {
            string query = @"SELECT TOP 1 [ThirdPartyInvoiceKey]
                              ,[SahlReference]
                              ,[InvoiceStatusKey]
                              ,[AccountKey]
                              ,[InvoiceNumber]
                          FROM [2AM].[dbo].[ThirdPartyInvoice]
                          WHERE [AccountKey] = @AccountKey";
            return query;
        }
    }
}