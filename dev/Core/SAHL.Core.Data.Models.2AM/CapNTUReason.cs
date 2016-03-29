using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapNTUReasonDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapNTUReasonDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public CapNTUReasonDataModel(int capNTUReasonKey, string description)
        {
            this.CapNTUReasonKey = capNTUReasonKey;
            this.Description = description;
		
        }		

        public int CapNTUReasonKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.CapNTUReasonKey =  key;
        }
    }
}