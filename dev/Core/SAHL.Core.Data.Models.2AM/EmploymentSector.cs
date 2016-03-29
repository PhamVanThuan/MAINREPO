using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentSectorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public EmploymentSectorDataModel(string description, int generalStatusKey)
        {
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public EmploymentSectorDataModel(int employmentSectorKey, string description, int generalStatusKey)
        {
            this.EmploymentSectorKey = employmentSectorKey;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int EmploymentSectorKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.EmploymentSectorKey =  key;
        }
    }
}