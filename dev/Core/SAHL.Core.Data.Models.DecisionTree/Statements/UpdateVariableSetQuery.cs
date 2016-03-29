using System;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    public class UpdateVariableSetQuery : ISqlStatement<VariableSetDataModel>
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public string Data { get; protected set; }

        public UpdateVariableSetQuery(Guid id, int version, string data)
        {
            this.Id = id;
            this.Version = version;
            this.Data = data;
        }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[VariableSet] SET data = @Data WHERE Id = @Id AND Version = @Version";
        }
    }
}