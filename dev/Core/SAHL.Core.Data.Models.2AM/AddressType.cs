using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AddressTypeDataModel :  IDataModel
    {
        public AddressTypeDataModel(int addressTypeKey, string description)
        {
            this.AddressTypeKey = addressTypeKey;
            this.Description = description;
		
        }		

        public int AddressTypeKey { get; set; }

        public string Description { get; set; }
    }
}