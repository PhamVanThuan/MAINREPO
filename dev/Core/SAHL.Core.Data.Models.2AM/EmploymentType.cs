using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentTypeDataModel :  IDataModel
    {
        public EmploymentTypeDataModel(int employmentTypeKey, string description)
        {
            this.EmploymentTypeKey = employmentTypeKey;
            this.Description = description;
		
        }		

        public int EmploymentTypeKey { get; set; }

        public string Description { get; set; }
    }
}