using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetLatestEventForGenericKeyTypeQueryStatement : IServiceQuerySqlStatement<GetLatestEventForGenericKeyTypeQuery, GetLatestEventForGenericKeyTypeQueryResult>
    {

        public string GetStatement()
        {
            return @"SELECT TOP 1 [EventKey]
                          ,[GenericKey]
                          ,[GenericKeyTypeKey]
                          ,[Data]
                          ,[EventInsertDate]
                      FROM [EventStore].[event].[Event]
                      where [GenericKeyTypeKey] = @GenericKeyTypeKey
                      order by  [EventKey] desc";
        }
    }
}