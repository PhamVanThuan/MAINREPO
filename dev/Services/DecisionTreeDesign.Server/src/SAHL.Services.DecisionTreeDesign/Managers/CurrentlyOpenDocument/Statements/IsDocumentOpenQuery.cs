using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Statements
{
    [NolockConventionExclude]
    public class IsDocumentOpenQuery : ISqlStatement<CurrentlyOpenDocumentDataModel>
    {
        public Guid DocumentVersionId { get; protected set; }

        public IsDocumentOpenQuery(Guid documentVersionId)
        {
            this.DocumentVersionId = documentVersionId;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[DocumentVersionId],[Username],[OpenDate],[DocumentTypeId]
                    FROM [DecisionTree].[dbo].[CurrentlyOpenDocument] (NOLOCK) WHERE [DocumentVersionId] = @DocumentVersionId";
        }
    }
}