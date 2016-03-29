using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    public class GetAllEnumerationVersionsQueryStatement : IServiceQuerySqlStatement<GetAllEnumerationVersionsQuery, GetAllEnumerationVersionsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT es.[Id],es.[Version],CASE WHEN PublishDate IS NULL THEN 0 ELSE 1 END as IsPublished,pes.PublishDate,pes.Publisher
                    FROM [DecisionTree].[dbo].[EnumerationSet] es
                    LEFT OUTER JOIN [DecisionTree].[dbo].[PublishedEnumerationSet] pes ON es.Id = pes.EnumerationSetId
                    ORDER BY es.[Version] DESC";
        }
    }
}