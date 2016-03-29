using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenericKeyTypeDataModel :  IDataModel
    {
        public GenericKeyTypeDataModel(int genericKeyTypeKey, string description, string tableName, string primaryKeyColumn)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.Description = description;
            this.TableName = tableName;
            this.PrimaryKeyColumn = primaryKeyColumn;
		
        }		

        public int GenericKeyTypeKey { get; set; }

        public string Description { get; set; }

        public string TableName { get; set; }

        public string PrimaryKeyColumn { get; set; }
    }
}