using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetLatestEnumerationSetQueryStatement : IServiceQuerySqlStatement<GetLatestEnumerationSetQuery, GetEnumerationSetQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 es.[Id],es.[Version],es.[Data],CASE WHEN pes.Publisher IS NULL THEN 0 ELSE 1 END as IsPublished,ISNULL(pes.Publisher,'') as Publisher,cod.Username as LockedBy
                FROM [DecisionTree].[dbo].[EnumerationSet] es (NOLOCK)
                LEFT OUTER JOIN [DecisionTree].[dbo].[PublishedEnumerationSet] pes (NOLOCK) ON pes.EnumerationSetId = es.Id
                LEFT OUTER JOIN [DecisionTree].[dbo].[CurrentlyOpenDocument] cod (NOLOCK) ON cod.DocumentVersionId = es.Id
                ORDER BY [Version] DESC";
        }
    }
}