using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class UpdateDecisionTreeQuery : ISqlStatement<DecisionTreeDataModel>
    {
        public UpdateDecisionTreeQuery(Guid id, string name, string description, bool isActive, string thumbnail)
        {
            this.DecisionTreeId = id;
            this.Name = name;
            this.Description = description;
            this.IsActive = IsActive;
            this.Thumbnail = thumbnail;
        }

        public string Description { get; protected set; }
        public Guid DecisionTreeId { get; protected set; }
        public bool IsActive { get; protected set; }
        public string Name { get; protected set; }
        public string Thumbnail { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[DecisionTree] SET Name = @Name, Description = @Description, IsActive = @IsActive, Thumbnail = @Thumbnail WHERE Id = @DecisionTreeId";
        }
    }
}