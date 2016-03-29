using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetLatestVariableSetQueryStatement : IServiceQuerySqlStatement<GetLatestVariableSetQuery, GetLatestVariableSetQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 vs.[Id],vs.[Version],vs.[Data],CASE WHEN pvs.Publisher IS NULL THEN 0 ELSE 1 END as IsPublished,ISNULL(pvs.Publisher,'') as Publisher,cod.Username as LockedBy
                FROM [DecisionTree].[dbo].[VariableSet] vs (NOLOCK)
                LEFT OUTER JOIN [DecisionTree].[dbo].[PublishedVariableSet] pvs (NOLOCK) ON pvs.VariableSetId = vs.Id
                LEFT OUTER JOIN [DecisionTree].[dbo].[CurrentlyOpenDocument] cod (NOLOCK) ON cod.DocumentVersionId = vs.Id
                ORDER BY [Version] DESC";
        }
    }
}