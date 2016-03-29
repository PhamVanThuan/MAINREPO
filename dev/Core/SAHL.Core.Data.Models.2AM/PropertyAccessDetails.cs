using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PropertyAccessDetailsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PropertyAccessDetailsDataModel(int propertyKey, string contact1, string contact1Phone, string contact1WorkPhone, string contact1MobilePhone, string contact2, string contact2Phone)
        {
            this.PropertyKey = propertyKey;
            this.Contact1 = contact1;
            this.Contact1Phone = contact1Phone;
            this.Contact1WorkPhone = contact1WorkPhone;
            this.Contact1MobilePhone = contact1MobilePhone;
            this.Contact2 = contact2;
            this.Contact2Phone = contact2Phone;
		
        }
		[JsonConstructor]
        public PropertyAccessDetailsDataModel(int propertyAccessDetailsKey, int propertyKey, string contact1, string contact1Phone, string contact1WorkPhone, string contact1MobilePhone, string contact2, string contact2Phone)
        {
            this.PropertyAccessDetailsKey = propertyAccessDetailsKey;
            this.PropertyKey = propertyKey;
            this.Contact1 = contact1;
            this.Contact1Phone = contact1Phone;
            this.Contact1WorkPhone = contact1WorkPhone;
            this.Contact1MobilePhone = contact1MobilePhone;
            this.Contact2 = contact2;
            this.Contact2Phone = contact2Phone;
		
        }		

        public int PropertyAccessDetailsKey { get; set; }

        public int PropertyKey { get; set; }

        public string Contact1 { get; set; }

        public string Contact1Phone { get; set; }

        public string Contact1WorkPhone { get; set; }

        public string Contact1MobilePhone { get; set; }

        public string Contact2 { get; set; }

        public string Contact2Phone { get; set; }

        public void SetKey(int key)
        {
            this.PropertyAccessDetailsKey =  key;
        }
    }
}