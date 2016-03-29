using System;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    public class UpdateEnumerationSetQuery : ISqlStatement<EnumerationSetDataModel>
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public string Data { get; protected set; }

        public UpdateEnumerationSetQuery(Guid id, int version, string data)
        {
            this.Id = id;
            this.Version = version;
            this.Data = data;
        }

        public string GetStatement()
        {
            return "UPDATE [DecisionTree].[dbo].[EnumerationSet] SET data = @Data WHERE Id = @Id AND Version = @Version";
        }
    }
}