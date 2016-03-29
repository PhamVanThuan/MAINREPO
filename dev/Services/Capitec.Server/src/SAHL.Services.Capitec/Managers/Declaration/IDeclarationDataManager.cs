using System;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.Declaration
{
    public interface IDeclarationDataManager
    {
        Guid GetDeclarationDefinition(Guid declarationTypeEnumId, string declarationText);

        void AddDeclaration(Guid declarationID, Guid declarationDefinitionID, DateTime declarationDate);
    }
}