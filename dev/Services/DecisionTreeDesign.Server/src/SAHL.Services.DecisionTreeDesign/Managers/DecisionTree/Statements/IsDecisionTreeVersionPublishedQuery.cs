using System;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class IsDecisionTreeVersionPublishedQuery : ISqlStatement<PublishedDecisionTreeDataModel>
    {
        public Guid Version { get; protected set; }

        public IsDecisionTreeVersionPublishedQuery(Guid version)
        {
            this.Version = version;
        }

        public string GetStatement()
        {
            return "SELECT Id, DecisionTreeVersionId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedDecisionTree] WHERE DecisionTreeVersionId = @Version";
        }
    }
}