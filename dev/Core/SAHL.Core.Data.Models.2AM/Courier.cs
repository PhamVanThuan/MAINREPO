using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CourierDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CourierDataModel(string courierName, string emailAddress)
        {
            this.CourierName = courierName;
            this.EmailAddress = emailAddress;
		
        }
		[JsonConstructor]
        public CourierDataModel(int courierKey, string courierName, string emailAddress)
        {
            this.CourierKey = courierKey;
            this.CourierName = courierName;
            this.EmailAddress = emailAddress;
		
        }		

        public int CourierKey { get; set; }

        public string CourierName { get; set; }

        public string EmailAddress { get; set; }

        public void SetKey(int key)
        {
            this.CourierKey =  key;
        }
    }
}