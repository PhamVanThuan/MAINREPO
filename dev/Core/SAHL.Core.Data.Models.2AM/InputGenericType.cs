using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InputGenericTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public InputGenericTypeDataModel(int coreBusinessObjectKey, int? genericKeyTypeParameterKey)
        {
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.GenericKeyTypeParameterKey = genericKeyTypeParameterKey;
		
        }
		[JsonConstructor]
        public InputGenericTypeDataModel(int inputGenericTypeKey, int coreBusinessObjectKey, int? genericKeyTypeParameterKey)
        {
            this.InputGenericTypeKey = inputGenericTypeKey;
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.GenericKeyTypeParameterKey = genericKeyTypeParameterKey;
		
        }		

        public int InputGenericTypeKey { get; set; }

        public int CoreBusinessObjectKey { get; set; }

        public int? GenericKeyTypeParameterKey { get; set; }

        public void SetKey(int key)
        {
            this.InputGenericTypeKey =  key;
        }
    }
}