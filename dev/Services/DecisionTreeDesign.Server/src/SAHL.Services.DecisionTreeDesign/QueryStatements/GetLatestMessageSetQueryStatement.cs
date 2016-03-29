using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetLatestMessageSetQueryStatement : IServiceQuerySqlStatement<GetLatestMessageSetQuery, MessageSetQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 ms.Id, ms.[Version], ms.Data, IIF(pms.MessageSetId Is Null, 0, 1) IsPublished,cod.Username as LockedBy
                FROM [DecisionTree].[dbo].[MessageSet] ms (NOLOCK)
                LEFT JOIN [DecisionTree].[dbo].[PublishedMessageSet] pms (NOLOCK) ON pms.MessageSetId = ms.Id
                LEFT JOIN [DecisionTree].[dbo].[CurrentlyOpenDocument] cod (NOLOCK) ON cod.DocumentVersionId = ms.Id
                ORDER BY [Version] DESC ";
        }
    }
}