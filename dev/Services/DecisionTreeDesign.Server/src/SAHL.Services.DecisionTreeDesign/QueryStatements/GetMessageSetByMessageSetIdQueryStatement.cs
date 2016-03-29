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
    public class GetMessageSetByMessageSetIdQueryStatement : IServiceQuerySqlStatement<GetMessageSetByMessageSetIdQuery, GetMessageSetByMessageSetIdQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT ms.Id MessageSetId, ms.[Version], ms.Data
FROM [DecisionTree].[dbo].[MessageSet] ms
WHERE ms.Id = @MessageSetId";
        }
    }
}
