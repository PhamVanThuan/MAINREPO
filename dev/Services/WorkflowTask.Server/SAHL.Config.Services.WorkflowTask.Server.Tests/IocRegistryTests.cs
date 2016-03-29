using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Logging;
using SAHL.Services.WorkflowTask.Server;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using StructureMap;

namespace SAHL.Config.Services.WorkflowTask.Server.Tests
{
    [TestFixture]
    public class IocRegistryTests
    {
        [Test]
        public void ITaskStatementSelector_Constructor()
        {
            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();

            var instance = ObjectFactory.Container.GetInstance<ITaskStatementSelector>();

            Assert.IsNotNull(instance);
        }

        [Test]
        public void ITaskQueryResultCoordinator_Constructor()
        {
            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();

            ObjectFactory.Configure(a =>
            {
                a.For<ISqlRepositoryFactory>().Use<DapperSqlRepositoryFactory>();

                a.For<IDbConnectionProviderStorage>().Singleton().Use<DefaultDbConnectionProviderStorage>();

                a.For<IDbConnectionProviderFactory>().Use<DefaultDbConnectionProviderFactory>();

                a.For<IDbConfigurationProvider>().Use<DefaultDbConfigurationProvider>();

                a.For<IUIStatementProvider>().Singleton().Use<AssemblyUIStatementProvider>();

                a.For<IRawLogger>().Use<NullRawLogger>();

                a.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

                a.For<ILoggerSource>().Use(new LoggerSource("SAHL.Services.WorkflowTask.Server.Tests", LogLevel.Error, true));
            });

            var instance = ObjectFactory.Container.GetInstance<ITaskQueryCoordinator>();

            Assert.IsNotNull(instance);
        }
    }
}
