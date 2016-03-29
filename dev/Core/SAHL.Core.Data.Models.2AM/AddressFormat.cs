using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AddressFormatDataModel :  IDataModel
    {
        public AddressFormatDataModel(int addressFormatKey, string description)
        {
            this.AddressFormatKey = addressFormatKey;
            this.Description = description;
		
        }		

        public int AddressFormatKey { get; set; }

        public string Description { get; set; }
    }
}