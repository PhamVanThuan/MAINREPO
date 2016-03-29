using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentStatusDataModel :  IDataModel
    {
        public EmploymentStatusDataModel(int employmentStatusKey, string description)
        {
            this.EmploymentStatusKey = employmentStatusKey;
            this.Description = description;
		
        }		

        public int EmploymentStatusKey { get; set; }

        public string Description { get; set; }
    }
}