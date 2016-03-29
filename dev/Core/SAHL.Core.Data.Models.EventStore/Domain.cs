using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventStore
{
    [Serializable]
    public partial class DomainDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DomainDataModel(string name)
        {
            this.Name = name;
		
        }
		[JsonConstructor]
        public DomainDataModel(int domainKey, string name)
        {
            this.DomainKey = domainKey;
            this.Name = name;
		
        }		

        public int DomainKey { get; set; }

        public string Name { get; set; }

        public void SetKey(int key)
        {
            this.DomainKey =  key;
        }
    }
}