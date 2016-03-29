using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DoesTreeIdExistQuery : ISqlStatement<DecisionTreeDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesTreeIdExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return "SELECT Id, Name, Description, IsActive, Thumbnail FROM [DecisionTree].[dbo].[DecisionTree] WHERE Id = @Id";
        }
    }
}