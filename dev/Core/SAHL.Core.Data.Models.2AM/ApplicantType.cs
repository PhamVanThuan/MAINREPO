using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ApplicantTypeDataModel :  IDataModel
    {
        public ApplicantTypeDataModel(int applicantTypeKey, string description)
        {
            this.ApplicantTypeKey = applicantTypeKey;
            this.Description = description;
		
        }		

        public int ApplicantTypeKey { get; set; }

        public string Description { get; set; }
    }
}