namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class GetCommandsThatArePendingStatement : ISqlStatement<CommandKeyModel>
    {
        public string ServiceName { get; protected set; }
        public  string MachineName { get; protected set; }

        public GetCommandsThatArePendingStatement(string machineName, string serviceName)
        {
            this.MachineName = machineName;
            this.ServiceName = serviceName;
        }

        public string GetStatement()
        {
            return @"SELECT CommandKey 
                   FROM [CuttleFish].[command].[Command]
                   WHERE MachineName = @MachineName
                   AND ServiceName = @ServiceName
                   AND HasCompleted = 0
                   AND HasFailed = 0";
        }
    }
}