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
    public class GetAllMessageVersionsQueryStatement : IServiceQuerySqlStatement<GetAllMessageVersionsQuery, GetAllMessageVersionsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT ms.[Id], ms.[Version],CASE WHEN PublishDate IS NULL THEN 0 ELSE 1 END as IsPublished, pms.PublishDate, pms.Publisher
FROM [DecisionTree].[dbo].[MessageSet] ms
LEFT OUTER JOIN [DecisionTree].[dbo].[PublishedMessageSet] pms ON ms.Id = pms.MessageSetId
ORDER BY ms.[Version] DESC";
        }
    }
}
