using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    public class GetDecisionTreeHistoryInfoQueryStatement : IServiceQuerySqlStatement<GetDecisionTreeHistoryInfoQuery, GetDecisionTreeHistoryInfoQueryResult>
    {
        public string GetStatement()
        {
            return string.Format(@"SELECT TOP 25 dth.[Id], dth.[DecisionTreeVersionId], dth.[ModificationUser], dth.[ModificationDate], dtv.Version AS DecisionTreeVersion,
                                CASE WHEN pdt.Id IS NOT NULL THEN 1 ELSE 0 END AS IsPublished
                                        FROM [DecisionTree].[dbo].[DecisionTreeHistory] dth
                                        JOIN [DecisionTree].[dbo].[DecisionTreeVersion] dtv on dtv.Id = dth.DecisionTreeVersionId
                                        LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt on pdt.DecisionTreeVersionId = dtv.Id
                                WHERE dtv.[DecisionTreeId] = @DecisionTreeId
                                ORDER BY dth.[ModificationDate] DESC ");
        }
    }
}