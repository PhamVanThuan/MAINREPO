using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeInsurableInterestTypeDataModel :  IDataModel
    {
        public LifeInsurableInterestTypeDataModel(int lifeInsurableInterestTypeKey, string description)
        {
            this.LifeInsurableInterestTypeKey = lifeInsurableInterestTypeKey;
            this.Description = description;
		
        }		

        public int LifeInsurableInterestTypeKey { get; set; }

        public string Description { get; set; }
    }
}