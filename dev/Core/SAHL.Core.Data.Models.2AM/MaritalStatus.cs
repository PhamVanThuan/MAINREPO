using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MaritalStatusDataModel :  IDataModel
    {
        public MaritalStatusDataModel(int maritalStatusKey, string description)
        {
            this.MaritalStatusKey = maritalStatusKey;
            this.Description = description;
		
        }		

        public int MaritalStatusKey { get; set; }

        public string Description { get; set; }
    }
}