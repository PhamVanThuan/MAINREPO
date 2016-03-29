using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ITC.Queries;

namespace SAHL.Services.ITC.QueryStatements
{
    public class GetITCQueryStatement : IServiceQuerySqlStatement<GetITCQuery, ITCRequestDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT top 1 [Id]
                          ,[ITCDate]
                          ,[ITCData]
                      FROM [2AM].[capitec].[ITCRequest]
                      Where [Id] = @ItcID
                      Order by [ITCDate] desc";
        }
    }
}