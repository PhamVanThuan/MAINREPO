using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class UpdateThumbnailDataQuery : ISqlStatement<DecisionTreeDataModel>
    {
        public UpdateThumbnailDataQuery(Guid decisionTreeId, string Thumbnail)
        {
            this.DecisionTreeId = decisionTreeId;
            this.Thumbnail = Thumbnail;
        }

        public Guid DecisionTreeId { get; protected set; }

        public string Thumbnail { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[DecisionTree] SET Thumbnail = @Thumbnail WHERE Id = @DecisionTreeId";
        }
    }
}