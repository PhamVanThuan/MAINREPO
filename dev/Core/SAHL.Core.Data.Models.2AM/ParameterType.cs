using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ParameterTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ParameterTypeDataModel(string description, string sQLDataType, string cSharpDataType)
        {
            this.Description = description;
            this.SQLDataType = sQLDataType;
            this.CSharpDataType = cSharpDataType;
		
        }
		[JsonConstructor]
        public ParameterTypeDataModel(int parameterTypeKey, string description, string sQLDataType, string cSharpDataType)
        {
            this.ParameterTypeKey = parameterTypeKey;
            this.Description = description;
            this.SQLDataType = sQLDataType;
            this.CSharpDataType = cSharpDataType;
		
        }		

        public int ParameterTypeKey { get; set; }

        public string Description { get; set; }

        public string SQLDataType { get; set; }

        public string CSharpDataType { get; set; }

        public void SetKey(int key)
        {
            this.ParameterTypeKey =  key;
        }
    }
}