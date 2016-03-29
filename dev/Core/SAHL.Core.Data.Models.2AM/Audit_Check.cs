using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class Audit_CheckDataModel :  IDataModel
    {
        public Audit_CheckDataModel(string tableName, long? count)
        {
            this.TableName = tableName;
            this.Count = count;
		
        }		

        public string TableName { get; set; }

        public long? Count { get; set; }
    }
}