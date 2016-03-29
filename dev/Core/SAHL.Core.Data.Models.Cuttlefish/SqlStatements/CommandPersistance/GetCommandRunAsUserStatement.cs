namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class GetCommandHostContextDetailsForCommandKeyStatement : ISqlStatement<CommandHostContextModel>
    {
         
        public int CommandKey { get; protected set; }

        public GetCommandHostContextDetailsForCommandKeyStatement(int commandKey)
        {
            this.CommandKey = commandKey;
        }

        
        public string GetStatement()
        {
            return string.Format(@"SELECT ContextValues 
                                    FROM [CuttleFish].[command].[Command] 
                                  WHERE CommandKey = @CommandKey");
        }

    }
}
