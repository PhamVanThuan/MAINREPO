namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class SetPersistedCommandCompleteAndAuthorisedStatement : ISqlStatement<CommandDataModel>
    {
        public int CommandKey { get; private set; }

        public SetPersistedCommandCompleteAndAuthorisedStatement(int commandKey)
        {
            CommandKey = commandKey;
        }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [CuttleFish].[command].[Command]
                                SET  HasCompleted = 1,
                                HasFailed = 0,
                                NotAuthorized = 0,
                                NotAuthenticated = 0
                                WHERE CommandKey = @CommandKey");
        }
    }
}