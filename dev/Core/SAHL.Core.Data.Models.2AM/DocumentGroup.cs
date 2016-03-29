using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentGroupDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public DocumentGroupDataModel(int documentGroupKey, string description)
        {
            this.DocumentGroupKey = documentGroupKey;
            this.Description = description;
		
        }		

        public int DocumentGroupKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.DocumentGroupKey =  key;
        }
    }
}