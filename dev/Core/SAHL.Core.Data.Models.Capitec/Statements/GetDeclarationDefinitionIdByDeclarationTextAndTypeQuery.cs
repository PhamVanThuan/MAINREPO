using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery : ISqlStatement<DeclarationDefinitionDataModel>
    {
        public Guid DeclarationTypeEnumId { get; protected set; }

        public string DeclarationText { get; protected set; }

        public GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery(Guid declarationTypeEnumId, string declarationText)
        {
            this.DeclarationTypeEnumId = declarationTypeEnumId;
            this.DeclarationText = declarationText;
        }

        public string GetStatement()
        {
            return @"SELECT [ID],
                            [DeclarationTypeEnumId],
                            [DeclarationText]
                      FROM Capitec.dbo.DeclarationDefinition
                     WHERE [DeclarationTypeEnumId] = @DeclarationTypeEnumId
                       AND [DeclarationText] = @DeclarationText";
        }
    }
}