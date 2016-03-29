using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetClientBankAccountQueryStatement : IServiceQuerySqlStatement<GetClientBankAccountQuery, GetClientBankAccountQueryResult>
    {
        public string GetStatement()
        {
            var query = @"SELECT [BankAccountKey] FROM [2AM].[dbo].[LegalEntityBankAccount] WHERE [LegalEntityBankAccountKey] = @ClientBankAccountKey";
            return query;
        }
    }
}