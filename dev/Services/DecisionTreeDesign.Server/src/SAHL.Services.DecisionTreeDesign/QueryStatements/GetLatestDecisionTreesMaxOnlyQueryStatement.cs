using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetLatestDecisionTreesMaxOnlyQueryStatement : IServiceQuerySqlStatement<GetLatestDecisionTreesMaxOnlyQuery, GetLatestDecisionTreesMaxOnlyQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT dt.Id DecisionTreeId, dt.Name, dt.[Description], dt.IsActive, dt.Thumbnail, dtv.Id DecisionTreeVersionId, Max(dtv.[Version]) LatestVersion, IIF(pdt.Id Is Null, 0, 1) IsPublished
                    FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv
                    INNER JOIN
                        (
                        SELECT DecisionTreeId, MAX(Version) AS MaxVersion
                        FROM [DecisionTree].[dbo].[DecisionTreeVersion]
                        GROUP BY DecisionTreeId
                        ) groupeddtv ON dtv.DecisionTreeId = groupeddtv.DecisionTreeId AND dtv.Version = groupeddtv.MaxVersion
                    JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) on groupeddtv.DecisionTreeId = dt.Id
                    LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt (NOLOCK) on pdt.DecisionTreeVersionId = dtv.Id
                    GROUP BY dt.Id, dt.Name, dt.[Description], dt.IsActive, dt.Thumbnail, dtv.Id, pdt.Id
                    ORDER BY dt.Name ASC";
        }
    }
}