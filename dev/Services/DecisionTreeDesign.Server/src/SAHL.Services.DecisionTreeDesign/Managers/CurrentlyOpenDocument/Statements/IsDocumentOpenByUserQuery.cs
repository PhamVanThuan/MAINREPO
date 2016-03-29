using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements
{
    [NolockConventionExclude]
    public class IsDocumentOpenByUserQuery : ISqlStatement<CurrentlyOpenDocumentDataModel>
    {
        public Guid DocumentVersionId { get; protected set; }

        public string Username { get; protected set; }

        public IsDocumentOpenByUserQuery(Guid documentVersionId, string username)
        {
            this.DocumentVersionId = documentVersionId;
            this.Username = username;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[DocumentVersionId],[Username],[OpenDate],[DocumentTypeId]
                    FROM [DecisionTree].[dbo].[CurrentlyOpenDocument] (NOLOCK) WHERE [DocumentVersionId] = @DocumentVersionId AND [Username] = @Username";
        }
    }
}