using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenericKeyTypeParameterDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GenericKeyTypeParameterDataModel(int genericKeyTypeKey, string parameterName, int parameterTypeKey)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.ParameterName = parameterName;
            this.ParameterTypeKey = parameterTypeKey;
		
        }
		[JsonConstructor]
        public GenericKeyTypeParameterDataModel(int genericKeyTypeParameterKey, int genericKeyTypeKey, string parameterName, int parameterTypeKey)
        {
            this.GenericKeyTypeParameterKey = genericKeyTypeParameterKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.ParameterName = parameterName;
            this.ParameterTypeKey = parameterTypeKey;
		
        }		

        public int GenericKeyTypeParameterKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public string ParameterName { get; set; }

        public int ParameterTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.GenericKeyTypeParameterKey =  key;
        }
    }
}