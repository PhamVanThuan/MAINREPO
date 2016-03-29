using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentVerificationProcessTypeDataModel :  IDataModel
    {
        public EmploymentVerificationProcessTypeDataModel(int employmentVerificationProcessTypeKey, string description, int generalStatuskey)
        {
            this.EmploymentVerificationProcessTypeKey = employmentVerificationProcessTypeKey;
            this.Description = description;
            this.GeneralStatuskey = generalStatuskey;
		
        }		

        public int EmploymentVerificationProcessTypeKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatuskey { get; set; }
    }
}