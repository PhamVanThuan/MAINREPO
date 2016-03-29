namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class SetPersistedCommandCompleteStatement : ISqlStatement<CommandDataModel>
    {
        public int CommandKey { get; protected set; }

        public SetPersistedCommandCompleteStatement(int commandKey)
        {
            this.CommandKey = commandKey;
        }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [CuttleFish].[command].[Command]
                                SET HasCompleted = 1,
                                HasFailed = 0
                                WHERE CommandKey = @CommandKey");
        }
    }
}