using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EducationDataModel :  IDataModel
    {
        public EducationDataModel(int educationKey, string description)
        {
            this.EducationKey = educationKey;
            this.Description = description;
		
        }		

        public int EducationKey { get; set; }

        public string Description { get; set; }
    }
}