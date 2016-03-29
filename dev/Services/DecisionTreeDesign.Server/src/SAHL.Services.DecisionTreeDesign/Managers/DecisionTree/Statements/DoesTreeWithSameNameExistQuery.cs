using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DoesTreeWithSameNameExistQuery : ISqlStatement<DecisionTreeDataModel>
    {
        public string Name { get; protected set; }

        public DoesTreeWithSameNameExistQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return "SELECT Id, Name, Description, IsActive, Thumbnail FROM [DecisionTree].[dbo].[DecisionTree] WHERE Name = @Name";
        }
    }
}