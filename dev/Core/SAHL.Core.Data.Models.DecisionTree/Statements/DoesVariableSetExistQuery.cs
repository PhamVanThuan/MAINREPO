using System;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    public class DoesVariableSetExistQuery : ISqlStatement<VariableSetDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesVariableSetExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return @"SELECT Id, [Version], Data
FROM [DecisionTree].[dbo].[VariableSet]
WHERE Id = @Id";
        }
    }
}