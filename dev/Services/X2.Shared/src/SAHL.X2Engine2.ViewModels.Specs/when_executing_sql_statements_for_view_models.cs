using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Logging;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.ViewModels.Specs
{
    [TestFixture]
    public class when_executing_sql_statements_for_view_models
    {
        [TestFixtureSetUp]
        public void OnTestFixtureSetUp()
        {
            SpecificationIoCBootstrapper.Initialize();
            DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
            var statementProvider = ObjectFactory.GetInstance<AssemblyUIStatementProvider>();
            var testLoggerAppSource = new LoggerAppSourceFromImplicitSource("TestLoggerAppSource");
            DbContextConfiguration.Instance.RepositoryFactory = new DapperSqlRepositoryFactory(statementProvider,
                new Logger(new NullRawLogger(), new NullMetricsRawLogger(), testLoggerAppSource, new MetricTimerFactory()),
                new LoggerSource("StatementTests", LogLevel.Error, false));
        }

        [Ignore("Green Scrum Team To Fix - NazirJ must fix these tests")]
        [Test, TestCaseSource(typeof(ViewModelTestCaseSource), "GetSqlStatements")]
        public void It_should_execute_without_error(Type sqlStatement)
        {
            try
            {
                var genericType = sqlStatement.GetInterfaces().First().GetGenericArguments().First();
                var constructor = sqlStatement.GetConstructors().First();
                var constructorParams = sqlStatement.GetConstructors().First().GetParameters();
                List<object> paramList = new List<object>();
                Dictionary<string, object> parameterDictionary = new Dictionary<string, object>();
                foreach (var param in constructorParams)
                {
                    object arg = null;
                    var type = param.ParameterType;
                    var name = param.Name;
                    if (type.IsValueType)
                        arg = Activator.CreateInstance(type);
                    object argument = GetArgumentHandlingNullableValues(arg, type);
                    paramList.Add(argument);
                    parameterDictionary.Add(name, argument);
                }
                object[] evaluatedParams = paramList.ToArray();
                object o = constructor.Invoke(evaluatedParams);
                string statement = o.GetType().GetMethods().Where(x => x.Name == "GetStatement").First().Invoke(o, null).ToString();
                foreach (var item in parameterDictionary)
                {
                    statement = statement.ToLower().Replace(string.Format("@{0}", item.Key).ToLower(), item.Value.ToString());
                }
                using (var db = new Db().InWorkflowContext())
                {
                    var openGenericMethod = db.GetType().GetMethods().Where(x => x.Name == "SelectOne").First();
                    var genericMethod = openGenericMethod.MakeGenericMethod(genericType);
                    dynamic myResult = genericMethod.Invoke(db, new object[] { statement, null });
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ? ex.Message : ex.InnerException.ToString();
                Assert.Fail(message);
            }
        }

        private object GetArgumentHandlingNullableValues(object arg, Type type)
        {
            Type nullableType = Nullable.GetUnderlyingType(type);
            if (arg == null && nullableType != null)
            {
                return Activator.CreateInstance(nullableType);
            }
            if (arg == null && type == typeof(System.String))
                return "'String Val'";
            return arg;
        }
    }
}