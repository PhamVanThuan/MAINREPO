using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetDecisionTreeQueryStatement : IServiceQuerySqlStatement<GetDecisionTreeQuery, GetDecisionTreeQueryResult>
    {
        public string GetStatement()
        {
            return @"with all_versions as (SELECT dtv.[Version]
                    FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
                    WHERE dtv.DecisionTreeId = (
                     SELECT d.DecisionTreeId
                     FROM [DecisionTree].[dbo].[DecisionTreeVersion] d
                     WHERE d.Id = @DecisionTreeVersionId
                    )
                    GROUP BY dtv.[Version])

                    SELECT dt.Id as DecisionTreeId, d.Id as DecisionTreeVersionId, d.Data, d.[Version], dt.Name, dt.[Description], IIF(pdt.DecisionTreeVersionId Is Null, 0, 1) IsPublished, Max(all_versions.Version) MaxVersion FROM all_versions
                    JOIN [DecisionTree].[dbo].[DecisionTreeVersion] d (NOLOCK)
                    ON d.Id=@DecisionTreeVersionId
                    JOIN [DecisionTree].[dbo].[DecisionTree] dt ON dt.Id = d.DecisionTreeId
                    LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt (NOLOCK) on pdt.DecisionTreeVersionId = d.Id
                    GROUP BY d.Id, pdt.DecisionTreeVersionId, dt.Id, dt.Name, dt.Description,d.Data,d.Version";
        }
    }
}