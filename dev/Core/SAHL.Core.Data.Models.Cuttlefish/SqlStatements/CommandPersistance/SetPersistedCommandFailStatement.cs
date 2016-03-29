namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class SetPersistedCommandFailStatement : ISqlStatement<CommandDataModel>
    {

        public int CommandKey { get; protected set; }

        public SetPersistedCommandFailStatement(int commandKey)
        {
            this.CommandKey = commandKey;
        }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [CuttleFish].[command].[Command] 
                                SET HasFailed = 1
                                WHERE CommandKey = @CommandKey");
        }

    }
}