using NUnit.Framework;
using SAHL.Core.IoC;
using SAHL.Core.Services;
using SAHL.Core.Extensions;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Conventions;
using SAHL.Core.Testing.Providers;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.WorkflowAssignmentDomain.Tests
{
    [TestFixture]
    public sealed class ConventionTests
    {
        [Test, TestCaseSource(typeof(TestParamsProvider<ServiceCommandHandlerConvention>), "GetTestParams")]
        public void CheckForDuplicateCommandHandlers(TestParams test)
        {
            var commandHandlersAndCommands = new Dictionary<string, string>();
            var provider = TypeProviderFactory.Instance<ServiceCommandHandlerConvention>();
            foreach (var commandHandlerType in provider.GetTypes())
            {
                var genericTypes = commandHandlerType.GetInterfaces().FirstOrDefault().GetGenericArguments().Select(x => x.Name).ToArray<string>();
                var concatenatedGenericTypes = String.Join("", genericTypes);
                commandHandlersAndCommands.Add(commandHandlerType.Name, concatenatedGenericTypes);
            }
            ConventionTestRunner.Run("CheckForDuplicateCommandHandlers", test.TypeUnderTest.Name, () =>
            {
                var sqlStatementInterfaceGenericTypes = commandHandlersAndCommands[test.TypeUnderTest.Name];
                if (commandHandlersAndCommands.Values.Where(x => x == sqlStatementInterfaceGenericTypes).Count() > 1)
                    throw new System.Exception("More than one command handler was found having the same generic arguments");
            });
        }

        [Test, TestCaseSource(typeof(TestParamsProvider<ServiceCommandConvention>), "GetTestParams")]
        public void CheckForMissingCommandHandlers(TestParams test)
        {
            ConventionTestRunner.Run("CheckForMissingCommandHandlers", test.TypeUnderTest.Name, () =>
            {
                var provider = TypeProviderFactory.Instance<ServiceCommandHandlerConvention>();
                var commandHandlers = provider.GetTypes();
                var exists = commandHandlers.Where(x =>
                        x.GetInterface(typeof(IServiceCommandHandler<>).Name).GetGenericArguments().FirstOrDefault().Name == test.TypeUnderTest.Name
                    ).Count() == 1;
                Assert.True(exists, string.Format("Command {0} does not have a command handler", test.TypeUnderTest.Name));
            });
        }

        [Test, TestCaseSource(typeof(TestParamsProvider<ServiceQuerySqlStatementConvention>), "GetTestParams")]
        public void CheckForDuplicateServiceQuerySqlStatements(TestParams test)
        {
            var sqlStatementQueriesAndResults = new Dictionary<string, string>();
            foreach (var query in ServiceQuerySqlStatementsProvider.GetServiceQuerySqlStatements(test.TypeUnderTest.Assembly))
            {
                var genericTypes = query.GetInterfaces().FirstOrDefault().GetGenericArguments().Select(x => x.Name).ToArray<string>();
                var concatenatedGenericTypes = String.Join("", genericTypes);
                sqlStatementQueriesAndResults.Add(query.Name, concatenatedGenericTypes);
            }

            ConventionTestRunner.Run("CheckForDuplicateServiceQuerySqlStatements", test.TypeUnderTest.Name, () =>
            {
                var sqlStatementInterfaceGenericTypes = sqlStatementQueriesAndResults[test.TypeUnderTest.Name];
                if (sqlStatementQueriesAndResults.Values.Where(x => x == sqlStatementInterfaceGenericTypes).Count() > 1)
                    throw new System.Exception("More than one sql statement was found that has the same generic arguments");
            });
        }

        [Test, TestCaseSource(typeof(TestParamsProvider<ServiceQuerySqlStatementConvention>), "GetTestParams")]
        public void ExecuteServiceQuerySqlStatement(TestParams test)
        {
            ConventionTestRunner.Run("ExecuteServiceQuerySqlStatements", test.TypeUnderTest.Name, () =>
            {
                var openGenericSQLStatement = test.TypeUnderTest.GetInterfaces().Where(x => x.IsAssignableToGenericType(typeof(IServiceQuerySqlStatement<,>))).FirstOrDefault();
                Type queryType = openGenericSQLStatement.GetGenericArguments()[0];

                var query = ObjectBuilder.CreateObjectWithDefaultProperties(queryType);

                dynamic queryStatement = ObjectFactory.GetInstance(test.TypeUnderTest);

                SqlStatementRunner.Run(queryStatement, openGenericSQLStatement.GetGenericArguments()[1], query);
            });
        }

        [Test, TestCaseSource(typeof(TestParamsProvider<SqlStatementConvention>), "GetTestParams")]
        public void ExecuteSqlStatements(TestParams test)
        {
            ConventionTestRunner.Run("RunSqlStatements", test.TypeUnderTest.Name, () =>
            {
                var statementInstance = ObjectBuilder.CreateObjectWithDefaultProperties(test.TypeUnderTest);
                var dataModel = test.TypeUnderTest.GetInterfaces().FirstOrDefault().GetGenericArguments().FirstOrDefault();
                var results = SqlStatementRunner.Run(statementInstance, dataModel);
            });
        }
    }
}
