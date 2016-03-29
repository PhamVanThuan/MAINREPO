using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentConfirmationSourceDataModel :  IDataModel
    {
        public EmploymentConfirmationSourceDataModel(int employmentConfirmationSourceKey, string description, int generalStatusKey)
        {
            this.EmploymentConfirmationSourceKey = employmentConfirmationSourceKey;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int EmploymentConfirmationSourceKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}