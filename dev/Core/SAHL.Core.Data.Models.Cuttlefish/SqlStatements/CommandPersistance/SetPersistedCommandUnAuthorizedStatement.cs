namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class SetPersistedCommandUnAuthorizedStatement : ISqlStatement<CommandDataModel>
    {
        public int CommandKey { get; protected set; }

        public SetPersistedCommandUnAuthorizedStatement(int commandKey)
        {
            this.CommandKey = commandKey;
        }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [CuttleFish].[command].[Command]
                                SET  NotAuthorized = 1
                                WHERE CommandKey = @CommandKey");
        }
    }
}