using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    public class DoesTreeVersionIdExistQuery : ISqlStatement<DecisionTreeVersionDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesTreeVersionIdExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return "SELECT Id, DecisionTreeId, Version, Data FROM [DecisionTree].[dbo].[DecisionTreeVersion] WHERE Id = @Id";
        }
    }
}