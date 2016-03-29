using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    public class GetPublishedMessageSetByMessageSetIdQueryStatement : IServiceQuerySqlStatement<GetPublishedMessageSetByMessageSetIdQuery, PublishedMessageSetQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT ms.Id MessageSetId, ms.[Version], ms.Data, pms.Id PublishedMessageSetId, PublishDate, Publisher, ps.Name PublishStatus
                FROM [DecisionTree].[dbo].[MessageSet] ms
                JOIN [DecisionTree].[dbo].[PublishedMessageSet] pms ON pms.MessageSetId = ms.Id
                JOIN [DecisionTree].[dbo].[PublishStatusEnum] ps ON ps.Id = pms.PublishStatusId
                WHERE ms.Id = @MessageSetId";
        }
    }
}