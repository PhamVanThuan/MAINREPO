using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AllocationMandateDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AllocationMandateDataModel(string name, string description, string typeName, int? parameterTypeKey, string parameterValue)
        {
            this.Name = name;
            this.Description = description;
            this.TypeName = typeName;
            this.ParameterTypeKey = parameterTypeKey;
            this.ParameterValue = parameterValue;
		
        }
		[JsonConstructor]
        public AllocationMandateDataModel(int allocationMandateKey, string name, string description, string typeName, int? parameterTypeKey, string parameterValue)
        {
            this.AllocationMandateKey = allocationMandateKey;
            this.Name = name;
            this.Description = description;
            this.TypeName = typeName;
            this.ParameterTypeKey = parameterTypeKey;
            this.ParameterValue = parameterValue;
		
        }		

        public int AllocationMandateKey { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TypeName { get; set; }

        public int? ParameterTypeKey { get; set; }

        public string ParameterValue { get; set; }

        public void SetKey(int key)
        {
            this.AllocationMandateKey =  key;
        }
    }
}