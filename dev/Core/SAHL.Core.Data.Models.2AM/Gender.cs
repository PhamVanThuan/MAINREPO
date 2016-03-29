using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenderDataModel :  IDataModel
    {
        public GenderDataModel(int genderKey, string description)
        {
            this.GenderKey = genderKey;
            this.Description = description;
		
        }		

        public int GenderKey { get; set; }

        public string Description { get; set; }
    }
}