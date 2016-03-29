using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetAccountByAccountNumberQueryStatement : IServiceQuerySqlStatement<GetAccountByAccountNumberQuery, GetAccountByAccountNumberQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT [AccountKey]
                      ,[FixedPayment]
                      ,[AccountStatusKey]
                      ,[InsertedDate]
                      ,[OriginationSourceProductKey]
                      ,[OpenDate]
                      ,[CloseDate]
                      ,[RRR_ProductKey]
                      ,[RRR_OriginationSourceKey]
                      ,[UserID]
                      ,[ChangeDate]
                      ,[SPVKey]
                      ,[ParentAccountKey]
                  FROM [2AM].[dbo].[Account]
                  WHERE [AccountKey] = @AccountNumber";
        }
    }
}
