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
    public class GetVariableSetByVariableSetIdQueryStatement : IServiceQuerySqlStatement<GetVariableSetByVariableSetIdQuery, GetVariableSetByVariableSetIdQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT vs.Id VariableSetId, vs.[Version], vs.Data
            FROM [DecisionTree].[dbo].[VariableSet] vs
            WHERE vs.Id = @VariableSetId";
        }
    }
}
