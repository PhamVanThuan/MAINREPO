using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DoesTreeWithSameNameAndIdExistQuery : ISqlStatement<DecisionTreeDataModel>
    {
        public string Name { get; protected set; }

        public Guid Id { get; protected set; }

        public DoesTreeWithSameNameAndIdExistQuery(Guid Id, string name)
        {
            this.Name = name;
            this.Id = Id;
        }

        public string GetStatement()
        {
            return "SELECT Id, Name, Description, IsActive, Thumbnail FROM [DecisionTree].[dbo].[DecisionTree] WHERE Name = @Name AND Id = @Id";
        }
    }
}