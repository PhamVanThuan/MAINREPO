using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetLatestDecisionTreesPubOnlyQueryStatement : IServiceQuerySqlStatement<GetLatestDecisionTreesPubOnlyQuery, GetLatestDecisionTreesPubOnlyQueryResult>
    {
        public string GetStatement()
        {
            return @"with full_result as (
                SELECT dt.Id DecisionTreeId, dt.Name, dt.[Description], dt.IsActive, dt.Thumbnail, dtv.Id DecisionTreeVersionId, Max(dtv.[Version]) LatestVersion, IIF(pdt.Id Is Null, 0, 1) IsPublished, dtv.Version thisVersion
                FROM [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK)
                JOIN [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK) on dtv.DecisionTreeId = dt.Id
                LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt (NOLOCK) on pdt.DecisionTreeVersionId = dtv.Id
                WHERE pdt.Id is not Null
                GROUP BY dt.Id, dt.Name, dt.[Description], dt.IsActive, dt.Thumbnail, dtv.Id, pdt.Id, dtv.Version
                 )

                SELECT full_result.DecisionTreeId, full_result.Name, full_result.Description, full_result.IsActive, full_result.Thumbnail, full_result.DecisionTreeVersionId, full_result.LatestVersion, full_result.IsPublished
                FROM full_result INNER JOIN (
                SELECT DecisionTreeId, MAX(thisVersion) AS MaxVersion
                    FROM full_result
                    GROUP BY DecisionTreeId
                    ) groupeddtv ON full_result.DecisionTreeId = groupeddtv.DecisionTreeId AND full_result.thisVersion = groupeddtv.MaxVersion
                ORDER BY full_result.Name ASC";
                        }
    }
}