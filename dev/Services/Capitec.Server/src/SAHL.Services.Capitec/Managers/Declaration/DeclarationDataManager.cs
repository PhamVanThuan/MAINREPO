using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Services.Interfaces.Capitec.Common;
using System;

namespace SAHL.Services.Capitec.Managers.Declaration
{
    public class DeclarationDataManager : IDeclarationDataManager
    {
        private IDbFactory dbFactory;
        public DeclarationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public Guid GetDeclarationDefinition(Guid declarationTypeEnumId, string declarationText)
        {
            var query = new GetDeclarationDefinitionIdByDeclarationTextAndTypeQuery(declarationTypeEnumId, declarationText);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var declarationDefinition = db.SelectOne(query);
                if (declarationDefinition != null)
                {
                    return declarationDefinition.ID;
                }
            }
            throw new NullReferenceException("declaration definition does not exist");
        }

        public void AddDeclaration(Guid declarationID, Guid declarationDefinitionID, DateTime declarationDate)
        {
            var declarationDataModel = new DeclarationDataModel(declarationID, declarationDefinitionID, declarationDate);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(declarationDataModel);
                db.Complete();
            }
        }
    }
}