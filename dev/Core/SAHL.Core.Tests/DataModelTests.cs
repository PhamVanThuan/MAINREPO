using Dapper;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models.X2;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Core.Testing
{
    [TestFixture]
    public class DataModelTests
    {
        [Test, TestCaseSource("GetDataModels")]
        public void EnsureModelsStillMeetDatabaseDefinition(DataModelsToTest modelToTest)
        {
            using (var connection = new SqlConnection(modelToTest.DbConfigProvider.ConnectionStringForApplicationRole))
            {
                connection.Open();
                try
                {
                    string commandText = string.Format(Strings.GetTableFields, modelToTest.DatabaseName, modelToTest.DatabaseTable);
                    var tableFields = connection.Query<DBTableField>(commandText).AsEnumerable();

                    var tableNotExistMessage = string.Format(" Server: {3}, Table {0} for model {1} does not exist in the {2} database."
                        , modelToTest.DatabaseTable
                        , modelToTest.FullName
                        , modelToTest.DatabaseName
                        , connection.DataSource
                        );
                    Assert.Greater(tableFields.Count(), 0, tableNotExistMessage);

                    //we should have the same number of properties/columns
                    var columnCountMismatchMessage = string.Format("Server: {2}, Model: {0} and Table: {1} do not have the same number of properties."
                        , modelToTest.Name
                        , modelToTest.DatabaseTable
                        , connection.DataSource
                        );
                    Assert.AreEqual(tableFields.Count(), modelToTest.Properties.Count(), columnCountMismatchMessage);

                    foreach (var tableField in tableFields)
                    {
                        //we should find the table property in the model
                        var modelProperty = (from m in modelToTest.Properties where m.Key == tableField.PropertyName select m).FirstOrDefault();
                        var columNotFoundMessage = string.Format(@"Server: {3}, Column: {0}.{1} not found in Data Model: {2}"
                            , connection.DataSource
                            , modelToTest.DatabaseTable
                            , tableField.PropertyName
                            , modelToTest.Name
                            );
                        Assert.That(modelProperty.Key != null, columNotFoundMessage);

                        //check types are equal
                        var dataTypeMismatchMessage = string.Format(@"Server: {4}, Type mismatch for Column: {0}.{1}. Expected {2} was {3}."
                            , modelToTest.DatabaseTable
                            , tableField.PropertyName
                            , tableField.TypeName
                            , modelProperty.Value
                            , connection.DataSource
                            );
                        StringAssert.AreEqualIgnoringCase(tableField.TypeName, modelProperty.Value, dataTypeMismatchMessage);
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public List<DataModelsToTest> GetDataModels()
        {
            var haloDbConfigurationProvider = new TestDbConfigurationProvider("Halo");
            var capitecDbConfigurationProvider = new TestDbConfigurationProvider("Capitec");
            var decisionTreeDbConfigurationProvider = new TestDbConfigurationProvider("DecisionTree");

            List<DataModelsToTest> models = new List<DataModelsToTest>();
            models.AddRange(GetDataModelsByType(typeof(AccountDataModel), "2AM", haloDbConfigurationProvider));
            models.AddRange(GetDataModelsByType(typeof(StateDataModel), "X2", haloDbConfigurationProvider));
            models.AddRange(GetDataModelsByType(typeof(ThroughputMetricMessageDataModel), "Cuttlefish", haloDbConfigurationProvider));
            models.AddRange(GetDataModelsByType(typeof(InvitationDataModel), "Capitec", capitecDbConfigurationProvider));
            models.AddRange(GetDataModelsByType(typeof(DecisionTreeDataModel), "DecisionTree", decisionTreeDbConfigurationProvider));
            models.AddRange(GetDataModelsByType(typeof(EventDataModel), "EventStore", haloDbConfigurationProvider));
            models.AddRange(GetDataModelsByType(typeof(CurrentStateForInstanceDataModel), "EventProjection", haloDbConfigurationProvider));
            return models;
        }

        private static List<DataModelsToTest> GetDataModelsByType(Type typeToTest, string databaseName, IDbConfigurationProvider dbConfigProvider)
        {
            List<DataModelsToTest> modelsToTest = new List<DataModelsToTest>();
            var assembly = Assembly.GetAssembly(typeToTest);
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.IsPublic &&
                            typeof(IDataModel).IsAssignableFrom(x)
                            ).ToList();
            foreach (var type in types)
            {
                modelsToTest.Add(new DataModelsToTest
                {
                    FullName = type.FullName,
                    Name = type.Name,
                    Properties = type.GetProperties()
                                .Where(x => x.PropertyType.IsPublic)
                                .ToDictionary(x => x.Name, x => x.PropertyType.ToString()),
                    DatabaseTable = type.Name.Replace("DataModel", string.Empty),
                    DatabaseName = databaseName,
                    DbConfigProvider = dbConfigProvider
                });
            }
            return modelsToTest;
        }

        private class DBTableField
        {
            public string Catalog { get; set; }

            public string Schema { get; set; }

            public string TableName { get; set; }

            public string TypeName { get; set; }

            public string PropertyName { get; set; }
        }

        public class DataModelsToTest
        {
            public string FullName { get; set; }

            public string Name { get; set; }

            public Dictionary<string, string> Properties { get; set; }

            public string DatabaseTable { get; set; }

            public string DatabaseName { get; set; }

            public IDbConfigurationProvider DbConfigProvider { get; set; }

            public override string ToString()
            {
                return FullName;
            }
        }
    }
}