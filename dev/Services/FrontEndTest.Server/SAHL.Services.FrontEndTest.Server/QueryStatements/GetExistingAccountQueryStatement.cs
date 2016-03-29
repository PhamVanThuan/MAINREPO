using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetExistingAccountQueryStatement : IServiceQuerySqlStatement<GetExistingAccountQuery, int>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 [AccountKey] FROM [2AM].[dbo].[Account] order by newid()";
        }
    }
}
