using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class UniquePropertyDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public UniquePropertyDefinitionDataModel(string displayName, string propertyName, bool? isParameterProperty, int? applicationModuleDefinition_id)
        {
            this.DisplayName = displayName;
            this.PropertyName = propertyName;
            this.IsParameterProperty = isParameterProperty;
            this.ApplicationModuleDefinition_id = applicationModuleDefinition_id;
		
        }

        public UniquePropertyDefinitionDataModel(int id, string displayName, string propertyName, bool? isParameterProperty, int? applicationModuleDefinition_id)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.PropertyName = propertyName;
            this.IsParameterProperty = isParameterProperty;
            this.ApplicationModuleDefinition_id = applicationModuleDefinition_id;
		
        }		

        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string PropertyName { get; set; }

        public bool? IsParameterProperty { get; set; }

        public int? ApplicationModuleDefinition_id { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}