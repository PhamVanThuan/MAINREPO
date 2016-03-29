using NUnit.Framework;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data.Configuration;
using SAHL.Core.UI.Halo.Tiles.LegalEntity.Default;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.UI.Halo.Tests
{
    [TestFixture]
    public class TileModelTests
    {
        private IDbConfigurationProvider dbConfigurationProvider;

        /// <summary>
        /// Get and execute SQL for all content providers
        /// Compare what is returned with the related model
        /// </summary>
        /// <param name="contentModel"></param>
        [Test, TestCaseSource(typeof(TileModelTests), "GetTileContentProviders")]
        public void EnsureContentProviderSQLColumnsMatchTileModelProperties(ContentModelsToTest contentModel)
        {
            // Get the SQL
            Type t = contentModel.ConfigurationType;
            dynamic instance = (dynamic)Activator.CreateInstance(t);
            string sqlQuery = instance.GetStatement(new BusinessKey(0, BusinessKeyType.Account));

            // Execute the SQL and assign the returned column values to the results list
            List<string> contentComparator = new List<string>();
            contentComparator = ExecuteSQL(sqlQuery);

            // Get public properties for the asociated tile model
            List<string> modelComparator = new List<string>();

            foreach (var modelProperty in contentModel.ModelType.GetProperties())
            {
                modelComparator.Add(modelProperty.Name);
            }

            Assert.AreEqual(contentComparator, modelComparator, "The SQL column names for content provider {0} do not match the properties of tile model {1}.", contentModel.Name, contentModel.ModelType.Name);
        }        

        /// <summary>
        /// Gets all the content providers.
        /// </summary>
        /// <returns>contentModelsToTest</returns>
        private List<ContentModelsToTest> GetTileContentProviders()
        {
            List<ContentModelsToTest> contentModelsToTest = new List<ContentModelsToTest>();
            var assembly = Assembly.GetAssembly(typeof(LegalEntityMajorTileContentProvider));
            List<Type> types = assembly.GetTypes().Where(x =>
                                !x.IsInterface &&
                                !x.IsGenericType &&
                                !x.IsAbstract &&
                                x.IsPublic &&
                                x.AssemblyQualifiedName.ToLower().Contains("content")
                            ).ToList();
            foreach (var type in types)
            {
                contentModelsToTest.Add(new ContentModelsToTest
                {
                    Name = type.Name,
                    ModelType = type.BaseType.GetGenericArguments()[0],
                    ConfigurationType = type
                });
            }
            return contentModelsToTest;
        }

        /// <summary>
        /// Defines content model
        /// </summary>
        public class ContentModelsToTest
        {
            public string Name { get; set; }

            public Type ModelType { get; set; }

            public Type ConfigurationType { get; set; }

            public MethodInfo Method { get; set; }

            public override string ToString()
            {
                return this.Name.Replace("SAHL.Core.UI.Halo.Tiles", "");
            }
        }        

        /// <summary>
        /// Gets a connection string
        /// </summary>
        /// <returns>ConnectionStringForApplicationRole</returns>
        private string GetConnectionStringForAppRole()
        {
            if (this.dbConfigurationProvider == null)
            {
                this.dbConfigurationProvider = new DefaultDbConfigurationProvider();
            }

            return this.dbConfigurationProvider.ConnectionStringForApplicationRole;
        }

        /// <summary>
        /// Executes the SQL
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns>SQL Column names</returns>
        private List<string> ExecuteSQL(string sqlQuery)
        {
            string connectionString = this.GetConnectionStringForAppRole();
            List<string> sqlResults = new List<string>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sqlResults.Add(reader.GetName(i).ToString());
                    }
                    
                }
            }
            return sqlResults;
        }
    }
}