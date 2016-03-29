using System;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    public class DoesEnumerationSetExistQuery : ISqlStatement<EnumerationSetDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesEnumerationSetExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return @"SELECT Id, [Version], Data
FROM [DecisionTree].[dbo].[EnumerationSet]
WHERE Id = @Id";
        }
    }
}