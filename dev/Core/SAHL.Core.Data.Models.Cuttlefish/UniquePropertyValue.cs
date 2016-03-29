using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class UniquePropertyValueDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public UniquePropertyValueDataModel(string propertyValue, int? uniquePropertyDefinition_id)
        {
            this.PropertyValue = propertyValue;
            this.UniquePropertyDefinition_id = uniquePropertyDefinition_id;
		
        }

        public UniquePropertyValueDataModel(int id, string propertyValue, int? uniquePropertyDefinition_id)
        {
            this.Id = id;
            this.PropertyValue = propertyValue;
            this.UniquePropertyDefinition_id = uniquePropertyDefinition_id;
		
        }		

        public int Id { get; set; }

        public string PropertyValue { get; set; }

        public int? UniquePropertyDefinition_id { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}