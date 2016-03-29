using SAHL.Core.Attributes;
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
    [NolockConventionExclude]
    public class GetDecisionTreeByNameAndVersionNumberQueryStatement : IServiceQuerySqlStatement<GetDecisionTreeByNameAndVersionNumberQuery, GetDecisionTreeByNameAndVersionNumberQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT dtv.DecisionTreeId, dtv.Id as DecisionTreeVersionId, dtv.Data, dtv.Version, dt.Name
                    FROM [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) 
                    JOIN [DecisionTree].[dbo].[DecisionTreeVersion] dtv ON dt.Id = dtv.DecisionTreeId
                    WHERE dt.Name = @DecisionTreeName 
                    AND dtv.Version = @VersionNumber";
        }
    }
}
