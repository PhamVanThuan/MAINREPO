using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Services.FinanceDomain.QueryStatements
{
    public class GetThirdPartyInvoiceCorrespondenceQueryStatement : IServiceQuerySqlStatement<GetThirdPartyInvoiceCorrespondenceQuery, GetThirdPartyInvoiceCorrespondenceQueryResult>
    {
        public string GetStatement()
        {
            var query = string.Format(
                          @"SELECT [Id]
                          ,[CorrespondenceType]
                          ,[CorrespondenceReason]
                          ,[CorrespondenceMedium]
                          ,[Date]
                          ,[UserName]
                          ,[MemoText] as QueryText
                          ,[GenericKey] as ThirdPartyInvoiceKey
                          ,[GenericKeyTypeKey]
                      FROM [EventProjection].[projection].[Correspondence]
                      WHERE
	                       [GenericKey] = @ThirdPartyInvoiceKey
	                       AND
	                       [GenericKeyTypeKey] = {0}
                      ORDER BY [Date] DESC", (int)GenericKeyType.ThirdPartyInvoice);

            return query;
        }
    }
}