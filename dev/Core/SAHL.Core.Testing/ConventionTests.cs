using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Testing.Ioc.Registration;
using SAHL.Core.Testing.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace SAHL.Core.Testing.Conventions
{
    [TestFixture]
    public class ConventionTests : ConventionTestSuite
    {
        [Test]
        public void CheckForDuplicateCommandHandlers()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandHandlersTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("CheckForDuplicateCommandHandlers", test.TypeUnderTest.Name, test, () =>
                {
                    var commandHandlersAndCommands = new Dictionary<string, string>();
                    foreach (var commandHandlerType in test.TestingIocContainer.GetRegisteredTypes())
                    {
                        var genericTypes = commandHandlerType
                            .GetInterfaces()
                            .First(x => x.IsGenericType)
                            .GetGenericArguments()
                            .Select(x => x.Name)
                            .ToArray();

                        var concatenatedGenericTypes = String.Join("", genericTypes);
                        commandHandlersAndCommands.Add(commandHandlerType.Name, concatenatedGenericTypes);
                    }

                    var sqlStatementInterfaceGenericTypes = commandHandlersAndCommands[test.TypeUnderTest.Name];
                    if (commandHandlersAndCommands.Values.Count(x => x == sqlStatementInterfaceGenericTypes) > 1)
                    {
                        throw new Exception("More than one command handler was found having the same generic arguments");
                    }
                });
            }
        }

        [Test]
        public void CheckForMissingCommandHandlers()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("CheckForMissingCommandHandlers", test.TypeUnderTest.Name, test, () =>
                {
                    var commandHandlersTestParams = this.ServiceCommandHandlersTestParams();
                    var exists = commandHandlersTestParams.Count(x =>
                    {
                        var serviceCommandHandler = x.TypeUnderTest.GetInterface(typeof(IServiceCommandHandler<>).Name).GetGenericArguments().FirstOrDefault();
                        return (serviceCommandHandler != null) && (serviceCommandHandler.Name == test.TypeUnderTest.Name);
                    }) == 1;

                    Assert.True(exists, string.Format("Command {0} does not have a command handler", test.TypeUnderTest.Name));
                });
            }
        }

        [Test]
        public void CheckForCommandHandlerNamingConventions()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            foreach (var test in testParams)
            {
                string handlerDescription = string.Empty;
                this.Run("CheckForCommandHandlerNamingConventions", test.TypeUnderTest.Name, test, () =>
                {
                    var commandHandlersTestParams = this.ServiceCommandHandlersTestParams();
                    var exists = commandHandlersTestParams.Count(x =>
                    {
                        var serviceCommandHandlerInterface = x.TypeUnderTest.GetInterface(typeof(IServiceCommandHandler<>).Name);
                        var arguments = serviceCommandHandlerInterface.GetGenericArguments();
                        var serviceCommand = arguments.FirstOrDefault();
                        if (serviceCommand != null && (serviceCommand.Name == test.TypeUnderTest.Name))
                        {
                            handlerDescription = x.TypeUnderTest.Name;
                            return x.TypeUnderTest.Name.Contains(serviceCommand.Name);
                        }
                        return false;
                    }) == 1;

                    Assert.True(exists, string.Format("Command/CommandHandler naming convention not followed. Command: {0}, Handler:{1}", test.TypeUnderTest.Name, handlerDescription));
                });
            }
        }

        [Test]
        public void CheckForDuplicateServiceQuerySqlStatements()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceQuerySqlStatementsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            foreach (var test in testParams)
            {
                this.Run("CheckForDuplicateServiceQuerySqlStatements", test.TypeUnderTest.Name, test, () =>
                {
                    var sqlStatementQueriesAndResults = new Dictionary<string, string>();
                    foreach (var query in test.TestingIocContainer.GetRegisteredTypes())
                    {
                        var genericTypes = query
                            .GetInterfaces()
                            .First()
                            .GetGenericArguments()
                            .Select(x => x.Name)
                            .ToArray();

                        var concatenatedGenericTypes = String.Join("", genericTypes);
                        sqlStatementQueriesAndResults.Add(query.Name, concatenatedGenericTypes);
                    }

                    var sqlStatementInterfaceGenericTypes = sqlStatementQueriesAndResults[test.TypeUnderTest.Name];
                    if (sqlStatementQueriesAndResults.Values.Count(x => x == sqlStatementInterfaceGenericTypes) > 1)
                    {
                        throw new Exception("More than one sql statement was found that has the same generic arguments");
                    }
                });
            }
        }

        [Test]
        public void ExecuteServiceQuerySqlStatement()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceQuerySqlStatementsTestParams());
            testParams.AddRange(this.ServiceQuerySqlStatementsDataModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            foreach (var test in testParams)
            {
                this.Run("ExecuteServiceQuerySqlStatement", test.TestName, test, () =>
                {
                    var serviceStatement = test.TestingIocContainer.GetInstance(test.TypeUnderTest);
                    var serviceQueryType = test.TypeUnderTest.GetInterfaces().First().GenericTypeArguments[0];
                    var serviceQueryTestParam = this.ServiceQueryTestParams().FirstOrDefault(x => x.TypeUnderTest == serviceQueryType);
                    bool hasInsertExcludeAttribute = serviceStatement.GetType().IsDefined(typeof(InsertConventionExclude), false);
                    if (hasInsertExcludeAttribute)
                    {
                        return;
                    }

                    var serviceQuery = serviceQueryTestParam.TestingIocContainer.GetInstance(serviceQueryTestParam.TypeUnderTest);
                    StatementRunner.RunServiceQueryStatement(serviceStatement, serviceQuery);
                });
            }
        }

        [Test]
        public void ExecuteSqlStatements()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.SqlStatementsTestParams());
            testParams.AddRange(this.SqlStatementsDataModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            foreach (var test in testParams)
            {
                this.Run("ExecuteSqlStatements", test.TestName, test, () =>
                {
                    var sqlStatement = test.TestingIocContainer.GetInstance(test.TypeUnderTest);
                    bool hasInsertExcludeAttribute = sqlStatement.GetType().IsDefined(typeof(InsertConventionExclude), false);
                    if (hasInsertExcludeAttribute)
                    {
                        return;
                    }
                    StatementRunner.RunSqlStatement(sqlStatement);
                });
            }
        }

        [Test]
        public void ValidateSqlForNoLocks()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.SqlStatementsTestParams());
            testParams.AddRange(this.SqlStatementsDataModelsTestParams());
            testParams.AddRange(this.ServiceQuerySqlStatementsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("ValidateSql", test.TestName, test, () =>
                {
                    dynamic statement = test.TestingIocContainer.GetInstance(test.TypeUnderTest);
                    bool hasNolockAttribute = statement.GetType().IsDefined(typeof(NolockConventionExclude), true);
                    if (hasNolockAttribute)
                    {
                        return;
                    }
                    var sql = statement.GetStatement().ToLower();
                    var info = String.Format("nolock was found in the sql statement, if this is valid then decorate the statement with {0}", typeof(NolockConventionExclude).FullName);
                    StringAssert.DoesNotContain("nolock", sql, info);
                });
            }
        }

        [Test]
        public void CheckSettersOnPropertiesProtected()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandsTestParams());
            testParams.AddRange(this.ServiceQuerySqlStatementsTestParams());
            testParams.AddRange(this.SqlStatementsTestParams());
            testParams.AddRange(this.SqlStatementsDataModelsTestParams());
            testParams.AddRange(this.ServiceQuerySqlStatementsDataModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("CheckSettersOnPropertiesProtected", test.TypeUnderTest.Name, test, () =>
                  {
                      var allProperties = test.TypeUnderTest.GetProperties().Where(x => x.SetMethod != null);
                      var allPropertyAccessors = new Dictionary<string, MethodInfo[]>();

                      var accessorsProp = allProperties.FirstOrDefault(x => !allPropertyAccessors.ContainsKey(x.Name));
                      while (accessorsProp != null)
                      {
                          //make sure we get all properties regardless of accessors
                          var nonPublicAccessors = accessorsProp.GetAccessors(nonPublic: true);
                          var publicAccessors = accessorsProp.GetAccessors(nonPublic: false);

                          var methodInfo = new List<MethodInfo>();
                          methodInfo.AddRange(nonPublicAccessors.ToArray());
                          methodInfo.AddRange(publicAccessors.ToArray());
                          methodInfo = methodInfo.Distinct().ToList();
                          allPropertyAccessors.Add(accessorsProp.Name, methodInfo.ToArray());

                          accessorsProp = allProperties.FirstOrDefault(x => !allPropertyAccessors.ContainsKey(x.Name));

                      }
                      var protectedSetterCount = allPropertyAccessors.Values.Count(y => y.Any(x => x.IsFamily || !x.IsPublic && x.Name.Contains("set_")));

                      Assert.AreEqual(protectedSetterCount, allProperties.Count(),
                                  "The setters of one or more properties for command {0} was not protected or private", test.TypeUnderTest.Name);
                  });
            }
        }

        [Test]
        public void CheckDomainRuleConventions()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.DomainRuleTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodBase.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    //first check the rule is correctly named
                    Assert.That(test.TypeUnderTest.Name.Replace("`1", string.Empty).EndsWith("Rule"),
                        string.Format("Domain Rule: {0} does not end with the Rule suffix.", test.TypeUnderTest.Name));

                    var assemblyName = test.TypeUnderTest.Assembly.GetName().Name;

                    //check if assName has numeric char
                    var castResult = 0;
                    var number = assemblyName.ToCharArray()
                                                .Where(x => Int32.TryParse(x.ToString(), out castResult))
                                                .FirstOrDefault();
                    if (castResult > 0)
                    {
                        assemblyName = assemblyName.Replace(number.ToString(), String.Format("_{0}", number.ToString()));
                    }

                    var typeNamespace = test.TypeUnderTest.Namespace;
                    var endsWithStr = String.Empty;
                    if (test.UsedConvention == typeof(DomainRuleConvention))
                    {
                        endsWithStr = "Rules";
                    }
                    var hasCorrectNamespace = typeNamespace.StartsWith(assemblyName) &&
                           typeNamespace.EndsWith(endsWithStr);
                    Assert.True(hasCorrectNamespace,
                        "expected type under test to follow a namespace convention, convention used: {0}",
                                    test.UsedConvention.Name);
                });
            }
        }

        [Test]
        public void CheckDomainDataManagerNamespace()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.DomainDataManagerTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodInfo.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var ns = test.TypeUnderTest.Namespace;
                    var assName = test.TypeUnderTest.Assembly.GetName().Name;
                    var hasCorrectNamespace = ns.StartsWith(assName) &&
                          (ns.EndsWith("Managers") || ns.EndsWith("Manager"));
                    Assert.True(hasCorrectNamespace,
                      "expected type under test to follow a namespace convention, convention used: {0}",
                                  test.UsedConvention.Name);
                });
            }
        }

        [Test]
        public void CheckServiceCommandHandlerNamespace()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandHandlersTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodInfo.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var ns = test.TypeUnderTest.Namespace;
                    var assName = test.TypeUnderTest.Assembly.GetName().Name;
                    var hasCorrectNamespace = ns.StartsWith(assName) &&
                          ns.EndsWith("CommandHandlers") || ns.EndsWith("CommandHandlers.Internal");
                    Assert.True(hasCorrectNamespace,
                      "expected type under test to follow a namespace convention, convention used: {0}",
                                  test.UsedConvention.Name);
                });
            }
        }

        [Test]
        public void CheckServiceCommandHandlerInternalNamespace()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.InternalServiceCommandHandlersTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodInfo.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var ns = test.TypeUnderTest.Namespace;
                    var assName = test.TypeUnderTest.Assembly.GetName().Name;
                    var hasCorrectNamespace = ns.StartsWith(assName) &&
                          !ns.EndsWith("CommandHandlers") && ns.EndsWith("CommandHandlers.Internal");
                    Assert.True(hasCorrectNamespace,
                      "expected type under test to follow a namespace convention, convention used: {0}",
                                  test.UsedConvention.Name);
                });
            }
        }

        [Test]
        public void CheckDomainServiceCommandHandlerNamespace()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.DomainServiceCommandHandlersTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodInfo.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var ns = test.TypeUnderTest.Namespace;
                    var assName = test.TypeUnderTest.Assembly.GetName().Name;
                    var hasCorrectNamespace = ns.StartsWith(assName) &&
                          ns.EndsWith("CommandHandlers") && !ns.EndsWith("CommandHandlers.Internal");
                    Assert.True(hasCorrectNamespace,
                      "expected type under test to follow a namespace convention, convention used: {0}",
                                  test.UsedConvention.Name);
                });
            }
        }

        [Test]
        public void CheckServiceManagersNamespaces()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceManagerTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodInfo.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    string classDescription = test.UsedConvention == typeof(ServiceDataManagerConvention) ? "DataManager" : "Manager";
                    var assName = test.TypeUnderTest.Assembly.GetName().Name;
                    var expectedDataManagerNamespace = string.Format("{0}.Managers.{1}", assName, test.TypeUnderTest.Name.Replace(classDescription, ""));
                    Assert.AreEqual(expectedDataManagerNamespace, test.TypeUnderTest.Namespace);
                });
            }
        }

        [Test]
        public void CheckSqlStatementConvention()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.SqlStatementsTestParams());
            testParams.AddRange(this.SqlStatementsDataModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodBase.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var ns = test.TypeUnderTest.Namespace;
                    if (ns.Contains("Capitec") || ns.Contains("DecisionTree"))
                    {
                        return;
                    }
                    var hasCorrectNames = test.TypeUnderTest.Name.EndsWith("Statement")
                        || (test.TypeUnderTest.IsAbstract && test.TypeUnderTest.ContainsGenericParameters && test.TypeUnderTest.Name.EndsWith("Statement`1"));
                    Assert.True(hasCorrectNames,
                        string.Format("All ISqlStatements should end with 'Statement'. {0} does not meet the convention.", test.TypeUnderTest.Name));
                });
            }
        }

        [Test]
        public void CheckForRuleExecutionHandlers()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandHandlersTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodBase.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var typeDefinition = base.AssertTypeDefinition(test.TypeUnderTest);
                    var methodDefinition = base.AssertMethodDefinition(typeDefinition, true);

                    var hasDomainRuleDependency = base.GetParameterDefinition(methodDefinition, "IDomainRuleManager") != null;
                    if (hasDomainRuleDependency)
                    {
                        base.AssertParameterDefinition(methodDefinition, "IDomainRuleManager");
                        base.AssertMethodBodyForAtLeastOneReference(methodDefinition, new ExpectedReference("SAHL.Core.Rules.IDomainRuleManager", "RegisterRule", "Void"),
                                                                                        new ExpectedReference("SAHL.Core.Rules.IDomainRuleManager", "RegisterPartialRule", "Void"));
                        methodDefinition = base.AssertMethodDefinition(typeDefinition, false, "HandleCommand");
                        base.AssertMethodBodyForAtLeastOneReference(methodDefinition, new[] { new ExpectedReference("SAHL.Core.Rules.IDomainRuleManager", "ExecuteRules", "Void") });
                    }
                });
            }
        }

        [Test]
        public void CheckCommandPropertiesMatchCtorArguments()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.ServiceCommandsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodBase.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var commandProperties = test.TypeUnderTest.GetProperties();
                    var commandCtorArgs = test.TypeUnderTest.GetConstructors().First().GetParameters();

                    foreach (var arg in commandCtorArgs)
                    {
                        bool argMatched = commandProperties.Any(x => string.Compare(arg.Name, x.Name, true) == 0);
                        Assert.IsTrue(argMatched, string.Format("Command [{0}] does not have matching property for constructor argument [{1}]", test.TypeUnderTest.Name, arg.Name));
                    }
                });
            }
        }

        [Test]
        public void CheckForMissingLegacyEventQueryHandlers()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.LegacyEventQueriesTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            foreach (var test in testParams)
            {
                this.Run("CheckForMissingLegacyEventQueryHandlers", test.TypeUnderTest.Name, test, () =>
                {
                    var queryHandlersTestParams = this.LegacyEventQueryHandlersTestParams();
                    var exists = queryHandlersTestParams.Count(x =>
                    {
                        var serviceQueryHandler = x.TypeUnderTest
                            .GetInterface(typeof(IServiceQueryHandler<>).Name)
                            .GetGenericArguments()
                            .FirstOrDefault();
                        return (serviceQueryHandler != null) && (serviceQueryHandler.Name == test.TypeUnderTest.Name);
                    }) == 1;

                    Assert.True(exists, string.Format("Legacy event query {0} does not have a query handler", test.TypeUnderTest.Name));
                });
            }
        }

        [Test]
        public void CheckForMissingLegacyEventQueries()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.LegacyEventTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("CheckForMissingLegacyEventQueries", test.TypeUnderTest.Name, test, () =>
                {
                    var legacyEventQueriesTestParams = this.LegacyEventQueriesTestParams();
                    var exists = legacyEventQueriesTestParams.Count(x =>
                    {
                        var serviceQueryBase = x.TypeUnderTest.BaseType;
                        Assert.NotNull(serviceQueryBase, "Expected LegalEventQuery: {0} to inherit from ServiceQuery<>", x.TypeUnderTest);
                        var legacyEventQuery = serviceQueryBase.GetGenericArguments().FirstOrDefault();
                        return (legacyEventQuery != null) && (legacyEventQuery.Name == test.TypeUnderTest.Name);
                    }) == 1;

                    Assert.True(exists, string.Format("Legacy event {0} does not have a query", test.TypeUnderTest.Name));
                });
            }
        }

        [Test]
        public void EnsureModelsAreSerializable()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.DomainProcessManagerModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("EnsureModelsAreSerializable", test.TypeUnderTest.Name, test, () =>
                {
                    var isSerializable = test.TypeUnderTest.GetCustomAttributes(typeof(SerializableAttribute), false);
                    Assert.That(isSerializable.Length > 0, string.Format("The {0} should be marked with the serializable attribute.", test.TypeUnderTest.Name));
                });
            }
        }

        [Test]
        public void EnsureModelsAreMarkedWithDataContractAttribute()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.DomainProcessManagerModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run("EnsureModelsAreMarkedWithDataContractAttribute", test.TypeUnderTest.Name, test, () =>
                {
                    var markedWithDataContractAttribute = test.TypeUnderTest.GetCustomAttributes(typeof(DataContractAttribute), false);
                    Assert.That(markedWithDataContractAttribute.Length > 0, string.Format("The {0} should be marked with the serializable attribute.", test.TypeUnderTest.Name));
                });
            }
        }

        [Test]
        public void EnsureAllPropertiesOnModelsAreMarkedWithDataContractAttribute()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.DomainProcessManagerModelsTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            foreach (var test in testParams)
            {
                this.Run("EnsureAllPropertiesOnModelsAreMarkedWithDataContractAttribute", test.TypeUnderTest.Name, test, () =>
                {
                    var properties = test.TypeUnderTest.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                    foreach (var p in properties)
                    {
                        var setter = p.SetMethod;
                        if (setter == null)
                        {
                            continue;
                        }
                        var markedWithDataMemberAttribute = p.CustomAttributes
                            .FirstOrDefault(x => x.AttributeType == typeof(DataMemberAttribute));
                        var message = string.Format(@"Type: {0} has a property {1} that is not marked as a data member.", test.TypeUnderTest.Name, p.Name);
                        Assert.That(markedWithDataMemberAttribute != null, message);
                    }
                });
            }
        }
        [Test]
        public void CheckEventPropertiesMatchCtorArguments()
        {
            //---------------Set up test pack-------------------
            var testParams = new List<ITestParams>();
            testParams.AddRange(this.EventTestParams());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var test in testParams)
            {
                this.Run(MethodBase.GetCurrentMethod().Name, test.TypeUnderTest.Name, test, () =>
                {
                    var commandProperties = test.TypeUnderTest.GetProperties();
                    var commandCtorArgs = test.TypeUnderTest.GetConstructors().First().GetParameters();

                    foreach (var arg in commandCtorArgs)
                    {
                        bool argMatched = commandProperties.Any(x => string.Compare(arg.Name, x.Name, true) == 0);
                        Assert.IsTrue(argMatched, string.Format("Event [{0}] does not have matching property for constructor argument [{1}]", test.TypeUnderTest.Name, arg.Name));
                    }
                });
            }
        }
        public IEnumerable<ITestParams> DomainProcessManagerModelsTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, DomainProcessManagerModelConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> LegacyEventQueryHandlersTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, LegacyEventQueryHandlerConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> LegacyEventTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, LegacyEventConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> LegacyEventQueriesTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, LegacyEventQueryConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> ServiceCommandHandlersTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, ServiceCommandHandlerConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> InternalServiceCommandHandlersTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, InternalServiceCommandHandlerForDomainConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> DomainServiceCommandHandlersTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, DomainServiceCommandHandlerConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> ServiceManagerTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, ServiceManagerConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> ServiceCommandsTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, ServiceCommandConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> ServiceQuerySqlStatementsTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, ServiceQuerySqlStatementConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> ServiceQuerySqlStatementsDataModelsTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLDataModelsAssemblyConvention, ServiceQuerySqlStatementConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> ServiceQueryTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, ServiceQueryConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> SqlStatementsTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, SqlStatementConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> SqlStatementsDataModelsTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLDataModelsAssemblyConvention, SqlStatementConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> DomainRuleTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, DomainRuleConvention>>()
                .GetTestParams();
        }
        public IEnumerable<ITestParams> DomainDataManagerTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, DomainDataManagerConvention>>()
                .GetTestParams();
        }

        public IEnumerable<ITestParams> EventTestParams()
        {
            return base.testingIoc.GetInstance<TestParamsProvider<SAHLServiceAssemblyConvention, EventConvention>>()
                .GetTestParams();

        }
    }
}