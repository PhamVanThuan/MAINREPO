using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet.Statements
{
    public class DoesMessageSetExistsQuery : ISqlStatement<MessageSetDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesMessageSetExistsQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return @"SELECT Id, [Version], Data
FROM [DecisionTree].[dbo].[MessageSet]
WHERE Id = @Id";
        }
    }
}