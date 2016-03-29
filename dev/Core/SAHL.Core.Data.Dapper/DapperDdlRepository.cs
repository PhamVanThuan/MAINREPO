using Dapper;
using System;
using System.Data;

namespace SAHL.Core.Data.Dapper
{
    public class DapperDdlRepository : IDdlRepository
    {
        public IDbConnection DbConnection { get; set; }

        public void UseConnection(IDbConnection connection)
        {
            this.DbConnection = connection;
        }

        public int Truncate(string tableName)
        {
            var sqlQuery = string.Format("TRUNCATE TABLE {0}", tableName);
            return this.Execute(sqlQuery);
        }

        public int Create<TDataModel>(string tableName, string schema) where TDataModel : IDataModel
        {
            //Move over to FluentMapper
            CreateSchema(schema);
            var sqlCreateTableStatement = String.Format(@"if (not exists(select * from INFORMATION_SCHEMA.TABLES
                                                          where table_schema = '{0}'
                                                          and table_name = '{1}')){2}", schema, tableName, Environment.NewLine);
            sqlCreateTableStatement += "begin" + Environment.NewLine;
            sqlCreateTableStatement += String.Format("CREATE TABLE {0}.{1}{2}", schema, tableName, Environment.NewLine);
            sqlCreateTableStatement += "(";
            foreach (var property in typeof(TDataModel).GetProperties())
            {
                sqlCreateTableStatement += String.Format("{0} {1},{2}", property.Name, MapPropertyTypeToSqlType(property.PropertyType), Environment.NewLine);
            }
            sqlCreateTableStatement += ");" + Environment.NewLine;
            sqlCreateTableStatement += "end" + Environment.NewLine;
            return this.Execute(sqlCreateTableStatement);
        }

        private int CreateSchema(string schema)
        {
            string createSchemaSqlStatement = String.Format(@"if not exists (select * from sys.schemas where name = '{0}')
                                                              begin
                                                                  exec('create schema {0}')
                                                              end", schema);
            return this.Execute(createSchemaSqlStatement);
        }

        private string MapPropertyTypeToSqlType(Type propertyType)
        {
            if (propertyType == typeof(int))
            {
                return "int";
            }
            else if (propertyType == typeof(DateTime))
            {
                return "datetime";
            }
            else if (propertyType == typeof(decimal) ||
                     propertyType == typeof(double) ||
                     propertyType == typeof(float))
            {
                return "float";
            }
            return "varchar(255)";
        }

        private int Execute(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            int numRowsAffected = SqlMapper.Execute(this.DbConnection, sql, param, transaction, commandTimeout, commandType);
            return numRowsAffected;
        }
    }
}