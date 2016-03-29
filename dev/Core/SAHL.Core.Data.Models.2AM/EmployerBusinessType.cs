using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmployerBusinessTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public EmployerBusinessTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public EmployerBusinessTypeDataModel(int employerBusinessTypeKey, string description)
        {
            this.EmployerBusinessTypeKey = employerBusinessTypeKey;
            this.Description = description;
		
        }		

        public int EmployerBusinessTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.EmployerBusinessTypeKey =  key;
        }
    }
}