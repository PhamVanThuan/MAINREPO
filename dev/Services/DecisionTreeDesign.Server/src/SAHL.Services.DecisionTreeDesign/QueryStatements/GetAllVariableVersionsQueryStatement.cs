using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    public class GetAllVariableVersionsQueryStatement : IServiceQuerySqlStatement<GetAllVariableVersionsQuery, GetAllVariableVersionsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT vs.[Id],vs.[Version],CASE WHEN PublishDate IS NULL THEN 0 ELSE 1 END as IsPublished,pvs.PublishDate,pvs.Publisher
FROM [DecisionTree].[dbo].[VariableSet] vs
LEFT OUTER JOIN [DecisionTree].[dbo].[PublishedVariableSet] pvs ON vs.Id = pvs.VariableSetId
ORDER BY vs.[Version] DESC";
        }
    }
}
