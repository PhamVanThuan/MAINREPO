using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetDecisionTreeByNameQueryStatement : IServiceQuerySqlStatement<GetDecisionTreeByNameQuery, GetDecisionTreeByNameQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT dt.Id, dt.Name, dt.[Description], dt.IsActive FROM [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) WHERE dt.Name = @DecisionTreeName";
        }
    }
}