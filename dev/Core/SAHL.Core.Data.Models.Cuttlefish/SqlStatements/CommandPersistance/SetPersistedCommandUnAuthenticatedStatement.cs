namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class SetPersistedCommandUnAuthenticatedStatement : ISqlStatement<CommandDataModel>
    {
        public int CommandKey { get; protected set; }

        public SetPersistedCommandUnAuthenticatedStatement(int commandKey)
        {
            CommandKey = commandKey;
        }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [CuttleFish].[command].[Command] 
                                SET NotAuthenticated = 1
                                WHERE CommandKey = @CommandKey"); 
        }
    }
}