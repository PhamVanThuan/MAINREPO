using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetNonTreeDocumentLockStatusQueryStatement : IServiceQuerySqlStatement<GetNonTreeDocumentLockStatusQuery, GetNonTreeDocumentLockStatusQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT cod.Username,REPLACE(dte.Name,' ','') as DocumentType
                FROM [DecisionTree].dbo.CurrentlyOpenDocument cod (NOLOCK)
                JOIN [DecisionTree].dbo.DocumentTypeEnum dte (NOLOCK) ON cod.DocumentTypeId = dte.id
                WHERE dte.Name <> 'Tree'";
        }
    }
}