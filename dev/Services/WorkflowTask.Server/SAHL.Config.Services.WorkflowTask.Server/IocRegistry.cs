using System.Collections.Concurrent;
using System.Reflection;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Logging;
using SAHL.Services.WorkflowTask.Server;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.WorkflowTask.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            var assemblyToScan = Assembly.Load("SAHL.Services.WorkflowTask");

            For<ITaskStatementSelector>()
                .Singleton()
                .Use<TaskStatementSelector>()
                .Ctor<Assembly>().Is(assemblyToScan);

            For<ITaskQueryCoordinator>()
                .Singleton()
                .Use<TaskQueryCoordinator>()
                .Ctor<ConcurrentDictionary<string, string>>().Is(new ConcurrentDictionary<string, string>())
                .Ctor<ConcurrentDictionary<TaskStatementTypeColumnHeaderPair, ColumnDefinition>>().Is(new ConcurrentDictionary<TaskStatementTypeColumnHeaderPair, ColumnDefinition>());
        }
    }
}