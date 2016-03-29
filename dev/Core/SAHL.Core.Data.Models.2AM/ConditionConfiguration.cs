using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionConfigurationDataModel(int genericKeyTypeKey, int genericColumnDefinitionKey, int genericColumnDefinitionValue)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericColumnDefinitionKey = genericColumnDefinitionKey;
            this.GenericColumnDefinitionValue = genericColumnDefinitionValue;
		
        }
		[JsonConstructor]
        public ConditionConfigurationDataModel(int conditionConfigurationKey, int genericKeyTypeKey, int genericColumnDefinitionKey, int genericColumnDefinitionValue)
        {
            this.ConditionConfigurationKey = conditionConfigurationKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericColumnDefinitionKey = genericColumnDefinitionKey;
            this.GenericColumnDefinitionValue = genericColumnDefinitionValue;
		
        }		

        public int ConditionConfigurationKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericColumnDefinitionKey { get; set; }

        public int GenericColumnDefinitionValue { get; set; }

        public void SetKey(int key)
        {
            this.ConditionConfigurationKey =  key;
        }
    }
}