using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetLatestDecisionTreesQueryStatement : IServiceQuerySqlStatement<GetLatestDecisionTreesQuery, GetLatestDecisionTreesQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT dt.Id DecisionTreeId, dt.Name, dt.[Description], dt.IsActive, dt.Thumbnail, dtv.Id DecisionTreeVersionId, IIF(pdt.Id Is Null, 0, 1) IsPublished, dtv.[Version] ThisVersion,
                       codt.Username AS CurrentlyOpenBy
                FROM [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) 
                JOIN [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK) on dtv.DecisionTreeId = dt.Id 
                JOIN ( 
                        SELECT d.DecisionTreeId, MAX(d.[Version]) as MaxVersion, IIF(pdt.Id Is Null, 0, 1) as IsPublished 
                        FROM [DecisionTree].[dbo].[DecisionTreeVersion] d (NOLOCK) 
                        LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt (NOLOCK) on pdt.DecisionTreeVersionId = d.Id 
                        GROUP BY d.DecisionTreeId, IIF(pdt.Id Is Null, 0, 1) 
                ) mxDTV ON mxDTV.DecisionTreeId = dt.Id AND mxDTV.MaxVersion = dtv.Version 
                LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt (NOLOCK) on pdt.DecisionTreeVersionId = dtv.Id 
                LEFT JOIN [DecisionTree].[dbo].[CurrentlyOpenDocument] codt (NOLOCK) ON codt.DocumentVersionId = dtv.Id
                ORDER BY dt.Name ASC, dt.Id, dtv.[Version]";
        }
    }
}