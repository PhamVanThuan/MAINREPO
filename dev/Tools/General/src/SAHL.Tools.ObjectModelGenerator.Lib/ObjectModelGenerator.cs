using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Abstractions;
using Dapper;
using SAHL.Tools.ObjectModelGenerator.Lib.Templates;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SAHL.Tools.ObjectModelGenerator.Lib
{
    public class ObjectModelGenerator
    {
        private IFileSystem fileSystem;

        public ObjectModelGenerator(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void GenerateObjects(string connectionString, string outputDirectory, string include, string schema, string database, string defaultNamespace)
        {
            
            // check that the output directory exists
            IncludeFilter filter = new IncludeFilter(include);

            if (!this.fileSystem.Directory.Exists(outputDirectory))
            {
                throw new Exception("Output directory does not exist.");
            }

            IEnumerable<string> includes = null;

            if (filter.Includes.Contains("*"))
            {
                includes = this.GetTablesFromDB(connectionString, schema, database);
            }
            else
            {
                includes = filter.Includes.ToArray();
            }

            DeleteFiles(outputDirectory,includes);

            includes = filter.Filter(includes);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();

                foreach (string table in includes)
                {
                    string query = string.Format(Strings.GetProperties, table, schema);
                    IEnumerable<BusinessModelDescriptionProperty> properties = connection.Query<BusinessModelDescriptionProperty>(query);

                    IEnumerable<EnumerationPropertyValue> enumerationValues = Enumerable.Empty<EnumerationPropertyValue>();

                    
                    if (table.ToLower().EndsWith("enum") )
                    {
                        enumerationValues = connection.Query<EnumerationPropertyValue>(string.Format(Strings.GetEnumerationValues, table, schema));
                    }

                    if ((table.ToLower().EndsWith("type") && properties.Any(x => x.IsPrimaryKey && x.TypeName.Equals("Guid", StringComparison.OrdinalIgnoreCase))))
                    {
                        enumerationValues = connection.Query<EnumerationPropertyValue>(string.Format(Strings.GetGuidDecriptionTuple, table, schema));
                    }

                    BusinessModelDescription businessModelDescription = new BusinessModelDescription(table, database, schema, properties,enumerationValues);

                    if (businessModelDescription != null)
                    {
                        BusinessModelObject businessObject = new BusinessModelObject(businessModelDescription, defaultNamespace);
                        string generatedData = businessObject.TransformText();

                        DapperUIStatements uiStatements = new DapperUIStatements(businessModelDescription);
                        string generatedStatements = uiStatements.TransformText();
                        sb.AppendLine(generatedStatements);
                        sb.AppendLine();

                        string outputPath = this.fileSystem.Path.Combine(outputDirectory, string.Format("{0}.cs", table));
                        using (StreamWriter sw = new StreamWriter(outputPath))
                        {
                            sw.Write(generatedData);
                            sw.Flush();
                        }
                    }
                }

                UIStatementsRoot baseUIStatements = new UIStatementsRoot(defaultNamespace);
                string outputPathUIsBase = this.fileSystem.Path.Combine(outputDirectory, string.Format("{0}.cs", "UIStatements"));
                using (StreamWriter sw = new StreamWriter(outputPathUIsBase))
                {
                    sw.Write(baseUIStatements.TransformText());
                    sw.Flush();
                }                

                UIStatements statements = new UIStatements(defaultNamespace);
                statements.GeneratedStatements = sb.ToString();

                string outputPathUIs = this.fileSystem.Path.Combine(outputDirectory, string.Format("{0}_{1}.cs", "UIStatements", schema));
                using (StreamWriter sw = new StreamWriter(outputPathUIs))
                {
                    sw.Write(statements.TransformText());
                    sw.Flush();
                }

            }
        }

        public string[] GetTablesFromDB(string connectionString, string schema, string database)
        {
            List<string> systemTables = new List<string>(new string[] { "dtproperties", "sysarticlecolumns", "sysarticles", "sysarticleupdates", "sysdiagrams", "syspublications", "sysreplservers", "sysschemaarticles", "syssubscriptions", "systranschemas", "MSpeer_lsns", "MSpeer_request", "MSpeer_response", "MSpub_identity_range" });

            IEnumerable<INFORMATION_SCHEMA_Table> tables = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    tables = connection.Query<INFORMATION_SCHEMA_Table>(string.Format("select * from information_schema.tables where table_type = 'BASE TABLE' and TABLE_SCHEMA = '{0}'", schema));
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            tables = tables.Where(x => !systemTables.Contains(x.TABLE_NAME)).ToList();

            return tables.Select(x => x.TABLE_NAME).ToArray();
        }

        private void DeleteFiles(string outputDirectory,IEnumerable<string> filesToDelete)
        {
            Parallel.ForEach(filesToDelete, currentFile => {
                string filePath = Path.Combine(outputDirectory, currentFile+".cs");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            });
            
        }
    }
}