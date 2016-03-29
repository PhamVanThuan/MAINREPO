using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValidatorTypeDataModel :  IDataModel
    {
        public ValidatorTypeDataModel(int validatorTypeKey, string description)
        {
            this.ValidatorTypeKey = validatorTypeKey;
            this.Description = description;
		
        }		

        public int ValidatorTypeKey { get; set; }

        public string Description { get; set; }
    }
}