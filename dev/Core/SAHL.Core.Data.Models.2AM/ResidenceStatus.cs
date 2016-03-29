using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ResidenceStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ResidenceStatusDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ResidenceStatusDataModel(int residenceStatusKey, string description)
        {
            this.ResidenceStatusKey = residenceStatusKey;
            this.Description = description;
		
        }		

        public int ResidenceStatusKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ResidenceStatusKey =  key;
        }
    }
}