using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MandateTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MandateTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public MandateTypeDataModel(int mandateTypeKey, string description)
        {
            this.MandateTypeKey = mandateTypeKey;
            this.Description = description;
		
        }		

        public int MandateTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.MandateTypeKey =  key;
        }
    }
}