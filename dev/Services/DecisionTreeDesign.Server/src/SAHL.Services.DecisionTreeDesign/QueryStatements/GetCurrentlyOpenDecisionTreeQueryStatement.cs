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
    public class GetCurrentlyOpenDecisionTreeQueryStatement : IServiceQuerySqlStatement<GetCurrentlyOpenDecisionTreeQuery, GetCurrentlyOpenDecisionTreeQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[DocumentVersionId],[Username],[OpenDate],[DocumentTypeId] FROM [DecisionTree].[dbo].[CurrentlyOpenDocument]
                    WHERE [DocumentVersionId] = @TreeVersionId";
        }
    }
}
