namespace SAHL.Core.Data.Models.EventStore.Statements.CommandPersistance
{
    public class GetCommandHostContextForCommandKeyStatement : ISqlStatement<CommandHostContextModel>
    {
        public int Key { get; private set; }

        public GetCommandHostContextForCommandKeyStatement(int key)
        {
            Key = key;
        }

        public string GetStatement()
        {
            return @"   SELECT RunAsUser
                        FROM [EventStore].[command].[Command]
                        WHERE CommandKey = @Key";
        }
    }
}