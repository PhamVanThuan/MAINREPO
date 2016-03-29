using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenericColumnDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GenericColumnDefinitionDataModel(string description, string tableName, string columnName)
        {
            this.Description = description;
            this.TableName = tableName;
            this.ColumnName = columnName;
		
        }
		[JsonConstructor]
        public GenericColumnDefinitionDataModel(int genericColumnDefinitionKey, string description, string tableName, string columnName)
        {
            this.GenericColumnDefinitionKey = genericColumnDefinitionKey;
            this.Description = description;
            this.TableName = tableName;
            this.ColumnName = columnName;
		
        }		

        public int GenericColumnDefinitionKey { get; set; }

        public string Description { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public void SetKey(int key)
        {
            this.GenericColumnDefinitionKey =  key;
        }
    }
}