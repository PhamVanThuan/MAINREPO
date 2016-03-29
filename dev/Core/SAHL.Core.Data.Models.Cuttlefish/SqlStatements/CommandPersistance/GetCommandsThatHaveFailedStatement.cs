namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance
{
    public class GetCommandsThatHaveFailedStatement : ISqlStatement<CommandKeyModel>
    {

        public string ServiceName { get; protected set; }
        public  string MachineName { get; protected set; }

        public GetCommandsThatHaveFailedStatement(string machineName, string serviceName)
        {
            this.MachineName = machineName;
            this.ServiceName = serviceName;
        }

        public string GetStatement()
        {
            return @"SELECT CommandKey 
                   FROM [cuttlefish].[command].[Command]
                   WHERE MachineName = @MachineName
                   AND ServiceName = @ServiceName
                   AND HasCompleted = 0
                   AND HasFailed = 1";
        }
    }
}