using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements
{
    [NolockConventionExclude]
    public class GetAllOpenDocumentsQuery : ISqlStatement<OpenDocumentsView>
    {
        public string GetStatement()
        {
            return @"SELECT cod.[Id],cod.[DocumentVersionId],cod.[Username],cod.[OpenDate],cod.[DocumentTypeId],REPLACE(dte.[Name],' ','') as [DocumentTypeName]
            FROM [DecisionTree].[dbo].CurrentlyOpenDocument cod (NOLOCK)
            JOIN [DecisionTree].[dbo].DocumentTypeEnum dte (NOLOCK) ON cod.DocumentTypeId = dte.id";
        }
    }
}